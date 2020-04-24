using g3;
using System.Collections.Generic;

namespace gs
{
    public static class FillSplitter<TSegmentInfo> where TSegmentInfo : IFillSegment
    {
        public static List<List<FillElement<TSegmentInfo>>> SplitAtDistances(
            IEnumerable<double> splitDistances,
            IEnumerable<FillElement<TSegmentInfo>> elements)
        {
            // TODO: Decide what happens when split distance greater than length.
            // TODO: Check for split distances monotonically increasing and > 0.

            double cumulativeDistance = 0;
            var splitsQueue = new Queue<double>(splitDistances);

            var result = new List<List<FillElement<TSegmentInfo>>>();

            // Initialize the first list of elements
            var splitElements = new List<FillElement<TSegmentInfo>>();

            // If splits are empty, just return the full copy of this curve
            if (splitsQueue.Count == 0)
            {
                splitElements.AddRange(elements);
                result.Add(splitElements);
                return result;
            }

            // If there is a split location on the first vertex, remove first split
            if (splitsQueue.Peek() == 0)
                splitsQueue.Dequeue();

            // Iterate through the fill elements in the polygon.
            foreach (var element in elements)
            {
                // If no splits are left, just add the current point
                if (splitsQueue.Count == 0)
                {
                    splitElements.Add(element);
                    continue;
                }

                // Calculate how much distance the current segment adds
                double nextDistance = element.GetSegment2d().Length;
                var nodeStart = element.NodeStart;
                var segmentInfo = element.Edge;

                // For each split distance within the current segment
                while (splitsQueue.Count > 0 && splitsQueue.Peek() < cumulativeDistance + nextDistance)
                {
                    // Create normalized split distance (0,1)
                    double splitDistance = splitsQueue.Dequeue() - cumulativeDistance;
                    double t = splitDistance / nextDistance;

                    var splitVertex = Vector3d.Lerp(nodeStart, element.NodeEnd, t);

                    var splitSegmentData = element.Edge.Split(t);

                    splitElements.Add( new FillElement<TSegmentInfo>(nodeStart, splitVertex, (TSegmentInfo)splitSegmentData.Item1));
                    result.Add(splitElements);
                    splitElements = new List<FillElement<TSegmentInfo>>();

                    segmentInfo = (TSegmentInfo)splitSegmentData.Item2;
                    cumulativeDistance += splitDistance;
                    nextDistance -= splitDistance;
                    nodeStart = splitVertex;
                }

                splitElements.Add(new FillElement<TSegmentInfo>(nodeStart, element.NodeEnd, segmentInfo));

                cumulativeDistance += nextDistance;
            }
            result.Add(splitElements);
            return result;
        }
    }
}
