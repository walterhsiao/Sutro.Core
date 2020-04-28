using System;

namespace gs
{
    public struct FillSegment : IFillSegment
    {
        public bool IsConnector { get; set; }

        public FillSegment Reversed()
        {
            return new FillSegment(this);
        }

        IFillSegment IFillSegment.Reversed()
        {
            return Reversed();
        }

        public Tuple<IFillSegment, IFillSegment> Split(double t)
        {
            return Tuple.Create((IFillSegment)new FillSegment(this), (IFillSegment)new FillSegment(this));
        }

        public FillSegment(FillSegment other)
        {
            IsConnector = other.IsConnector;
        }
    }
}