using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    public class BasicFillCurve : FillCurveBase<BasicSegmentInfo>
    {
        public BasicFillCurve()
        { }

        public BasicFillCurve(IEnumerable<Vector2d> vertices)
        {
            var vertexEnumerator = vertices.GetEnumerator();
            vertexEnumerator.MoveNext();
            BeginOrAppendCurve(vertexEnumerator.Current);

            while (vertexEnumerator.MoveNext())
                BeginOrAppendCurve(vertexEnumerator.Current);
        }

        public override FillCurveBase<BasicSegmentInfo> CloneBare()
        {
            return new BasicFillCurve()
            {
                FillThickness = FillThickness,
                FillType = FillType,
                IsHoleShell = IsHoleShell,
                PerimOrder = PerimOrder
            };
        }

        protected override Tuple<BasicSegmentInfo, BasicSegmentInfo> SplitSegmentInfo(BasicSegmentInfo segmentInfo, double param)
        {
            return segmentInfo?.Split(param) ?? new Tuple<BasicSegmentInfo, BasicSegmentInfo>(null, null);
        }

        public double TotalAreaXY(double defaultThickness)
        {
            double width = FillThickness > 0 ? FillThickness : defaultThickness;
            return ArcLength * width;
        }
    }
}