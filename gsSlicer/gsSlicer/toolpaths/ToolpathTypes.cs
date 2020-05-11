using g3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gs
{
    public enum ToolpathTypes
    {
        Deposition,
        Cut,

        Travel,
        PlaneChange,

        CustomAssemblerCommands,

        Composite,
        Custom
    };
}