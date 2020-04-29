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

        public virtual void CopyProperties(FillBase<TSegmentInfo> other)
        {
            FillType = other.FillType;
            FillThickness = other.FillThickness;
            IsHoleShell = other.IsHoleShell;
            PerimOrder = other.PerimOrder;
        }

        public IEnumerable<FillElement<TSegmentInfo>> ElementsReversed()
        {
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                var element = elements[i];
                // Review: (TSegmentInfo) cast seems messy
                yield return new FillElement<TSegmentInfo>(element.NodeEnd, element.NodeStart, (TSegmentInfo)element.Edge.Reversed());
            }
        }

        public double TotalLength()
        {
            double length = 0;

            foreach (var element in elements)
            {
                length += (element.NodeEnd - element.NodeStart).Length;
            }

            return length;
        }
        
        public double FindClosestElementToPoint(Vector2d point, out ElementLocation location)
        {
            location = new ElementLocation(int.MinValue, 0);

            double closestDistanceSquared = double.MaxValue;
            int currentElementIndex = 0;

            foreach (var element in elements)
            {
                // Update results if current element is closer
                Segment2d seg = element.GetSegment2d();
                double currentSegmentClosestDistanceSquared = seg.DistanceSquared(point);

                // Update results if current element is closer
                if (currentSegmentClosestDistanceSquared < closestDistanceSquared)
                {
                    closestDistanceSquared = currentSegmentClosestDistanceSquared;
                    location.Index = currentElementIndex;
                    location.ParameterizedDistance = GetParameterizedDistance(point, seg);
                }

                currentElementIndex++;
            }

            // For consistency, if the closest point is on a vertex,
            // give the index of the element after the vertex
            if (MathUtil.EpsilonEqual(location.ParameterizedDistance, 1, 1e-6))
            {
                location.ParameterizedDistance = 0;
                location.Index = (location.Index + 1) % elements.Count;
            }
            return Math.Sqrt(closestDistanceSquared);
        }

        private static double GetParameterizedDistance(Vector2d point, Segment2d seg)
        {
            return (Math.Clamp((point - seg.Center).Dot(seg.Direction), -seg.Extent, seg.Extent) / seg.Extent + 1) / 2d;
        }
    }
}