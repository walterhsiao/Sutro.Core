using g3;
using gs.FillTypes;

namespace gs
{
    public abstract class FillElementBase<TSegmentInfo>
        where TSegmentInfo : BasicSegmentInfo, new()
    {
        public IFillType FillType { get; set; } = new DefaultFillType();
    }
}