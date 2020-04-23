using g3;
using gs.FillTypes;

namespace gs
{
    public abstract class FillElementBase<TVertexInfo, TSegmentInfo>
        where TVertexInfo : BasicVertexInfo, new()
        where TSegmentInfo : BasicSegmentInfo, new()
    {
        public IFillType FillType { get; set; } = new DefaultFillType();
    }
}