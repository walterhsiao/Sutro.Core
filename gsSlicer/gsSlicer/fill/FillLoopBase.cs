using g3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gs
{
    /// <summary>
    /// Additive polygon fill curve
    /// </summary>
    public class FillLoopBase<TSegmentInfo> :
        FillElementBase<TSegmentInfo>
        where TSegmentInfo : IFillSegment, ICloneable, new()
    {
        public FillLoopBase<TSegmentInfo> CloneBare()
        {
            var loop = new FillLoopBase<TSegmentInfo>();
            loop.CopyProperties(this);
            return loop;
        }

        public FillCurveBase<TSegmentInfo> CloneBareAsCurve()
        {
            var curve = new FillCurveBase<TSegmentInfo>();
            curve.CopyProperties(this);
            return curve;
        }

        public Polygon2d Polygon { get; protected set; } = new Polygon2d();
        protected List<TSegmentInfo> SegmentInfo = new List<TSegmentInfo>();

        // Pass through some properties & methods from wrapped Polygon

        public bool IsClockwise { get => Polygon.IsClockwise; }
        public double Perimeter { get => Polygon.Perimeter; }
        public int VertexCount { get => Polygon.VertexCount; }
        public int SegmentCount { get => SegmentInfo.Count; }
        public IEnumerable<Vector2d> Vertices { get => Polygon.VerticesItr(false); }

        public Vector2d EntryExitPoint => Polygon.Vertices[0];

        public AxisAlignedBox2d Bounds => Polygon.GetBounds();

        public Vector2d this[int i] { get => Polygon[i]; }

        private bool loopStarted = false;
        private bool loopFinished = false;

        public FillLoopBase()
        {
        }

        public FillLoopBase(ReadOnlyCollection<Vector2d> vertices)
        {
            Polygon = new Polygon2d(vertices);
            SegmentInfo = new TSegmentInfo[Polygon.VertexCount].ToList();
        }

        public FillLoopBase(IEnumerable<Vector2d> vertices)
        {
            Polygon = new Polygon2d(vertices);
            SegmentInfo = new TSegmentInfo[Polygon.VertexCount].ToList();
        }

        public void BeginOrAppendCurve(Vector2d pt)
        {
            Polygon.AppendVertex(pt);
            if (loopStarted)
            {
                SegmentInfo.Add(new TSegmentInfo());
            }
            else
            {
                loopStarted = true;
            }
        }

        public void BeginLoop(Vector2d pt)
        {
            if (loopStarted)
                throw new MethodAccessException("BeginLoop called more than once.");

            loopStarted = true;

            Polygon.AppendVertex(pt);
        }

        public void AddToLoop(Vector2d pt, TSegmentInfo sInfo)
        {
            if (!loopStarted)
                throw new MethodAccessException("AddToLoop called before BeginLoop.");
            if (loopFinished)
                throw new MethodAccessException("AddToLoop called after CloseLoop");

            Polygon.AppendVertex(pt);
            SegmentInfo.Add(sInfo);
        }

        public void CloseLoop(TSegmentInfo sInfo)
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

        public void Roll(int k)
        {
            // Make copies of current lists
            var rolledVertices = new List<Vector2d>(VertexCount);
            var rolledSegmentInfo = new List<TSegmentInfo>(VertexCount);

            for (int i = 0; i < VertexCount; i++)
            {
                int j = (i + k) % VertexCount;
                rolledVertices.Add(Polygon.Vertices[j]);
                rolledSegmentInfo.Add(SegmentInfo[j]);
            }

            Polygon = new Polygon2d(rolledVertices);
            SegmentInfo = rolledSegmentInfo;
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
            SegmentInfo.Reverse();
            foreach (var segmentInfo in SegmentInfo)
                segmentInfo?.Reverse();
        }

        public virtual void ConvertToCurve(FillCurveBase<TSegmentInfo> curve)
        {
            curve.BeginCurve(Polygon[0]);
            for (int i = 1; i < VertexCount; ++i)
            {
                curve.AddToCurve(Polygon[i % VertexCount], GetSegmentDataBeforeVertex(i));
            }
            curve.AddToCurve(Polygon[0], GetSegmentDataBeforeVertex(0));
        }

        public void RollMidSegment(int iSegment, double fNearSeg, FillLoopBase<TSegmentInfo> rolled, double tolerance = 0.001)
        {
            double splitParam = fNearSeg / Polygon.Segment(iSegment).Extent / 2d + 0.5d;

            if (Math.Abs(fNearSeg) < GetSegment2dAfterVertex(iSegment).Extent - tolerance)
            {
                int iNextVertex = (iSegment + 1) % VertexCount;
                var interpolatedVertex = InterpolateVertex(Polygon[iSegment], Polygon[iNextVertex], splitParam);

                var segData = GetSegmentDataAfterVertex(iSegment);
                var splitSegmentData = segData == null ? Tuple.Create((IFillSegment)new TSegmentInfo(), (IFillSegment)new TSegmentInfo()) : segData.Split(splitParam);

                rolled.BeginLoop(interpolatedVertex);
                rolled.AddToLoop(Polygon[iNextVertex], (TSegmentInfo)splitSegmentData.Item2);

                for (int i = iSegment + 2; i < VertexCount; ++i)
                    rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));

                for (int i = 0; i <= iSegment; ++i)
                    rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));

                rolled.CloseLoop((TSegmentInfo)splitSegmentData.Item1);
            }
            else
            {
                if (fNearSeg > 0)
                    ++iSegment;
                if (iSegment >= VertexCount)
                    iSegment = 0;

                rolled.BeginLoop(Polygon[iSegment]);
                for (int i = iSegment + 1; i < VertexCount; ++i)
                    rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));
                for (int i = 0; i < iSegment; ++i)
                    rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));
                rolled.CloseLoop(GetSegmentDataBeforeVertex(iSegment));
            }
        }

        public List<FillCurveBase<TSegmentInfo>> SplitAtDistances(
                IEnumerable<double> splits,
                Func<FillCurveBase<TSegmentInfo>> createFillCurveF,
                bool joinFirstAndLast = false)
        {
            // TODO: Decide what happens when split distance greater than perimeter.
            // TODO: Check for split distances monotonically increasing and > 0.
            // TODO: Check for split distance count more than 0.
            var splitFillCurves = new List<FillCurveBase<TSegmentInfo>>();
            var curve = createFillCurveF();
            ConvertToCurve(curve);
            curve.SplitAtDistances(splits, splitFillCurves, createFillCurveF);

            if (joinFirstAndLast)
            {
                var lastCurve = splitFillCurves[splitFillCurves.Count - 1];
                var firstCurve = splitFillCurves[0];
                splitFillCurves.RemoveAt(0);
                lastCurve.Extend(firstCurve);
            }
            return splitFillCurves;
        }

        public FillCurveBase<TSegmentInfo> ConvertToCurve()
        {
            var curve = CloneBareAsCurve();
            curve.PopulateFromLoop(this);
            return curve;
        }
    }
}