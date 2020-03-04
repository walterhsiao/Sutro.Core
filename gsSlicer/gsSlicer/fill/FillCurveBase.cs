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
        protected PolyLine2d Polyline = new PolyLine2d();
        protected List<TSegmentInfo> SegmentInfo = new List<TSegmentInfo>();
        protected List<TVertexInfo> VertexInfo = new List<TVertexInfo>();

        public double CustomThickness { get; set; }

        // Pass through some properties & methods from wrapped Polyline

        public double ArcLength { get => Polyline.ArcLength; }
        public Vector2d End { get => Polyline.Vertices[Polyline.VertexCount - 1]; }
        public Vector2d Start { get => Polyline.Vertices[0]; }
        public int VertexCount { get => Polyline.VertexCount; }
        public int SegmentCount { get => SegmentInfo.Count; }

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

        public TSegmentInfo GetSegmentAfterVertex(int vertexIndex)
        {
            return SegmentInfo[vertexIndex];
        }
        
        public TSegmentInfo GetSegmentBeforeVertex(int vertexIndex)
        {
            return SegmentInfo[vertexIndex - 1];
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

        public void UpdateSegmentData(int vertexIndex, TSegmentInfo segmentInfo)
        {
            SegmentInfo[vertexIndex] = segmentInfo;
        }

        public void UpdateVertexData(int vertexIndex, TVertexInfo vertexInfo)
        {
            VertexInfo[vertexIndex] = vertexInfo;
        }

        public Segment2d Segment(int i)
        {
            return Polyline.Segment(i);
        }
    }
}