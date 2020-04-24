using g3;

namespace gs
{
    public struct FillElement<TEdge>
    {
        public Vector3d NodeStart { get; }
        public TEdge Edge { get; }
        public Vector3d NodeEnd { get; }

        public FillElement(Vector3d start, Vector3d end, TEdge edge)
        {
            NodeStart = start;
            NodeEnd = end;
            Edge = edge;
        }

        public FillElement(Vector2d start, Vector2d end, TEdge edge)
            : this(new Vector3d(start.x, start.y, 0), new Vector3d(end.x, end.y, 0), edge)
        {
        }

        public Segment2d GetSegment2d()
        {
            return new Segment2d(NodeStart.xy, NodeEnd.xy);
        }
    }
}