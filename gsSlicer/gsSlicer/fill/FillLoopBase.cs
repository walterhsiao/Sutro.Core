using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    /// <summary>
    /// Additive polygon fill curve
    /// </summary>
    public abstract class FillLoopBase<TVertexInfo, TSegmentInfo> :
        FillElementBase<TVertexInfo, TSegmentInfo>, IFillLoop
        where TVertexInfo : BasicVertexInfo, new()
        where TSegmentInfo : BasicSegmentInfo, new()
    {
        public abstract FillLoopBase<TVertexInfo, TSegmentInfo> CloneBare();

        public Polygon2d Polygon { get; protected set; } = new Polygon2d();
        protected List<TSegmentInfo> SegmentInfo = new List<TSegmentInfo>();
        protected List<TVertexInfo> VertexInfo = new List<TVertexInfo>();
        public double CustomThickness { get; set; }

        // Pass through some properties & methods from wrapped Polygon

        public bool IsClockwise { get => Polygon.IsClockwise; }
        public double Perimeter { get => Polygon.Perimeter; }
        public int VertexCount { get => Polygon.VertexCount; }
        public int SegmentCount { get => SegmentInfo.Count; }
        public IEnumerable<Vector2d> Vertices { get => Polygon.VerticesItr(false); }
        public bool IsHoleShell { get; set; }

        public Vector2d EntryExitPoint => Polygon.Vertices[0];

        public AxisAlignedBox2d Bounds => Polygon.GetBounds();

        public int PerimOrder { get; set; } = -1;

        public Vector2d this[int i] { get => Polygon[i]; }

        private bool loopStarted = false;
        private bool loopFinished = false;

        public void BeginOrAppendCurve(Vector2d pt, TVertexInfo vInfo = null)
        {
            Polygon.AppendVertex(pt);
            VertexInfo.Add(vInfo);
            if (loopStarted)
            {
                SegmentInfo.Add(null);
            }
            else
            {
                loopStarted = true;
            }
        }

        public void BeginLoop(Vector2d pt, TVertexInfo vInfo = null)
        {
            if (loopStarted)
                throw new MethodAccessException("BeginLoop called more than once.");

            loopStarted = true;

            Polygon.AppendVertex(pt);
            VertexInfo.Add(vInfo);
        }

        public void AddToLoop(Vector2d pt, TVertexInfo vInfo = null, TSegmentInfo sInfo = null)
        {
            if (!loopStarted)
                throw new MethodAccessException("AddToLoop called before BeginLoop.");
            if (loopFinished)
                throw new MethodAccessException("AddToLoop called after CloseLoop");

            Polygon.AppendVertex(pt);
            VertexInfo.Add(vInfo);
            SegmentInfo.Add(sInfo);
        }

        public void CloseLoop(TSegmentInfo sInfo = null)
        {
            if (!loopStarted)
                throw new MethodAccessException("CloseLoop called before BeginLoop.");
            if (loopFinished)
                throw new MethodAccessException("CloseLoop called more than once.");

            loopFinished = true;

            SegmentInfo.Add(sInfo);
        }

        public double DistanceSquared(Vector2d pt, out int iNearSeg, out double fNearSegT)
        {
            return Polygon.DistanceSquared(pt, out iNearSeg, out fNearSegT);
        }

        protected Vector2d InterpolateVertex(Vector2d vertexA, Vector2d vertexB, double param)
        {
            return vertexA * (1 - param) + vertexB * param;
        }

        protected abstract TVertexInfo InterpolateVertexInfo(TVertexInfo vertexInfoA, TVertexInfo vertexInfoB, double param);

        protected abstract Tuple<TSegmentInfo, TSegmentInfo> SplitSegmentInfo(TSegmentInfo segmentInfo, double param);

        public void Roll(int k)
        {
            // Make copies of current lists
            var rolledVertices = new List<Vector2d>(VertexCount);
            var rolledVertexInfo = new List<TVertexInfo>(VertexCount);
            var rolledSegmentInfo = new List<TSegmentInfo>(VertexCount);

            for (int i = 0; i < VertexCount; i++)
            {
                int j = (i + k) % VertexCount;
                rolledVertices.Add(Polygon.Vertices[j]);
                rolledVertexInfo.Add(VertexInfo[j]);
                rolledSegmentInfo.Add(SegmentInfo[j]);
            }

            Polygon = new Polygon2d(rolledVertices);
            VertexInfo = rolledVertexInfo;
            SegmentInfo = rolledSegmentInfo;
        }

        public PointData GetPoint(int i, bool reverse)
        {
            if (reverse)
            {
                var segReversed = (TSegmentInfo)SegmentInfo[i].Clone();
                segReversed?.Reverse();
                return new PointData()
                {
                    Vertex = Polygon[i],
                    VertexInfo = VertexInfo[i],
                    SegmentInfo = segReversed,
                };
            }
            else
            {
                return new PointData()
                {
                    Vertex = Polygon[i],
                    VertexInfo = VertexInfo[i],
                    SegmentInfo = SegmentInfo[(i + Polygon.VertexCount - 1) % Polygon.VertexCount]
                };
            }
        }

        public TVertexInfo GetVertexData(int vertexIndex)
        {
            return VertexInfo[vertexIndex];
        }

        public TSegmentInfo GetSegmentDataAfterVertex(int vertexIndex)
        {
            return SegmentInfo[vertexIndex];
        }

        public TSegmentInfo GetSegmentDataBeforeVertex(int vertexIndex)
        {
            if (vertexIndex == 0)
                return SegmentInfo[VertexCount - 1];
            else
                return SegmentInfo[vertexIndex - 1];
        }

        public Segment2d GetSegment2dAfterVertex(int vertexIndex)
        {
            return Polygon.Segment(vertexIndex);
        }

        public Segment2d GetSegment2dBeforeVertex(int vertexIndex)
        {
            if (vertexIndex == 0)
                return Polygon.Segment(VertexCount - 1);
            else
                return Polygon.Segment(vertexIndex - 1);
        }

        public void SetSegmentInfoAfterVertex(int vertexIndex, TSegmentInfo segInfo)
        {
            SegmentInfo[vertexIndex] = segInfo;
        }

        public void SetSegmentInfoBeforeVertex(int vertexIndex, TSegmentInfo segInfo)
        {
            if (vertexIndex == 0)
                SegmentInfo[VertexCount - 1] = segInfo;
            else
                SegmentInfo[vertexIndex - 1] = segInfo;
        }

        public Segment2d SegmentBeforeVertex(int vertexIndex)
        {
            if (vertexIndex == 0)
                return Polygon.Segment(Polygon.VertexCount - 1);
            else
                return Polygon.Segment(vertexIndex - 1);
        }

        public Segment2d SegmentAfterIndex(int i)
        {
            return Polygon.Segment(i);
        }

        public void TrimEnd(double d)
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            Polygon.Reverse();
            VertexInfo.Reverse();
            SegmentInfo.Reverse();
            foreach (var segmentInfo in SegmentInfo)
                segmentInfo?.Reverse();
        }

        public virtual void ConvertToCurve(FillCurveBase<TVertexInfo, TSegmentInfo> curve)
        {
            curve.BeginCurve(Polygon[0], GetVertexData(0));
            for (int i = 1; i < VertexCount; ++i)
            {
                curve.AddToCurve(Polygon[i % VertexCount], GetVertexData(i), GetSegmentDataBeforeVertex(i));
            }
            curve.AddToCurve(Polygon[0], GetVertexData(0), GetSegmentDataBeforeVertex(0));
        }

        public void RollMidSegment(int iSegment, double fNearSeg, FillLoopBase<TVertexInfo, TSegmentInfo> rolled, double tolerance = 0.001)
        {
            double splitParam = fNearSeg / Polygon.Segment(iSegment).Extent / 2d + 0.5d;

            if (Math.Abs(fNearSeg) < GetSegment2dAfterVertex(iSegment).Extent - tolerance)
            {
                int iNextVertex = (iSegment + 1) % VertexCount;
                var interpolatedVertex = InterpolateVertex(Polygon[iSegment], Polygon[iNextVertex], splitParam);
                var interpolatedVertexData = InterpolateVertexInfo(GetVertexData(iSegment), GetVertexData(iNextVertex), splitParam);
                var splitSegmentData = SplitSegmentInfo(GetSegmentDataAfterVertex(iSegment), splitParam);

                rolled.BeginLoop(interpolatedVertex, interpolatedVertexData);
                rolled.AddToLoop(Polygon[iNextVertex], GetVertexData(iSegment), splitSegmentData.Item2);

                for (int i = iSegment + 2; i < VertexCount; ++i)
                    rolled.AddToLoop(Polygon[i], VertexInfo[i], GetSegmentDataBeforeVertex(i));

                for (int i = 0; i <= iSegment; ++i)
                    rolled.AddToLoop(Polygon[i], VertexInfo[i], GetSegmentDataBeforeVertex(i));

                rolled.CloseLoop(splitSegmentData.Item1);
            }
            else
            {
                if (fNearSeg > 0)
                    ++iSegment;
                if (iSegment >= VertexCount)
                    iSegment = 0;

                rolled.BeginLoop(Polygon[iSegment], GetVertexData(iSegment));
                for (int i = iSegment + 1; i < VertexCount; ++i)
                    rolled.AddToLoop(Polygon[i], GetVertexData(i), GetSegmentDataBeforeVertex(i));
                for (int i = 0; i < iSegment; ++i)
                    rolled.AddToLoop(Polygon[i], GetVertexData(i), GetSegmentDataBeforeVertex(i));
                rolled.CloseLoop(GetSegmentDataBeforeVertex(iSegment));
            }
        }

        public void SplitAtDistances<TFillCurve>(
                IEnumerable<double> splits,
                IList<TFillCurve> splitFillCurves,
                Func<TFillCurve> createFillCurveF,
                bool connectEnds = false)
            where TFillCurve : FillCurveBase<TVertexInfo, TSegmentInfo>
        {
            // TODO: Decide what happens when split distance greater than perimeter.
            // TODO: Check for split distances monotonically increasing and > 0.
            // TODO: Check for split distance count more than 0.
            var curve = createFillCurveF();
            ConvertToCurve(curve);
            curve.SplitAtDistances(splits, splitFillCurves, createFillCurveF);

            if (connectEnds)
            {
                var lastCurve = splitFillCurves[splitFillCurves.Count - 1];
                var firstCurve = splitFillCurves[0];
                splitFillCurves.RemoveAt(0);
                lastCurve.Extend(firstCurve);
            }
        }

        public abstract IFillCurve ConvertToCurve();
    }
}