using g3;

namespace gs
{
    public abstract class FillElementBase<TVertexInfo, TSegmentInfo>
        where TVertexInfo : BasicVertexInfo, new()
        where TSegmentInfo : BasicSegmentInfo, new()
    {
        public class PointData
        {
            public TSegmentInfo SegmentInfo;
            public Vector2d Vertex;
            public TVertexInfo VertexInfo;
        }
    }
}