using g3;
using gs.FillTypes;
using System;
using System.Collections.Generic;

namespace gs
{
    public abstract class FillBase<TSegmentInfo>
        where TSegmentInfo : IFillSegment, new()
    {
        // General Properties
        public IFillType FillType { get; set; } = new DefaultFillType();
        public double FillThickness { get; set; }
        public bool IsHoleShell { get; set; } = false;
        public int PerimOrder { get; set; } = -1;

        protected List<FillElement<TSegmentInfo>> elements = new List<FillElement<TSegmentInfo>>();
        public IReadOnlyList<FillElement<TSegmentInfo>> Elements => elements.AsReadOnly();

        public virtual IEnumerable<Vector2d> Vertices()
        {
            yield return elements[0].NodeStart.xy;
            foreach (var edge in elements)
                yield return edge.NodeEnd.xy;
        }

        public virtual void CopyProperties(FillBase<TSegmentInfo> other)
        {
            FillType = other.FillType;
            FillThickness = other.FillThickness;
            IsHoleShell = other.IsHoleShell;
            PerimOrder = other.PerimOrder;
        }

        public IEnumerable<FillElement<TSegmentInfo>> ElementsReversed()
        {
            for(int i = elements.Count - 1; i >=0; i--)
            {
                var element = elements[i];
                // Review: (TSegmentInfo) cast seems messy
                yield return new FillElement<TSegmentInfo>(element.NodeEnd, element.NodeStart, (TSegmentInfo)element.Edge.Reversed()); 
            }
        }


        public double TotalLength()
        {
            double totalLength;
            throw new NotImplementedException();
        }

        public double FindClosestElementToPoint(Vector2d point, out int elementIndex, out double elementParameterizedDistance)
        {
            throw new NotImplementedException();
        }

    }
}