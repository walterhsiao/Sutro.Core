using g3;
using gs.FillTypes;

namespace gs
{
    public abstract class FillElementBase<TSegmentInfo>
        where TSegmentInfo : BasicSegmentInfo, new()
    {
        public IFillType FillType { get; set; } = new DefaultFillType();
        public double FillThickness { get; set; }
        public bool IsHoleShell { get; set; } = false;
        public int PerimOrder { get; set; } = -1;

        public virtual void CopyProperties(FillElementBase<TSegmentInfo> other)
        {
            FillType = other.FillType;
            FillThickness = other.FillThickness;
            IsHoleShell = other.IsHoleShell;
            PerimOrder = other.PerimOrder;
        }

    }
}