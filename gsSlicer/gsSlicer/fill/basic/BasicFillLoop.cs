using g3;
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
    }
}