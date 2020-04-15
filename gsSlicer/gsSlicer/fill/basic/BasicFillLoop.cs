using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    public class BasicFillLoop : FillLoopBase<BasicVertexInfo, BasicSegmentInfo>
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

        public override FillLoopBase<BasicVertexInfo, BasicSegmentInfo> CloneBare()
        {
            return new BasicFillLoop()
            {
                CustomThickness = CustomThickness,
                FillType = FillType,
                IsHoleShell = IsHoleShell,
                PerimOrder = PerimOrder
            };
        }

        public override FillCurveBase<BasicVertexInfo, BasicSegmentInfo> CloneBareAsCurve()
        {
            return new BasicFillCurve()
            {
                CustomThickness = CustomThickness,
                FillType = FillType,
                IsHoleShell = IsHoleShell,
                PerimOrder = PerimOrder
            };
        }

        public override IFillCurve ConvertToCurve()
        {
            var curve = CloneBareAsCurve();
            curve.PopulateFromLoop(this);
            return curve;
        }

        protected override BasicVertexInfo InterpolateVertexInfo(BasicVertexInfo vertexInfoA, BasicVertexInfo vertexInfoB, double param)
        {
            if (vertexInfoA != null && vertexInfoB != null)
                return vertexInfoA.Interpolate(vertexInfoB, param);
            else
                return null;
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