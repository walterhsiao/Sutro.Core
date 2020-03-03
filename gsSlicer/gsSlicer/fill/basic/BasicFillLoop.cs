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
            foreach (var v in vertices)
                AppendVertex(v);
        }
    }
}