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
            return new BasicFillLoop()
            {
                CustomThickness = CustomThickness,
                FillType = FillType,
                IsHoleShell = IsHoleShell,
                PerimOrder = PerimOrder
            };
        }

        public override FillCurveBase<BasicSegmentInfo> CloneBareAsCurve()
        {
            return new BasicFillCurve()
            {
                CustomThickness = CustomThickness,
                FillType = FillType,
                IsHoleShell = IsHoleShell,
                PerimOrder = PerimOrder
            };
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