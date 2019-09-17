using CommandLine;
using g3;
using gs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using client.Lib;

namespace smSlicerCLI
{
    class Program
    {
        #pragma warning disable CS0649
        [ImportMany(typeof(IGenerator))]
        public IEnumerable<Lazy<IGenerator, IGeneratorData>> Generators;
        #pragma warning restore CS0649

        private CompositionContainer _container;

        public Program()
        {
            // An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            // Add all the "parts" found in the same assembly as the Program class
            catalog.Catalogs.Add(new ApplicationCatalog());

            // Add all the "parts" found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(client.Lib.clientGeneratorA).Assembly));

            //// Add all the "parts" found in dlls in the given directory.
            //string dir = Path.Combine(
            //    Directory.GetParent(Environment.CurrentDirectory).Parent.FullName,
            //    "Extensions");
            //catalog.Catalogs.Add(new DirectoryCatalog(dir));

            // Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            // Fill the imports of this object
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public class Options
        {
            [Value(0, MetaName="mesh", HelpText="Path to input mesh file.")]
            public string MeshFilePath { get; set; }

            [Value(1, MetaName = "gcode", HelpText = "Path to output gcode file.")]
            public string GCodeFilePath { get; set; }

            [Option('g', "generator", Required = false, HelpText = "Generator to use")]
            public string Generator { get; set; }

            [Option('s', "settings_files", Required=false, HelpText = "Settings file(s).")]
            public IEnumerable<string> SettingsFiles { get; set; }

            [Option('o', "settings_override", Required = false, HelpText = "Override individual settings")]
            public IEnumerable<string> SettingsOverride{ get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {
            var p = new Program();
            var generatorDictionary = new Dictionary<string, Lazy<IGenerator, IGeneratorData>>();
            foreach (var g in p.Generators)
            {
                Console.WriteLine(g.Metadata.Name);
                Console.WriteLine(g.Metadata.Description);
                Console.WriteLine("-----------------------");
                if (!generatorDictionary.ContainsKey(g.Metadata.Name))
                    generatorDictionary.Add(g.Metadata.Name.ToLower(), g);
            }

            var generatorName = args[0].ToLower();
            IGenerator generator = generatorDictionary[generatorName].Value;
            generator.Write("", new GCodeFile());

            //Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o =>
            //{
            //    if (o.MeshFilePath is null || !File.Exists(o.MeshFilePath))
            //    {
            //        Console.WriteLine("Must provide valid mesh file path as first argument.");
            //        Console.WriteLine(Path.GetFullPath(o.MeshFilePath));
            //        return;
            //    }
            //    else if (o.GCodeFilePath is null || !Directory.Exists(Directory.GetParent(o.GCodeFilePath).ToString()))
            //    {
            //        Console.WriteLine("Must provide valid gcode file path as second argument.");
            //        return;
            //    }
            //    foreach (string s in o.SettingsFiles)
            //    {
            //        if (!File.Exists(s)){
            //            Console.WriteLine("Must provide valid settings file path.");
            //            return;
            //        }
            //    }

            //    // create settings
            //    RepRapSettings settings = new RepRapSettings(RepRap.Models.Unknown);

            //    JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
            //    {
            //        MissingMemberHandling = MissingMemberHandling.Error,

            //    };

            //    // load settings from files
            //    foreach (string s in o.SettingsFiles)
            //    {
            //        try
            //        {
            //            string settingsText = File.ReadAllText(s);
            //            // TODO: Make this more strict to avoid converting values unintentionally
            //            JsonConvert.PopulateObject(settingsText, settings, jsonSerializeSettings);
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("Error processing settings file: ");
            //            Console.WriteLine(Path.GetFullPath(s));
            //            Console.WriteLine(e.Message);
            //            return;
            //        }
            //    }

            //    // override settings from command-line arguments
            //    foreach (string s in o.SettingsOverride)
            //    {
            //        try
            //        {
            //            // TODO: Make this more strict to avoid converting values unintentionally

            //            string[] keyValue = s.Split(':');
            //            if (keyValue.Length != 2)
            //                throw new Exception("Need setting in \"KeyName:Value\" format; got " + s);
            //            string sFormatted = "{\"" + keyValue[0] + "\":" + keyValue[1] + "}";

            //            JsonConvert.PopulateObject(sFormatted, settings, jsonSerializeSettings);
            //        }
            //        catch (Exception)
            //        {
            //            // TODO: Make error message more useful regarding format
            //            Console.WriteLine("Error processing settings override from command line argument: ");
            //            Console.WriteLine(s);
            //            return;
            //        }
            //    }

            //    string fMeshFilePath = Path.GetFullPath(o.MeshFilePath);
            //    string fGCodeFilePath = Path.GetFullPath(o.GCodeFilePath);

            //    Console.Write("Loading mesh " + fMeshFilePath + "...");
            //    DMesh3 mesh = StandardMeshReader.ReadMesh(fMeshFilePath);
            //    Console.WriteLine(" loaded.");


            //    // center mesh above origin
            //    AxisAlignedBox3d bounds = mesh.CachedBounds;
            //    Vector3d baseCenterPt = bounds.Center - bounds.Extents.z * Vector3d.AxisZ;
            //    MeshTransforms.Translate(mesh, -baseCenterPt);

            //    // create print mesh set
            //    PrintMeshAssembly meshes = new PrintMeshAssembly();
            //    meshes.AddMesh(mesh, PrintMeshOptions.Default());

            //    Console.Write("Slicing mesh...");

            //    // do slicing
            //    MeshPlanarSlicer slicer = new MeshPlanarSlicer()
            //    {
            //        LayerHeightMM = settings.LayerHeightMM
            //    };
            //    slicer.Add(meshes);
            //    PlanarSliceStack slices = slicer.Compute();
            //    Console.WriteLine(" sliced.");


            //    Console.Write("Generating print...");
            //    // run print generator
            //    SingleMaterialFFFPrintGenerator printGen =
            //        new SingleMaterialFFFPrintGenerator(meshes, slices, settings);
            //    if (printGen.Generate())
            //    {
            //        Console.WriteLine(" generated.");
            //        // export gcode

            //        Console.Write("Writing gcode...");

            //        GCodeFile gcode = printGen.Result;
            //        using (StreamWriter w = new StreamWriter(fGCodeFilePath))
            //        {
            //            StandardGCodeWriter writer = new StandardGCodeWriter();
            //            writer.WriteFile(gcode, w);
            //        }
            //        Console.WriteLine(" done.");

            //        Console.WriteLine("".PadRight(79, '-'));
            //        foreach (string line in printGen.TotalPrintTimeStatistics.ToStringList())
            //        {
            //            Console.WriteLine(line);
            //        }
            //        Console.WriteLine("".PadRight(79, '-'));


            //    }

            //});



            return;
        }
    }
}
