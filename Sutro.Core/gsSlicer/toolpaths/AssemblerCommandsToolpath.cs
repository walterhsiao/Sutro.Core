using g3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gs
{

    public class AssemblerCommandsToolpath : SentinelToolpath
    {
        public override ToolpathTypes Type
        {
            get { return ToolpathTypes.CustomAssemblerCommands; }
        }

        public Action<IGCodeAssembler, ICNCCompiler> AssemblerF;
    }
}