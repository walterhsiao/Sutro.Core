using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

using g3;
using gs;
using gs.info;

namespace smSlicerCLI
{
    class Program
    {
        public class Options
        {
            [Option('v', "verbose", Required=false, HelpText ="Set output to verbose messages.")]
            public bool Verbose { get; set; }

            [Value(0, MetaName="mesh", HelpText="Path to input mesh file.")]
            public string MeshFilePath { get; set; }

            [Value(1, MetaName = "gcode", HelpText = "Path to output gcode file.")]
            public string GCodeFilePath { get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {

            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            {
                if (o.MeshFilePath is null || !File.Exists(o.MeshFilePath))
                {
                    Console.WriteLine("Must provide valid mesh file path as first argument.");
                    return;
                }
                else if (o.GCodeFilePath is null || !Directory.Exists(Directory.GetParent(o.GCodeFilePath).ToString()))
                {
                    Console.WriteLine("Must provide valid gcode file path as second argument.");
                    return;
                }

                string fMeshFilePath = Path.GetFullPath(o.MeshFilePath);
                string fGCodeFilePath = Path.GetFullPath(o.GCodeFilePath);

                Console.Write("Loading mesh " + fMeshFilePath + "...");
                DMesh3 mesh = StandardMeshReader.ReadMesh(fMeshFilePath);
                Console.WriteLine(" loaded.");


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


                Console.Write("Slicing mesh...");

                // do slicing
                MeshPlanarSlicer slicer = new MeshPlanarSlicer()
                {
                    LayerHeightMM = settings.LayerHeightMM
                };
                slicer.Add(meshes);
                PlanarSliceStack slices = slicer.Compute();
                Console.WriteLine(" sliced.");


                Console.Write("Generating print...");
                // run print generator
                SingleMaterialFFFPrintGenerator printGen =
                    new SingleMaterialFFFPrintGenerator(meshes, slices, settings);
                if (printGen.Generate())
                {
                    Console.WriteLine(" generated.");
                    // export gcode

                    Console.Write("Writing gcode...");

                    GCodeFile gcode = printGen.Result;
                    using (StreamWriter w = new StreamWriter(fGCodeFilePath))
                    {
                        StandardGCodeWriter writer = new StandardGCodeWriter();
                        writer.WriteFile(gcode, w);
                    }
                    Console.WriteLine(" done.");

                }

            });

            return;
        }
    }
}
