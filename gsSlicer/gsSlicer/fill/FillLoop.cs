using g3;
using System;
using System.Collections.Generic;
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
        protected FillLoop()
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

        public bool IsClockwise()
        {
            // Note: Could cache or otherwise optimize this computation
            var poly = new Polygon2d(Vertices(false));
            return poly.IsClockwise;
        }

        public Vector2d EntryExitPoint => elements[0].NodeStart.xy;

        public FillLoop(IList<Vector2d> vertices)
        {
            elements.Capacity = vertices.Count;
            for (int i = 1; i < vertices.Count; i++)
            {
                elements.Add(new FillElement<TSegmentInfo>(vertices[i - 1], vertices[i], new TSegmentInfo()));
            }
            elements.Add(new FillElement<TSegmentInfo>(vertices[^1], vertices[0], new TSegmentInfo()));
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

        public FillLoop<TSegmentInfo> RollBetweenVertices(ElementLocation location, double tolerance = 0.001)
        {
            if (!ElementShouldSplit(location.ParameterizedDistance, tolerance, elements[location.Index].GetSegment2d().Length))
            {
                return RollToVertex(IdentifyClosestVertex(location.Index, location.ParameterizedDistance));
            }

            var elementToSplit = elements[location.Index];

            var interpolatedVertex = Vector3d.Lerp(elementToSplit.NodeStart, elementToSplit.NodeEnd, location.ParameterizedDistance);

            var splitSegmentData = SplitSegment(location.ParameterizedDistance, elementToSplit);

            var rolledElements = CreateRolledElements(location.Index, interpolatedVertex, elementToSplit, splitSegmentData);

            var rolledLoop = new FillLoop<TSegmentInfo>(rolledElements);
            rolledLoop.CopyProperties(this);
            return rolledLoop;
        }

        private List<FillElement<TSegmentInfo>> CreateRolledElements(int elementIndex, Vector3d interpolatedVertex, FillElement<TSegmentInfo> elementToSplit,
            Tuple<IFillSegment, IFillSegment> splitSegmentData)
        {
            var rolledElements = new List<FillElement<TSegmentInfo>>(elements.Count + 1);

            // Add the second half of the split element
            rolledElements.Add(new FillElement<TSegmentInfo>(
                interpolatedVertex,
                elementToSplit.NodeEnd,
                (TSegmentInfo) splitSegmentData.Item2));

            // Add all elements after the split element
            for (int i = elementIndex + 1; i < elements.Count; ++i)
                rolledElements.Add(elements[i]);

            // Add all elements before the split element
            for (int i = 0; i < elementIndex; ++i)
                rolledElements.Add(elements[i]);

            // Add the first half of the split element
            rolledElements.Add(new FillElement<TSegmentInfo>(
                elementToSplit.NodeStart,
                interpolatedVertex,
                (TSegmentInfo) splitSegmentData.Item1));
            return rolledElements;
        }

        private static Tuple<IFillSegment, IFillSegment> SplitSegment(double parameterizedDistance, FillElement<TSegmentInfo> element)
        {
            return element.Edge == null
                ? Tuple.Create((IFillSegment) new TSegmentInfo(), (IFillSegment) new TSegmentInfo())
                : element.Edge.Split(parameterizedDistance);
        }

        private int IdentifyClosestVertex(int elementIndex, double elementParameterizedDistance)
        {
            if (elementParameterizedDistance > 0.5)
            {
                ++elementIndex;
                if (elementIndex >= elements.Count)
                {
                    elementIndex = 0;
                }
            }

            return elementIndex;
        }

        private static bool ElementShouldSplit(double parameterizedSplitDistance, double tolerance, double segmentLength)
        {
            double toleranceParameterized = tolerance / segmentLength;
            return parameterizedSplitDistance > toleranceParameterized && parameterizedSplitDistance < (1 - toleranceParameterized);
        }

        public List<FillCurve<TSegmentInfo>> SplitAtDistances(double[] splitDistances, bool joinFirstAndLast = false)
        {
            var elementGroups = FillSplitter<TSegmentInfo>.SplitAtDistances(splitDistances, elements);

            if (joinFirstAndLast)
            {
                var firstCurve = elementGroups[0];
                elementGroups.RemoveAt(0);
                elementGroups[^1].AddRange(firstCurve);
            }

            var curves = new List<FillCurve<TSegmentInfo>>();
            foreach (var elementGroup in elementGroups)
            {
                var curve = new FillCurve<TSegmentInfo>(elementGroup);
                curve.CopyProperties(this);
                curves.Add(curve);
            }
            return curves;
        }

        public FillCurve<TSegmentInfo> ConvertToCurve()
        {
            var curve = new FillCurve<TSegmentInfo>(elements);
            curve.CopyProperties(this);
            return curve;
        }

        public FillLoop<TSegmentInfo> Reversed()
        {
            var loop = new FillLoop<TSegmentInfo>(ElementsReversed());
            loop.CopyProperties(this);
            return loop;
        }

        public virtual IEnumerable<Vector2d> Vertices(bool repeatFirst = false)
        {
            foreach (var edge in elements)
                yield return edge.NodeStart.xy;

            if (repeatFirst)
                yield return elements[0].NodeStart.xy;
        }
    }
}