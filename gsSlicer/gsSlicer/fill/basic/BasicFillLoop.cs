using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    public class BasicFillLoop : FillLoopBase<BasicSegmentInfo>
    {
        public BasicFillLoop()
        { }
 
        public BasicFillLoop(IEnumerable<Vector2d> vertices)
        {
            var vertexEnumerator = vertices.GetEnumerator();
            vertexEnumerator.MoveNext();
            BeginLoop(vertexEnumerator.Current);

            while (vertexEnumerator.MoveNext())
                AddToLoop(vertexEnumerator.Current);

            CloseLoop();
        }

        public override FillLoopBase<BasicSegmentInfo> CloneBare()
        {
            var loop = new BasicFillLoop();
            loop.CopyProperties(this);
            return loop;
        }

        public override FillCurveBase<BasicSegmentInfo> CloneBareAsCurve()
        {
            var curve = new BasicFillCurve();
            curve.CopyProperties(this);
            return curve;
        }

        public override FillCurveBase<BasicSegmentInfo> ConvertToCurve()
        {
            var curve = CloneBareAsCurve();
            curve.PopulateFromLoop(this);
            return curve;
        }

        protected override Tuple<BasicSegmentInfo, BasicSegmentInfo> SplitSegmentInfo(BasicSegmentInfo segmentInfo, double param)
        {
            if (segmentInfo != null)
                return segmentInfo.Split(param);
            else
                return new Tuple<BasicSegmentInfo, BasicSegmentInfo>(null, null);
        }
    }
}