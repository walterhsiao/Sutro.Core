using g3;
using gs.FillTypes;
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

        public Vector2d this[int i] { get => Polygon[i]; }

        private bool loopStarted = false;
        private bool loopFinished = false;

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

        public TVertexInfo GetDataAtVertex(int vertexIndex)
        {
            return VertexInfo[vertexIndex];
        }

        public TSegmentInfo GetSegmentInfoAfterVertex(int vertexIndex)
        {
            return SegmentInfo[vertexIndex];
        }

        public TSegmentInfo GetSegmentInfoBeforeVertex(int vertexIndex)
        {
            if (vertexIndex == 0)
                return SegmentInfo[VertexCount - 1];
            else
                return SegmentInfo[vertexIndex - 1];
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
    }
}