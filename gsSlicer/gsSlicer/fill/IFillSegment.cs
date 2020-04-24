using System;

namespace gs
{
    public interface IFillSegment : ICloneable
    {
        bool IsConnector { get; }
        IFillSegment Reversed();
        Tuple<IFillSegment, IFillSegment> Split(double t);
    }
}