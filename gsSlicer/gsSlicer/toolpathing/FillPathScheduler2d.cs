using g3;
using gs.FillTypes;
using System;
using System.Collections.Generic;
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

        public virtual void AppendCurveSets(List<FillCurveSet2d> paths)
        {
            if (OnAppendCurveSetsF != null)
                OnAppendCurveSetsF(paths, this);

            foreach (FillCurveSet2d polySet in paths)
            {
                foreach (BasicFillLoop loop in polySet.Loops)
                {
                    AppendPolygon2d(loop);
                }
                foreach (BasicFillCurve curve in polySet.Curves)
                {
                    AppendPolyline2d(curve);
                }
            }
        }

        // [TODO] no reason we couldn't start on edge midpoint??
        public virtual void AppendPolygon2d(BasicFillLoop poly)
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            int N = poly.VertexCount;
            if (N < 2)
                throw new Exception("PathScheduler.AppendPolygon2d: degenerate curve!");

            int startIndex;
            if (Settings.ZipperAlignedToPoint && poly.FillType.IsEntryLocationSpecified())
            {
                // split edges to position zipper closer to the desired point?
                Vector2d zipperLocation = new Vector2d(Settings.ZipperLocationX, Settings.ZipperLocationY);
                startIndex = CurveUtils2.FindNearestVertex(zipperLocation, poly.Vertices);
            }
            else if (Settings.ShellRandomizeStart && poly.FillType.IsEntryLocationSpecified())
            {
                // split edges for a actual random location along the perimeter instead of a random vertex?
                Random rnd = new Random();
                startIndex = rnd.Next(poly.VertexCount);
            }
            else
            {
                // use the vertex closest to the current nozzle position
                startIndex = CurveUtils2.FindNearestVertex(currentPos2, poly.Vertices);
            }

            Vector2d startPt = poly[startIndex];

            AppendTravel(currentPos2, startPt);

            List<Vector2d> loopV = new List<Vector2d>(N + 1);
            for (int i = 0; i <= N; i++)
            {
                int k = (startIndex + i) % N;
                loopV.Add(poly[k]);
            }

            double useSpeed = select_speed(poly);

            Builder.AppendExtrude(loopV, useSpeed, poly.FillType, null);
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
        public virtual void AppendPolyline2d(BasicFillCurve curve)
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            if (curve.VertexCount < 2)
                throw new Exception("PathScheduler.AppendPolyline2d: degenerate curve!");

            if (curve.Start.DistanceSquared(currentPos2) > curve.End.DistanceSquared(currentPos2))
            {
                // This modifies input; avoid?
                curve.Reverse();
            }

            AppendTravel(currentPos2, curve[0]);

            var flags = new List<TPVertexFlags>(curve.VertexCount);

            var vertices = new List<Vector2d>(curve.VertexCount);
            for (int i = 0; i < curve.VertexCount; i++)
            {
                var p = curve.GetPoint(i, false);
                var flag = TPVertexFlags.None;

                if (i == 0)
                    flag = TPVertexFlags.IsPathStart;
                else if (p.SegmentInfo != null && p.SegmentInfo.IsConnector)
                    flag = TPVertexFlags.IsConnector;

                vertices.Add(p.Vertex);
                flags.Add(flag);
            }

            double useSpeed = select_speed(curve);

            Vector2d dimensions = GCodeUtil.UnspecifiedDimensions;
            if (curve.CustomThickness > 0)
                dimensions.x = curve.CustomThickness;
            Builder.AppendExtrude(vertices, useSpeed, dimensions, curve.FillType, flags);
        }

        // 1) If we have "careful" speed hint set, use CarefulExtrudeSpeed
        //       (currently this is only set on first layer)
        protected virtual double select_speed(IFillElement pathCurve)
        {
            double speed = SpeedHint == SchedulerSpeedHint.Careful ?
                Settings.CarefulExtrudeSpeed : Settings.RapidExtrudeSpeed;

            return pathCurve.FillType.ModifySpeed(speed, SpeedHint);
        }
    }
}