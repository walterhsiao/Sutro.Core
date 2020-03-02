using g3;
using System.Collections.Generic;

namespace gs
{
    internal class VertexListSimplification2 : PolySimplification2
    {
        private IEnumerable<Vector2d> Vertices;
        private bool IsLoop;

        public VertexListSimplification2(IEnumerable<Vector2d> vertices, bool isLoop)
            : base(new Polygon2d(vertices))
        {
            this.Vertices = vertices;
            this.IsLoop = isLoop;
        }
    }
}