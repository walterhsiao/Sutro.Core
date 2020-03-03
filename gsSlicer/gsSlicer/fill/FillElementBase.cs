using g3;
using gs.FillTypes;

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

        public IFillType FillType { get; set; } = new DefaultFillType();
    }
}