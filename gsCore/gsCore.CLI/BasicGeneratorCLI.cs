using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gs;
using g3;
using System.ComponentModel.Composition;

namespace smSlicerCLI
{
    [Export(typeof(IGenerator))]
    [ExportMetadata("Name", "gsCore")]
    [ExportMetadata("Description", "CLI - Provides access to the basic print generator included in gsCore. Can only create gcode for a single mesh with single material.")]
    public class gsCoreGeneratorCLI :
        BasicGenerator<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings>, IGenerator
    {
        public GCodeFile Generate(IEnumerable<Tuple<DMesh3, object>> meshPartPairs)
        {
            throw new NotImplementedException();
        }

        public void Write(string path, GCodeFile file)
        {
            throw new NotImplementedException();
        }
    }

}
