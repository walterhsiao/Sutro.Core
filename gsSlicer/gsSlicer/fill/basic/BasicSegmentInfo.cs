using System;

namespace gs
{
    public class BasicSegmentInfo : ICloneable
    {
        public bool IsConnector;

        public object Clone()
        {
            return (BasicSegmentInfo)this.MemberwiseClone();
        }

        public void Reverse()
        {
        }
    }
}