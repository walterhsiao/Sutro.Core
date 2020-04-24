using g3;
using gs.FillTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace gs
{
    public enum SchedulerSpeedHint
    {
        Careful, Default, Rapid, MaxSpeed
    }

    public interface IFillPathScheduler2d
    {
        void AppendCurveSets(List<FillCurveSet2d> paths);

        SchedulerSpeedHint SpeedHint { get; set; }

        Vector2d CurrentPosition { get; }

    }

    // dumbest possible scheduler...
    public class SequentialScheduler2d : IFillPathScheduler2d
    {
        public ToolpathSetBuilder Builder;
        public SingleMaterialFFFSettings Settings;

        public bool ExtrudeOnShortTravels = false;
        public double ShortTravelDistance = 0;

        // optional function we will call when curve sets are appended
        public Action<List<FillCurveSet2d>, SequentialScheduler2d> OnAppendCurveSetsF = null;

        public SequentialScheduler2d(ToolpathSetBuilder builder, SingleMaterialFFFSettings settings)
        {
            Builder = builder;
            Settings = settings;
        }

        private SchedulerSpeedHint speed_hint = SchedulerSpeedHint.Default;

        public virtual SchedulerSpeedHint SpeedHint
        {
            get { return speed_hint; }
            set { speed_hint = value; }
        }

        public Vector2d CurrentPosition => Builder.Position.xy;

        protected virtual void AppendFill(FillBase<FillSegment> fill)
        {
            switch (fill)
            {
                case FillLoop<FillSegment> basicFillLoop:
                    AppendBasicFillLoop(basicFillLoop);
                    break;
                case FillCurve<FillSegment> basicFillCurve:
                    AppendBasicFillCurve(basicFillCurve);
                    break;
                default:
                    throw new NotImplementedException($"{fill.GetType()} not supported by {nameof(SequentialScheduler2d)}");
            }
        }

        public virtual void AppendCurveSets(List<FillCurveSet2d> fillSets)
        {
            OnAppendCurveSetsF?.Invoke(fillSets, this);
            foreach (var fill in FlattenFillCurveSets(fillSets))
            {
                AppendFill(fill);
            }
        }

        protected static List<FillBase<FillSegment>> FlattenFillCurveSets(List<FillCurveSet2d> fillSets)
        {
            var fillElements = new List<FillBase<FillSegment>>();

            foreach (var fills in fillSets)
            {
                fillElements.AddRange(fills.Loops);
                fillElements.AddRange(fills.Curves);
            }

            return fillElements;
        }

        // [TODO] no reason we couldn't start on edge midpoint??
        public virtual void AppendBasicFillLoop(FillLoop<FillSegment> poly)
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            AssertValidLoop(poly);

            int startIndex = FindLoopEntryPoint(poly, currentPos2);
            var rolled = poly.RollToVertex(startIndex);

            AppendTravel(currentPos2, rolled.EntryExitPoint);

            double useSpeed = SelectSpeed(poly);

            Builder.AppendExtrude(rolled.Vertices().ToList(), useSpeed, poly.FillType, null);
        }

        private int FindLoopEntryPoint(FillLoop<FillSegment> poly, Vector2d currentPos2)
        {
            int startIndex;
            if (Settings.ZipperAlignedToPoint && poly.FillType.IsEntryLocationSpecified())
            {
                // split edges to position zipper closer to the desired point?
                Vector2d zipperLocation = new Vector2d(Settings.ZipperLocationX, Settings.ZipperLocationY);
                startIndex = CurveUtils2.FindNearestVertex(zipperLocation, poly.Vertices());
            }
            else if (Settings.ShellRandomizeStart && poly.FillType.IsEntryLocationSpecified())
            {
                // split edges for a actual random location along the perimeter instead of a random vertex?
                Random rnd = new Random();
                startIndex = rnd.Next(poly.Elements.Count);
            }
            else
            {
                // use the vertex closest to the current nozzle position
                startIndex = CurveUtils2.FindNearestVertex(currentPos2, poly.Vertices());
            }

            return startIndex;
        }

        protected void AppendTravel(Vector2d startPt, Vector2d endPt)
        {
            double travelDistance = startPt.Distance(endPt);

            // a travel may require a retract, which we might want to skip
            if (ExtrudeOnShortTravels &&
                travelDistance < ShortTravelDistance)
            {
                // TODO: Add strategy for extrude move?
                Builder.AppendExtrude(endPt, Settings.RapidTravelSpeed, new DefaultFillType());
            }
            else if (Settings.TravelLiftEnabled &&
                travelDistance > Settings.TravelLiftDistanceThreshold)
            {
                Builder.AppendZChange(Settings.TravelLiftHeight, Settings.ZTravelSpeed, ToolpathTypes.Travel);
                Builder.AppendTravel(endPt, Settings.RapidTravelSpeed);
                Builder.AppendZChange(-Settings.TravelLiftHeight, Settings.ZTravelSpeed, ToolpathTypes.Travel);
            }
            else
            {
                Builder.AppendTravel(endPt, Settings.RapidTravelSpeed);
            }
        }

        // [TODO] would it ever make sense to break polyline to avoid huge travel??
        public virtual void AppendBasicFillCurve(FillCurve<FillSegment> curve)
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            AssertValidCurve(curve);

            if (curve.Entry.DistanceSquared(currentPos2) > curve.Exit.DistanceSquared(currentPos2))
            {
                curve = new FillCurve<FillSegment>(curve.ElementsReversed());
            }

            AppendTravel(currentPos2, curve.Entry);

            var vertices = curve.Vertices().ToList();

            var flags = new List<TPVertexFlags>(curve.Elements.Count + 1);
            for (int i = 0; i < vertices.Count; i++)
            {
                var flag = TPVertexFlags.None;

                if (i == 0)
                    flag = TPVertexFlags.IsPathStart;
                else
                {
                    var segInfo = curve.Elements[i - 1].Edge;
                    if (segInfo != null && segInfo.IsConnector)
                        flag = TPVertexFlags.IsConnector;
                }

                flags.Add(flag);
            }

            double useSpeed = SelectSpeed(curve);

            Vector2d dimensions = GCodeUtil.UnspecifiedDimensions;
            if (curve.FillThickness > 0)
                dimensions.x = curve.FillThickness;
            Builder.AppendExtrude(vertices, useSpeed, dimensions, curve.FillType, curve.IsHoleShell, flags);
        }

        // 1) If we have "careful" speed hint set, use CarefulExtrudeSpeed
        //       (currently this is only set on first layer)
        public virtual double SelectSpeed(FillBase<FillSegment> pathCurve)
        {
            double speed = SpeedHint == SchedulerSpeedHint.Careful ?
                Settings.CarefulExtrudeSpeed : Settings.RapidExtrudeSpeed;

            return pathCurve.FillType.ModifySpeed(speed, SpeedHint);
        }

        protected void AssertValidCurve(FillCurve<FillSegment> curve)
        {
            int N = curve.Elements.Count;
            if (N < 1)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;
                throw new ArgumentException($"{type}.{name}: degenerate curve; must have at least 1 edge.");
            }
        }

        protected void AssertValidLoop(FillLoop<FillSegment> curve)
        {
            int N = curve.Elements.Count;
            if (N < 2)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;
                throw new ArgumentException($"{type}.{name}: degenerate loop; must have at least 2 edges");
            }
        }
    }
}