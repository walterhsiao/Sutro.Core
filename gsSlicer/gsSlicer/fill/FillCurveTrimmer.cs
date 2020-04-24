using System;
using System.Collections.Generic;
using System.Text;

namespace gs
{
    public static class FillCurveTrimmer
    {
        public static IEnumerable<FillElement<TSegmentInfo>> TrimFront<TSegmentInfo>(
            IEnumerable<FillElement<TSegmentInfo>> elements, double trimDistance) 
            where TSegmentInfo : IFillSegment
        {
            throw new NotImplementedException();
            //// TODO: Check distance
            //var split = new List<FillCurve<TSegmentInfo>>();
            //SplitAtDistances(new double[] { trimDistance }, split, CloneBare);

            //if (split.Count > 1)
            //{
            //    Polyline = split[1].Polyline;
            //    SegmentInfo = split[1].SegmentInfo;
            //}
        }

        public static IEnumerable<FillElement<TSegmentInfo>> TrimBack<TSegmentInfo>(double trimDistance)
        {
            throw new NotImplementedException();
            //// TODO: Check distance
            //var split = new List<FillCurve<TSegmentInfo>>();
            //SplitAtDistances(new double[] { ArcLength - trimDistance }, split, CloneBare);

            //if (split.Count > 1)
            //{
            //    Polyline = split[0].Polyline;
            //    SegmentInfo = split[0].SegmentInfo;
            //}
        }

        public static FillCurve<TSegmentInfo> TrimFrontAndBack<TSegmentInfo>(FillCurve<TSegmentInfo> curve, double trimDistanceFront, double? trimDistanceBack = null) where TSegmentInfo : IFillSegment, new()
        {
            throw new NotImplementedException();
            //// TODO: Check distance
            //var split = new List<FillCurve<TSegmentInfo>>();
            //var trimDistances = new double[] { trimDistanceFront, ArcLength - trimDistanceBack ?? trimDistanceFront };
            //SplitAtDistances(trimDistances, split, CloneBare);

            //if (split.Count > 1)
            //{
            //    Polyline = split[1].Polyline;
            //    SegmentInfo = split[1].SegmentInfo;
            //}
        }
    }
}
