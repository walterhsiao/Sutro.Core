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
        protected Polygon2d Polygon = new Polygon2d();
        protected List<TSegmentInfo> SegmentInfo = new List<TSegmentInfo>();
        protected List<TVertexInfo> VertexInfo = new List<TVertexInfo>();
        public double CustomThickness { get; set; }
        public IFillType FillType { get; set; } = new DefaultFillType();
        public double Perimeter { get => Polygon.Perimeter; }

        // Expose some properties & methods from underlying Polygon
        public int VertexCount { get => Polygon.VertexCount; }

        public IEnumerable<Vector2d> Vertices { get => Polygon.VerticesItr(false); }
        public Vector2d this[int i] { get => Polygon[i]; }

        public void AppendVertex(Vector2d pt, TVertexInfo vInfo = null, TSegmentInfo sInfo = null)
        {
            Polygon.AppendVertex(pt);
            VertexInfo.Add(vInfo);

            if (Polygon.VertexCount > 0)
                SegmentInfo.Add(sInfo);
            else if (sInfo != null)
                throw new Exception("Cannot add SegmentInfo to the first vertex.");
        }

        public void AppendVertex(Vector2d pt, TSegmentInfo sInfo)
        {
            AppendVertex(pt, null, sInfo);
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

        public Segment2d Segment(int i)
        {
            return Polygon.Segment(i);
        }
    }
}