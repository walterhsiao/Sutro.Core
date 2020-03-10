using System;

namespace gs
{
    public class BasicSegmentInfo
    {
        public bool IsConnector;

        public object Clone()
        {
            return (BasicSegmentInfo)this.MemberwiseClone();
        }

        public virtual void Reverse()
        {
        }
    }
}