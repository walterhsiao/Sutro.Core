using g3;
using gs.FillTypes;
using System;
using System.Collections.Generic;

namespace gs
{
    /// <summary>
    /// Additive polyline fill curve
    /// </summary>
    public abstract class FillCurveBase<TVertexInfo, TSegmentInfo> :
        FillElementBase<TVertexInfo, TSegmentInfo>, IFillCurve
        where TVertexInfo : BasicVertexInfo, new()
        where TSegmentInfo : BasicSegmentInfo, new()
    {
        private PolyLine2d Polyline = new PolyLine2d();
        private List<TSegmentInfo> SegmentInfo = new List<TSegmentInfo>();
        private List<TVertexInfo> VertexInfo = new List<TVertexInfo>();
        public double ArcLength { get => Polyline.ArcLength; }
        public double CustomThickness { get; set; }
        public Vector2d End { get => Polyline.Vertices[Polyline.VertexCount - 1]; }
        public IFillType FillType { get; set; } = new DefaultFillType();
        public Vector2d Start { get => Polyline.Vertices[0]; }

        // Expose some properties & methods from underlying Polyline
        public int VertexCount { get => Polyline.VertexCount; }

        public IEnumerable<Vector2d> Vertices { get => Polyline.Vertices; }

        public Vector2d this[int i] { get => Polyline[i]; }

        public void AppendVertex(Vector2d pt, TVertexInfo vInfo = null, TSegmentInfo sInfo = null)
        {
            Polyline.AppendVertex(pt);
            VertexInfo.Add(vInfo);

            if (Polyline.VertexCount > 0)
                SegmentInfo.Add(sInfo);
            else if (sInfo != null)
                throw new Exception("Cannot add SegmentInfo to the first vertex.");
        }

        public void AppendVertex(Vector2d pt, TSegmentInfo sInfo)
        {
            AppendVertex(pt, null, sInfo);
        }

        public PointData GetPoint(int i, bool reverse)
        {
            if (reverse)
            {
                var segReversed = i >= SegmentInfo.Count - 1 ? null : (TSegmentInfo)SegmentInfo[i]?.Clone();
                segReversed?.Reverse();
                return new PointData()
                {
                    Vertex = Polyline[i],
                    VertexInfo = VertexInfo[i],
                    SegmentInfo = segReversed,
                };
            }
            else
            {
                return new PointData()
                {
                    Vertex = Polyline[i],
                    VertexInfo = VertexInfo[i],
                    SegmentInfo = i == 0 ? null : SegmentInfo[i - 1]
                };
            }
        }

        public void Reverse()
        {
            // Reverse Lists
            Polyline.Reverse();
            VertexInfo.Reverse();
            SegmentInfo.Reverse();

            // Reverse each segment in case segment data is directional
            foreach (var seg in SegmentInfo)
                seg?.Reverse();
        }

        public Segment2d Segment(int i)
        {
            return Polyline.Segment(i);
        }
    }
}