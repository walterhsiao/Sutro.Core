using System;

namespace gs
{
    public interface IFillSegment
    {
        bool IsConnector { get; }
        void Reverse();
        IFillSegment Reversed();
        Tuple<IFillSegment, IFillSegment> Split(double t);
    }
}