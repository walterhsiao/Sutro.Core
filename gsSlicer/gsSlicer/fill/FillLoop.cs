using g3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gs
{
    /// <summary>
    /// Additive polygon fill curve
    /// </summary>
    public class FillLoop<TSegmentInfo> :
        FillBase<TSegmentInfo>
        where TSegmentInfo : IFillSegment, new()
    {
        private FillLoop()
        {
        }

        public FillLoop<TSegmentInfo> CloneBare()
        {
            var loop = new FillLoop<TSegmentInfo>();
            loop.CopyProperties(this);
            return loop;
        }

        public FillCurve<TSegmentInfo> CloneBareAsCurve()
        {
            var curve = new FillCurve<TSegmentInfo>();
            curve.CopyProperties(this);
            return curve;
        }

        public bool IsClockwise { get { throw new NotImplementedException("FIX"); } }
        public double LoopPerimeter { get { throw new NotImplementedException("FIX"); } }

        public Vector2d EntryExitPoint => elements[0].NodeStart.xy;

        public FillLoop(IEnumerable<Vector2d> vertices)
        {
            throw new NotImplementedException();
        }

        public FillLoop(IEnumerable<FillElement<TSegmentInfo>> elements)
        {
            this.elements = elements.ToList();
        }

        public FillLoop<TSegmentInfo> RollToVertex(int startIndex)
        {
            // TODO: Add range checking for startIndex
            var rolledLoop = new FillLoop<TSegmentInfo>();
            rolledLoop.CopyProperties(this);

            for (int i = 0; i < elements.Count; i++)
            {
                rolledLoop.elements.Add(elements[(i + startIndex) % elements.Count]);
            }

            return rolledLoop;
        }

        public FillLoop<TSegmentInfo> RollBetweenVertex(int elementIndex, double elementParameterizedDistance, double tolerance = 0.001)
        {
            throw new NotImplementedException();

            //if (Math.Abs(fNearSeg) < GetSegment2dAfterVertex(iSegment).Extent - tolerance)
            //{
            //    int iNextVertex = (iSegment + 1) % VertexCount;
            //    var interpolatedVertex = InterpolateVertex(Polygon[iSegment], Polygon[iNextVertex], splitParam);

            //    var segData = GetSegmentDataAfterVertex(iSegment);
            //    var splitSegmentData = segData == null ? Tuple.Create((IFillSegment)new TSegmentInfo(), (IFillSegment)new TSegmentInfo()) : segData.Split(splitParam);

            //    rolled.BeginLoop(interpolatedVertex);
            //    rolled.AddToLoop(Polygon[iNextVertex], (TSegmentInfo)splitSegmentData.Item2);

            //    for (int i = iSegment + 2; i < VertexCount; ++i)
            //        rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));

            //    for (int i = 0; i <= iSegment; ++i)
            //        rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));

            //    rolled.CloseLoop((TSegmentInfo)splitSegmentData.Item1);
            //}
            //else
            //{
            //    if (fNearSeg > 0)
            //        ++iSegment;
            //    if (iSegment >= VertexCount)
            //        iSegment = 0;

            //    rolled.BeginLoop(Polygon[iSegment]);
            //    for (int i = iSegment + 1; i < VertexCount; ++i)
            //        rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));
            //    for (int i = 0; i < iSegment; ++i)
            //        rolled.AddToLoop(Polygon[i], GetSegmentDataBeforeVertex(i));
            //    rolled.CloseLoop(GetSegmentDataBeforeVertex(iSegment));
            //}
        }

        public List<FillLoop<TSegmentInfo>> SplitAtDistances(double[] v)
        {
            throw new NotImplementedException();
        }

        public FillCurve<TSegmentInfo> ConvertToCurve()
        {
            throw new NotImplementedException();
        }
    }
}