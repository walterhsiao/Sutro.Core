using g3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace gs
{
    public class PrintGeneratorManager<TPrintGenerator, TPrintSettings> : IPrintGeneratorManager
            where TPrintGenerator : IPrintGenerator<TPrintSettings>, new()
            where TPrintSettings : SettingsPrototype, IPlanarAdditiveSettings, new()
    {
        private readonly ILogger logger;
        private ISettingsBuilder<TPrintSettings> settingsBuilder;

        public bool AcceptsParts { get; } = true;

        public bool AcceptsPartSettings { get; } = false;

        public Version PrintGeneratorAssemblyVersion
        {
            get
            {
                var assembly = Assembly.GetAssembly(typeof(TPrintGenerator));
                return assembly.GetName().Version;
            }
        }

        public string PrintGeneratorName
        {
            get
            {
                return typeof(TPrintGenerator).Name;
            }
        }

        public ISettingsBuilder SettingsBuilder => settingsBuilder;

        public TPrintSettings Settings => settingsBuilder.Settings;

        public PrintGeneratorManager(TPrintSettings settings, ILogger logger = null, bool acceptsParts = true)
        {
            AcceptsParts = acceptsParts;
            settingsBuilder = new SettingsBuilder<TPrintSettings>(settings, logger);
            this.logger = logger ?? new NullLogger();
        }

        public GCodeFile GCodeFromMesh(DMesh3 mesh, out IEnumerable<string> generationReport)
        {
            if (!AcceptsParts && mesh != null)
                throw new Exception("Must pass null or empty list of parts to generator that does not accept parts.");

            // Create print mesh set
            PrintMeshAssembly meshes = new PrintMeshAssembly();
            meshes.AddMesh(mesh, PrintMeshOptions.Default());

            logger?.WriteLine("Slicing...");

            // Do slicing
            MeshPlanarSlicer slicer = new MeshPlanarSlicer()
            {
                LayerHeightMM = Settings.LayerHeightMM
            };

            slicer.Add(meshes);
            PlanarSliceStack slices = slicer.Compute();

            // Run the print generator
            logger.WriteLine("Running print generator...");
            var printGenerator = new TPrintGenerator();
            AssemblerFactoryF overrideAssemblerF = Settings.AssemblerType();
            printGenerator.Initialize(meshes, slices, Settings, overrideAssemblerF);

            if (printGenerator.Generate())
            {
                generationReport = printGenerator.GenerationReport;
                return printGenerator.Result;
            }
            else
            {
                throw new Exception("PrintGenerator failed to generate gcode!");
            }
        }

        public GCodeFile LoadGCode(TextReader input)
        {
            GenericGCodeParser parser = new GenericGCodeParser();
            return parser.Parse(input);
        }

        public void SaveGCodeToFile(TextWriter output, GCodeFile file)
        {
            StandardGCodeWriter writer = new StandardGCodeWriter();
            writer.WriteFile(file, output);
        }
    }
}