using CommandLine;
using CommandLine.Text;
using g3;
using gs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using gs.interfaces;

namespace gsCore.CLI
{
    class Program
    {
        #pragma warning disable CS0649
        [ImportMany(typeof(IEngine))]
        protected IEnumerable<Lazy<IEngine, IEngineData>> Engines;
        #pragma warning restore CS0649

        private CompositionContainer _container;

        public Program()
        {
            // An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            // Add catalogs in overrideable method
            AddCatalogs(catalog);

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

        protected virtual void AddCatalogs(AggregateCatalog catalog)
        {
            // Add all the "parts" found in the same assembly as the Program class
            catalog.Catalogs.Add(new ApplicationCatalog());

            // Add all the "parts" found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));

            //// Add all the "parts" found in dlls in the given directory.
            //string dir = Path.Combine(
            //    Directory.GetParent(Environment.CurrentDirectory).Parent.FullName,
            //    "Extensions");
            //catalog.Catalogs.Add(new DirectoryCatalog(dir));
        }

        public class Options
        {
            [Value(0, MetaName = "engine", Required = true, HelpText = "Select which toolpathing engine to use.")]
            public string Engine { get; set; }

            [Value(1, MetaName = "gcode", Required = true, HelpText = "Path to output gcode file.")]
            public string GCodeFilePath { get; set; }

            [Value(2, MetaName = "mesh", Required = false, HelpText = "Path to input mesh file.")]
            public string MeshFilePath { get; set; }

            [Option('s', "settings_files", Required=false, HelpText = "Settings file(s).")]
            public IEnumerable<string> SettingsFiles { get; set; }

            [Option('o', "settings_override", Required = false, HelpText = "Override individual settings")]
            public IEnumerable<string> SettingsOverride{ get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {
            // Construct a dictionary of all the engines that were imported via MEF
            var p = new Program();
            var engineDictionary = new Dictionary<string, Lazy<IEngine, IEngineData>>();
            foreach (var e in p.Engines)
            {
                if (!engineDictionary.ContainsKey(e.Metadata.Name))
                    engineDictionary.Add(e.Metadata.Name.ToLower(), e);
            }

            // Parse the input arguments
            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);

            parserResult.WithParsed(o =>
            {
                if (!engineDictionary.TryGetValue(o.Engine, out var engineEntry))
                {
                    Console.WriteLine("Invalid engine specified.");
                    Console.WriteLine("");
                    Console.WriteLine("Available engines:");
                    Console.WriteLine("");
                    foreach (string s in ListEngines(p))
                        Console.WriteLine(s);
                }
                var engine = engineEntry.Value;
    
                if (engine.Generator.AcceptsParts && (o.MeshFilePath is null || !File.Exists(o.MeshFilePath)))
                {
                    Console.WriteLine("Must provide valid mesh file path as second argument.");
                    Console.WriteLine(Path.GetFullPath(o.MeshFilePath));
                    return;
                }

                else if (o.GCodeFilePath is null || !Directory.Exists(Directory.GetParent(o.GCodeFilePath).ToString()))
                {
                    Console.WriteLine("Must provide valid gcode file path as second argument.");
                    return;
                }

                foreach (string s in o.SettingsFiles)
                {
                    if (!File.Exists(s))
                    {
                        Console.WriteLine("Must provide valid settings file path.");
                        return;
                    }
                }

                var settings = engine.SettingsManager.FactorySettings[0];

                // Load settings from files
                foreach (string s in o.SettingsFiles)
                {
                    try
                    {
                        string settingsText = File.ReadAllText(s);
                        engine.SettingsManager.ApplyJSON(settings, settingsText);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error processing settings file: ");
                        Console.WriteLine(Path.GetFullPath(s));
                        Console.WriteLine(e.Message);
                        return;
                    }
                }

                // Override settings from command-line arguments
                foreach (string s in o.SettingsOverride)
                {
                    try
                    {
                        engine.SettingsManager.ApplyKeyValuePair(settings, s);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error processing settings override from command line argument: ");
                        Console.WriteLine(s);
                        return;
                    }
                }

                string fMeshFilePath = Path.GetFullPath(o.MeshFilePath);
                string fGCodeFilePath = Path.GetFullPath(o.GCodeFilePath);

                Console.Write("Loading mesh " + fMeshFilePath + "...");
                DMesh3 mesh = StandardMeshReader.ReadMesh(fMeshFilePath);
                Console.WriteLine(" done.");

                // Center mesh above origin.
                AxisAlignedBox3d bounds = mesh.CachedBounds;
                Vector3d baseCenterPt = bounds.Center - bounds.Extents.z * Vector3d.AxisZ;
                MeshTransforms.Translate(mesh, -baseCenterPt);

                var part = new Tuple<DMesh3, object>(mesh, null);
                var parts = new List<Tuple<DMesh3, object>>() { part };

                Console.Write("Generating gcode...");
                var gcode = engine.Generator.GenerateGCode(parts, settings);
                Console.WriteLine(" done.");

                Console.Write("Writing gcode...");
                engine.Generator.SaveGCode(fGCodeFilePath, gcode);
                Console.WriteLine(" done.");
            });
        
            parserResult.WithNotParsed(errs => 
            {
                Console.WriteLine("ERRORS:");
                foreach (var err in errs)
                    Console.WriteLine(err);
                Console.WriteLine("");

                Console.WriteLine("HELP:");
                var helpText = HelpText.AutoBuild(parserResult, h => { return h; }, e => e);
                Console.WriteLine(helpText.ToString());
                Console.WriteLine("");

                Console.WriteLine("ENGINES:");
                Console.WriteLine("");
                foreach (string s in ListEngines(p))
                    Console.WriteLine(s);
            });

            return;
        }

        private static IEnumerable<string> ListEngines(Program p)
        {
            var result = new List<string>();

            foreach (var engine in p.Engines)
            {
                result.Add(engine.Metadata.Name + " : " + engine.Metadata.Description);
                result.Add("");
            }
            return result;
        }
    }
}
