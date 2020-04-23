using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    /// <summary>
    /// Additive polyline fill curve
    /// </summary>
    public abstract class FillCurveBase<TSegmentInfo> :
        FillElementBase<TSegmentInfo>
        where TSegmentInfo : BasicSegmentInfo, ICloneable, new()
    {
        public abstract FillCurveBase<TSegmentInfo> CloneBare();

        protected PolyLine2d Polyline = new PolyLine2d();
        protected List<TSegmentInfo> SegmentInfo = new List<TSegmentInfo>();

        // Pass through some properties & methods from wrapped Polyline

        public double ArcLength { get => Polyline.ArcLength; }
        public Vector2d End { get => Polyline.Vertices[Polyline.VertexCount - 1]; }
        public Vector2d Start { get => Polyline.Vertices[0]; }
        public int VertexCount { get => Polyline.VertexCount; }
        public int SegmentCount { get => SegmentInfo.Count; }

        public IEnumerable<Vector2d> Vertices { get => Polyline.Vertices; }


        public Vector2d this[int i] { get => Polyline[i]; }

        private bool curveStarted = false;

        protected FillCurveBase()
        {
        }

        protected FillCurveBase(FillCurveBase<TSegmentInfo> other)
        {
            CopyProperties(other);
        }

        public void BeginOrAppendCurve(Vector2d pt)
        {
            Polyline.AppendVertex(pt);
            if (curveStarted)
            {
                SegmentInfo.Add(null);
            }
            else
            {
                curveStarted = true;
            }
        }

        public void BeginCurve(Vector2d pt)
        {
            if (curveStarted)
                throw new MethodAccessException("BeginCurve called more than once.");

            curveStarted = true;

            Polyline.AppendVertex(pt);
        }

        public void AddToCurve(Vector2d pt, TSegmentInfo sInfo = null)
        {
            if (!curveStarted)
                throw new MethodAccessException("AddToCurve called before BeginCurve.");

            Polyline.AppendVertex(pt);
            SegmentInfo.Add(sInfo);
        }

        protected Vector2d InterpolateVertex(Vector2d vertexA, Vector2d vertexB, double param)
        {
            return vertexA * (1 - param) + vertexB * param;
        }


        protected abstract Tuple<TSegmentInfo, TSegmentInfo> SplitSegmentInfo(TSegmentInfo segmentInfo, double param);

        public Segment2d GetSegment2dAfterVertex(int vertexIndex)
        {
            return Polyline.Segment(vertexIndex);
        }

        public Segment2d GetSegment2dBeforeVertex(int vertexIndex)
        {
            return Polyline.Segment(vertexIndex - 1);
        }

        public TSegmentInfo GetSegmentDataAfterVertex(int vertexIndex)
        {
            return SegmentInfo[vertexIndex];
        }

        public TSegmentInfo GetSegmentDataBeforeVertex(int vertexIndex)
        {
            return SegmentInfo[vertexIndex - 1];
        }

        public void Reverse()
        {
            // Reverse Lists
            Polyline.Reverse();
            SegmentInfo.Reverse();

            // Reverse each segment in case segment data is directional
            foreach (var seg in SegmentInfo)
                seg?.Reverse();
        }

        public void UpdateSegmentData(int vertexIndex, TSegmentInfo segmentInfo)
        {
            SegmentInfo[vertexIndex] = segmentInfo;
        }

        public void TrimFront(double trimDistance)
        {
            // TODO: Check distance
            var split = new List<FillCurveBase<TSegmentInfo>>();
            SplitAtDistances(new double[] { trimDistance }, split, CloneBare);

            if (split.Count > 1)
            {
                Polyline = split[1].Polyline;
                SegmentInfo = split[1].SegmentInfo;
            }
        }

        public void TrimBack(double trimDistance)
        {
            // TODO: Check distance
            var split = new List<FillCurveBase<TSegmentInfo>>();
            SplitAtDistances(new double[] { ArcLength - trimDistance }, split, CloneBare);

            if (split.Count > 1)
            {
                Polyline = split[0].Polyline;
                SegmentInfo = split[0].SegmentInfo;
            }
        }

        public void TrimFrontAndBack(double trimDistanceFront, double? trimDistanceBack = null)
        {
            // TODO: Check distance
            var split = new List<FillCurveBase<TSegmentInfo>>();
            var trimDistances = new double[] { trimDistanceFront, ArcLength - trimDistanceBack ?? trimDistanceFront };
            SplitAtDistances(trimDistances, split, CloneBare);

            if (split.Count > 1)
            {
                Polyline = split[1].Polyline;
                SegmentInfo = split[1].SegmentInfo;
            }
        }

        public void SplitAtDistances(IEnumerable<double> splitDistances, IList<FillCurveBase<TSegmentInfo>> splitFillCurves, Func<FillCurveBase<TSegmentInfo>> createFillCurveF)
        {
            // TODO: Decide what happens when split distance greater than length.
            // TODO: Check for split distances monotonically increasing and > 0.

            double cumulativeDistance = 0;
            var splitsQueue = new Queue<double>(splitDistances);

            // Initialize the first curve
            var curve = createFillCurveF();
            curve.BeginCurve(Polyline[0]);

            // If splits are empty, just return the full copy of this curve
            if (splitsQueue.Count == 0)
            {
                curve.Extend(this);
                splitFillCurves.Add(curve);
            }

            // If there is a split location on the first vertex, remove first split
            if (splitsQueue.Peek() == 0)
                splitsQueue.Dequeue();

            // Iterate through the fill elements in the polygon.
            for (int i = 1; i < VertexCount; i++)
            {
                // If no splits are left, just add the current point
                if (splitsQueue.Count == 0)
                {
                    curve.AddToCurve(Polyline[i], GetSegmentDataBeforeVertex(i));
                    continue;
                }

                // Calculate how much distance the current segment adds
                double nextDistance = GetSegment2dBeforeVertex(i).Length;

                var segmentInfo = GetSegmentDataBeforeVertex(i);

                // For each split distance within the current segment
                while (splitsQueue.Count > 0 && splitsQueue.Peek() < cumulativeDistance + nextDistance)
                {
                    // Create normalized split distance (0,1)
                    double splitDistance = splitsQueue.Dequeue() - cumulativeDistance;
                    double t = splitDistance / nextDistance;

                    var splitVertex = InterpolateVertex(curve.Polyline[curve.VertexCount - 1], Polyline[i], t);
                    var splitSegmentData = SplitSegmentInfo(segmentInfo, t);

                    curve.AddToCurve(splitVertex, splitSegmentData.Item1);
                    splitFillCurves.Add(curve);
                    curve = createFillCurveF();
                    curve.BeginCurve(splitVertex);

                    segmentInfo = splitSegmentData.Item2;
                    cumulativeDistance += splitDistance;
                    nextDistance -= splitDistance;
                }

                curve.AddToCurve(Polyline[i], segmentInfo);

                cumulativeDistance += nextDistance;
            }
            splitFillCurves.Add(curve);
        }

        public void Extend(FillCurveBase<TSegmentInfo> other, double stitchTolerance = 1e-6)
        {
            if (!other.Polyline[0].EpsilonEqual(Polyline[VertexCount - 1], stitchTolerance))
            {
                throw new ArgumentException("Can only extend with a FillCurve that starts where this FillCurve ends.");
            }

            for (int i = 1; i < other.VertexCount; i++)
            {
                AddToCurve(other.Polyline[i], other.GetSegmentDataBeforeVertex(i));
            }
        }

        public void PopulateFromLoop(FillLoopBase<TSegmentInfo> loop)
        {
            CopyProperties(loop);

            BeginCurve(loop[0]);
            for (int i = 1; i < loop.VertexCount; i++)
            {
                AddToCurve(loop[i], loop.GetSegmentDataBeforeVertex(i));
            }
            AddToCurve(loop[0]);
        }
    }
}