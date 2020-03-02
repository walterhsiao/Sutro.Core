﻿using g3;
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
                foreach (FillPolygon2d loop in polySet.Loops)
                {
                    AppendPolygon2d(loop);
                }
                foreach (FillPolyline2d curve in polySet.Curves)
                {
                    AppendPolyline2d(curve);
                }
            }
        }

        // [TODO] no reason we couldn't start on edge midpoint??
        public virtual void AppendPolygon2d(FillPolygon2d poly)
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            int N = poly.VertexCount;
            if (N < 2)
                throw new Exception("PathScheduler.AppendPolygon2d: degenerate curve!");

            bool isOutermostShell = poly.HasTypeFlag(FillTypeFlags.OutermostShell);

            int startIndex;
            if (Settings.ZipperAlignedToPoint && isOutermostShell)
            {
                // split edges to position zipper closer to the desired point?
                Vector2d zipperLocation = new Vector2d(Settings.ZipperLocationX, Settings.ZipperLocationY);
                startIndex = CurveUtils2.FindNearestVertex(zipperLocation, poly.Vertices);
            }
            else if (Settings.ShellRandomizeStart && isOutermostShell)
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

            double useSpeed = select_speed(poly.TypeFlags);

            Builder.AppendExtrude(loopV, useSpeed, poly.TypeFlags, null);
        }

        protected void AppendTravel(Vector2d startPt, Vector2d endPt)
        {
            double travelDistance = startPt.Distance(endPt);

            // a travel may require a retract, which we might want to skip
            if (ExtrudeOnShortTravels &&
                travelDistance < ShortTravelDistance)
            {
                Builder.AppendExtrude(endPt, Settings.RapidTravelSpeed);
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
        public virtual void AppendPolyline2d(FillPolyline2d curve)
        {
            Vector3d currentPos = Builder.Position;
            Vector2d currentPos2 = currentPos.xy;

            int N = curve.VertexCount;
            if (N < 2)
                throw new Exception("PathScheduler.AppendPolyline2d: degenerate curve!");

            int iNearest = 0;
            bool bReverse = false;
            if (curve.Start.DistanceSquared(currentPos2) > curve.End.DistanceSquared(currentPos2))
            {
                iNearest = N - 1;
                bReverse = true;
            }

            Vector2d startPt = curve[iNearest];
            AppendTravel(currentPos2, startPt);

            List<Vector2d> loopV;
            List<TPVertexFlags> flags = null;

            loopV = new List<Vector2d>(N);
            flags = new List<TPVertexFlags>(N);

            var range = Enumerable.Range(0, N);
            if (bReverse) range.Reverse();

            foreach (int i in range)
            {
                var point = curve.GetPoint(i, bReverse);
                loopV.Add(point.Vertex);
                flags.Add(point.SegmentInfo != null && point.SegmentInfo.IsConnector ? TPVertexFlags.IsConnector : TPVertexFlags.None);
            }
            double useSpeed = select_speed(curve.TypeFlags);

            Vector2d dimensions = GCodeUtil.UnspecifiedDimensions;
            if (curve.CustomThickness > 0)
                dimensions.x = curve.CustomThickness;

            Builder.AppendExtrude(loopV, useSpeed, dimensions, curve.TypeFlags, flags);
        }

        private bool HasTypeFlag(FillTypeFlags typeFlags, FillTypeFlags f)
        {
            return (typeFlags & f) != 0;
        }

        // 1) If we have "careful" speed hint set, use CarefulExtrudeSpeed
        //       (currently this is only set on first layer)
        // 2) if this is an outer perimeter, scale by outer perimeter speed multiplier
        // 3) if we are being "careful" and this is support, also use that multiplier
        //       (bit of a hack, currently means on first layer we do support extra slow)
        protected virtual double select_speed(FillTypeFlags flags)
        {
            bool bIsSupport = HasTypeFlag(flags, FillTypeFlags.SupportMaterial);
            bool bIsOuterPerimeter = HasTypeFlag(flags, FillTypeFlags.OuterPerimeter);
            bool bCareful = (SpeedHint == SchedulerSpeedHint.Careful);
            double useSpeed = bCareful ? Settings.CarefulExtrudeSpeed : Settings.RapidExtrudeSpeed;
            if (bIsOuterPerimeter || (bCareful && bIsSupport))
                useSpeed *= Settings.OuterPerimeterSpeedX;

            bool bIsBridgeSupport = HasTypeFlag(flags, FillTypeFlags.BridgeSupport);
            if (bIsBridgeSupport)
                useSpeed = Settings.CarefulExtrudeSpeed * Settings.BridgeExtrudeSpeedX;

            return useSpeed;
        }
    }
}