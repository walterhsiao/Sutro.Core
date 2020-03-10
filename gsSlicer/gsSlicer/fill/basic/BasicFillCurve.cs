using g3;
using System.Collections.Generic;

namespace gs
{
    public class BasicFillCurve : FillCurveBase<BasicVertexInfo, BasicSegmentInfo>
    {
        public BasicFillCurve()
        { }

        public BasicFillCurve(IEnumerable<Vector2d> vertices)
        {
            var vertexEnumerator = vertices.GetEnumerator();
            vertexEnumerator.MoveNext();
            BeginOrAppendCurve(vertexEnumerator.Current);

            while (vertexEnumerator.MoveNext())
                BeginOrAppendCurve(vertexEnumerator.Current);
        }

        public void Trim(double v)
        {
            Polyline.Trim(v);

            // Remove any vertex info that was trimmed away.
            VertexInfo.RemoveRange(Polyline.VertexCount, VertexInfo.Count - Polyline.VertexCount);

            // Remove any segment info that was trimmed away.
            VertexInfo.RemoveRange(Polyline.VertexCount - 1, SegmentInfo.Count - 1 - Polyline.VertexCount);
        }
    }
}