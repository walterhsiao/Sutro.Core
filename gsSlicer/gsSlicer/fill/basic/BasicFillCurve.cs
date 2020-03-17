using g3;
using System;
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

        [Obsolete("This method currently is jury-rigged; logic needs to be improved for non-trivial cases")]
        public void TrimBothEnds(double v)
        {
            // TODO: This method needs big improvements. The segment and vertex information needs
            // to be appropriately interpolated when removing points.

            Polyline.Trim(v);

            // Exit early if no points were actually removed
            if (Polyline.VertexCount == VertexInfo.Count)
                return;

            // Remove any vertex info that was trimmed away.
            VertexInfo.RemoveRange(Polyline.VertexCount, VertexInfo.Count - Polyline.VertexCount);

            // Remove any segment info that was trimmed away.
            VertexInfo.RemoveRange(Polyline.VertexCount - 1, SegmentInfo.Count - 1 - Polyline.VertexCount);
        }
    }
}