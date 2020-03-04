using g3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gs
{

    public interface IToolpathVertex
    {
        Vector3d Position { get; set; }
        double FeedRate { get; set; }
        TPVertexData ExtendedData { get; set; }
    }
}