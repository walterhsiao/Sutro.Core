using g3;
using System;
using System.Collections.Generic;

namespace gs
{
    public interface IFillCurve : IFill
    {
        Vector2d Entry { get; }
        Vector2d Exit { get; }
    }

    /// <summary>
    /// Additive polyline fill curve
    /// </summary>
    public class FillCurve<TSegmentInfo> :
        FillBase<TSegmentInfo>, IFillCurve
        where TSegmentInfo : IFillSegment, new()
    {
        public Vector2d Entry => elements[0].NodeStart.xy;
        public Vector2d Exit => elements[^1].NodeEnd.xy;

        public FillCurve<TSegmentInfo> CloneBare()
        {
            var curve = new FillCurve<TSegmentInfo>();
            curve.CopyProperties(this);
            return curve;
        }

        public FillCurve()
        {
        }

        public FillCurve(PolyLine2d polyline) : this(polyline.Vertices)
        {
        }

        public FillCurve(IList<Vector2d> vertices)
        {
            elements.Capacity = vertices.Count - 1;
            for (int i = 1; i < vertices.Count; i++)
            {
                elements.Add(new FillElement<TSegmentInfo>(vertices[i - 1], vertices[i], new TSegmentInfo()));
            }
        }

        public FillCurve(IEnumerable<FillElement<TSegmentInfo>> elements)
        {
            // Note: may want to add continuity checking here for start/end vertices
            base.elements.AddRange(elements);
        }

        private Vector3d? firstPoint = null;

        public void BeginCurve(Vector3d pt)
        {
            if (elements.Count > 0 || firstPoint != null)
                throw new MethodAccessException("BeginCurve called more than once.");
            firstPoint = pt;
        }

        public void BeginCurve(Vector2d pt)
        {
            BeginCurve(new Vector3d(pt.x, pt.y, 0));
        }

        public void AddToCurve(Vector3d pt, TSegmentInfo segmentInfo)
        {
            if (elements.Count > 0)
            {
                elements.Add(new FillElement<TSegmentInfo>(elements[^1].NodeEnd, pt, segmentInfo));
            }
            else if (firstPoint.HasValue)
            {
                elements.Add(new FillElement<TSegmentInfo>(firstPoint.Value, pt, segmentInfo));
            }
            else
            {
                throw new MethodAccessException("AddToCurve called before BeginCurve.");
            }
        }

        public void AddToCurve(Vector2d pt, TSegmentInfo segmentInfo)
        {
            AddToCurve(new Vector3d(pt.x, pt.y, 0), segmentInfo);
        }

        public void AddToCurve(Vector3d pt)
        {
            AddToCurve(pt, new TSegmentInfo());
        }

        public void AddToCurve(Vector2d pt)
        {
            AddToCurve(new Vector3d(pt.x, pt.y, 0), new TSegmentInfo());
        }

        public FillLoop<TSegmentInfo> CloseCurve(TSegmentInfo segmentInfo)
        {
            var loopElements = new List<FillElement<TSegmentInfo>>(elements.Count + 1);
            loopElements.AddRange(elements);
            loopElements.Add(new FillElement<TSegmentInfo>(loopElements[^1].NodeEnd, loopElements[0].NodeStart, segmentInfo));
            var loop = new FillLoop<TSegmentInfo>(loopElements);
            loop.CopyProperties(this);
            return loop;
        }

        public FillLoop<TSegmentInfo> CloseCurve()
        {
            return CloseCurve(new TSegmentInfo());
        }

        public virtual IEnumerable<Vector2d> Vertices()
        {
            yield return elements[0].NodeStart.xy;
            foreach (var edge in elements)
                yield return edge.NodeEnd.xy;
        }

        public FillCurve<TSegmentInfo> Reversed()
        {
            var curve = new FillCurve<TSegmentInfo>(ElementsReversed());
            curve.CopyProperties(this);
            return curve;
        }

        public void Extend(IEnumerable<FillElement<TSegmentInfo>> elements, double stitchTolerance = 1e-6)
        {
            var enumerator = elements.GetEnumerator();
            enumerator.MoveNext();

            if (!enumerator.Current.NodeStart.EpsilonEqual(base.elements[^1].NodeEnd, stitchTolerance))
            {
                throw new ArgumentException("Can only extend with a FillCurve that starts where this FillCurve ends.");
            }
            base.elements.Add(enumerator.Current);
            while (enumerator.MoveNext())
            {
                base.elements.Add(enumerator.Current);
            }
        }

        public List<FillCurve<TSegmentInfo>> SplitAtDistances(IEnumerable<double> splitDistances)
        {
            var elementGroups = FillSplitter<TSegmentInfo>.SplitAtDistances(splitDistances, elements);

            var curves = new List<FillCurve<TSegmentInfo>>();
            foreach (var elementGroup in elementGroups)
            {
                var curve = new FillCurve<TSegmentInfo>(elementGroup);
                curve.CopyProperties(this);
                curves.Add(curve);
            }
            return curves;
        }

        public FillCurve<TSegmentInfo> TrimFront(double trimDistance)
        {
            ValidateTrimDistance(trimDistance, TotalLength());
            return SplitAtDistances(new double[] {trimDistance})[1];
        }

        public FillCurve<TSegmentInfo> TrimBack(double trimDistance)
        {
            double totalLength = TotalLength();
            double trimLocation = totalLength - trimDistance;
            ValidateTrimDistance(trimLocation, totalLength);
            return SplitAtDistances(new double[] { trimLocation })[0];
        }

        public FillCurve<TSegmentInfo> TrimFrontAndBack(double trimDistanceFront, double? trimDistanceBack = null)
        {
            double totalLength = TotalLength();
            double trimLocationBack = totalLength - (trimDistanceBack ?? trimDistanceFront);

            ValidateTrimDistance(trimDistanceFront, totalLength);
            ValidateTrimDistance(trimLocationBack, totalLength);

            if (trimLocationBack < trimDistanceFront)
                throw new ArgumentException("Combined trim amounts are greater than curve length.");

            return SplitAtDistances(new double[] { trimDistanceFront, trimLocationBack })[1];
        }

        private void ValidateTrimDistance(double trimLocation, double totalLength)
        {
            if (trimLocation > totalLength)
                throw new ArgumentException("Trim location must be less than total length.");

            if (trimLocation <= 0)
                throw new ArgumentException("Trim location must be greater than 0.");
        }
    }
}