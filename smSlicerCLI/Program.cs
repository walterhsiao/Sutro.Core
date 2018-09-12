using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using g3;
using gs;
using gs.info;

namespace smSlicerCLI
{
    class Program
    {
        [STAThread]
        static void Main()
        {

            Console.WriteLine("Loading mesh...");

            string fInputMeshPath = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), 
                "..", "..", "..", "Sample Input", "bunny.stl"));
            string fOutputGcodePath = Path.GetFullPath(Path.Combine(
                Directory.GetCurrentDirectory(), 
                "..", "..", "..", "Sample Output", "bunny.gcode"));

            Console.WriteLine(fInputMeshPath);

            DMesh3 mesh = StandardMeshReader.ReadMesh(fInputMeshPath);

            //MeshTransforms.ConvertYUpToZUp(mesh);       // g3 meshes are usually Y-up

            // center mesh above origin
            AxisAlignedBox3d bounds = mesh.CachedBounds;
            Vector3d baseCenterPt = bounds.Center - bounds.Extents.z * Vector3d.AxisZ;
            MeshTransforms.Translate(mesh, -baseCenterPt);

            // create print mesh set
            PrintMeshAssembly meshes = new PrintMeshAssembly();
            meshes.AddMesh(mesh, PrintMeshOptions.Default());

            // create settings
            RepRapSettings settings = new RepRapSettings(RepRap.Models.Unknown);
            settings.GenerateSupport = false;

            // do slicing
            MeshPlanarSlicer slicer = new MeshPlanarSlicer()
            {
                LayerHeightMM = settings.LayerHeightMM
            };
            slicer.Add(meshes);
            PlanarSliceStack slices = slicer.Compute();

            // run print generator
            SingleMaterialFFFPrintGenerator printGen =
                new SingleMaterialFFFPrintGenerator(meshes, slices, settings);
            if (printGen.Generate())
            {
                // export gcode
                GCodeFile gcode = printGen.Result;
                using (StreamWriter w = new StreamWriter(fOutputGcodePath))
                {
                    StandardGCodeWriter writer = new StandardGCodeWriter();
                    writer.WriteFile(gcode, w);
                }
            }
        }
    }
}
