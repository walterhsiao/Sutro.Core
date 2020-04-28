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

        public virtual void AppendCurveSets(List<FillCurveSet2d> fillSets)
        {
            OnAppendCurveSetsF?.Invoke(fillSets, this);
            foreach (var curveSet in fillSets)
            {
                foreach (var curve in curveSet.Curves)
                    AppendFillCurve(curve);

                foreach (var loop in curveSet.Loops)
                    AppendFillLoop (loop);
            }
        }

        // [TODO] no reason we couldn't start on edge midpoint??
        public virtual void AppendFillLoop<TSegment>(FillLoop<TSegment> loop) 
            where TSegment : IFillSegment, new()
        {
            AssertValidLoop(loop);

            var oriented = SelectLoopDirection(loop);
            var rolled = SelectLoopEntry(oriented, Builder.Position.xy);

            AppendTravel(Builder.Position.xy, rolled.EntryExitPoint);

            double useSpeed = SelectSpeed(rolled);
            BuildLoop(rolled, useSpeed);
        }

        protected virtual FillLoop<TSegment> SelectLoopEntry<TSegment>(FillLoop<TSegment> loop, Vector2d currentPosition)
            where TSegment : IFillSegment, new()

        {
            int startIndex = FindLoopEntryPoint(loop, currentPosition);
            return loop.RollToVertex(startIndex);
        }

        protected virtual void BuildLoop<TSegment>(FillLoop<TSegment> rolled, double useSpeed) where TSegment : IFillSegment, new()
        {
            Builder.AppendExtrude(rolled.Vertices(true).ToList(), useSpeed, rolled.FillType, null);
        }

        protected virtual FillLoop<TSegment> SelectLoopDirection<TSegment>(FillLoop<TSegment> loop)
            where TSegment : IFillSegment, new()
        {
            return loop;
        }

        protected int FindLoopEntryPoint<TSegment>(FillLoop<TSegment> poly, Vector2d currentPos2)
            where TSegment : IFillSegment, new()
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
        public virtual void AppendFillCurve<TSegment>(FillCurve<TSegment> curve)
            where TSegment : IFillSegment, new()
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            AssertValidCurve(curve);

            if (curve.Entry.DistanceSquared(currentPos2) > curve.Exit.DistanceSquared(currentPos2))
            {
                curve = curve.Reversed();
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
                    if (segInfo.IsConnector)
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
        public virtual double SelectSpeed<TSegment>(FillBase<TSegment> pathCurve)
            where TSegment : IFillSegment, new()
        {
            double speed = SpeedHint == SchedulerSpeedHint.Careful ?
                Settings.CarefulExtrudeSpeed : Settings.RapidExtrudeSpeed;

            return pathCurve.FillType.ModifySpeed(speed, SpeedHint);
        }

        protected void AssertValidCurve<TSegment>(FillCurve<TSegment> curve)
            where TSegment : IFillSegment, new()
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

        protected void AssertValidLoop<TSegment>(FillLoop<TSegment> curve)
            where TSegment : IFillSegment, new()
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