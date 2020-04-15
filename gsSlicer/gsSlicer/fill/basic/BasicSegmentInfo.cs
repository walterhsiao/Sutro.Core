using System;

namespace gs
{
    public class BasicSegmentInfo : ICloneable
    {
        public bool IsConnector { get; set; }

        public virtual void Reverse()
        {
        }

        public BasicSegmentInfo()
        {
        }

        public BasicSegmentInfo(BasicSegmentInfo other)
        {
            IsConnector = other.IsConnector;
        }

        public Tuple<BasicSegmentInfo, BasicSegmentInfo> Split(double param)
        {
            return Tuple.Create(new BasicSegmentInfo(this), new BasicSegmentInfo(this));
        }

        object ICloneable.Clone()
        {
            return (BasicSegmentInfo)MemberwiseClone();
        }
    }
}