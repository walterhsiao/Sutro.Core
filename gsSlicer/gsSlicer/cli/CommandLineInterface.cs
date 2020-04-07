using CommandLine;
using CommandLine.Text;
using g3;
using System;
using System.Collections.Generic;
using System.IO;

namespace gs
{
    public class CommandLineInterface
    {
        private readonly ILogger logger;
        private readonly Dictionary<string, IPrintGeneratorManager> printGeneratorDict;

        private IPrintGeneratorManager printGeneratorManager;

        public CommandLineInterface(ILogger logger, IEnumerable<IPrintGeneratorManager> printGenerators)
        {
            this.logger = logger;

            printGeneratorDict = new Dictionary<string, IPrintGeneratorManager>();
            foreach (var printGenerator in printGenerators)
                printGeneratorDict.Add(printGenerator.Id, printGenerator);
        }

        public void Execute(string[] args)
        {
            var parser = new Parser(with => with.HelpWriter = null);

            var parserResult = parser.ParseArguments<CommandLineOptions>(args);

            parserResult.WithParsed(ParsingSuccessful);

            parserResult.WithNotParsed((err) => ParsingUnsuccessful(err, parserResult));
        }

        protected static bool OutputFilePathIsValid(CommandLineOptions o)
        {
            if (o.GCodeFilePath is null || !Directory.Exists(Directory.GetParent(o.GCodeFilePath).ToString()))
            {
                Console.WriteLine("Must provide valid gcode file path as second argument.");
                return false;
            }
            return true;
        }

        protected virtual void CenterMeshAboveOrigin(DMesh3 mesh)
        {
            MeshTransforms.Translate(mesh, new Vector3d(-mesh.CachedBounds.Center.x, -mesh.CachedBounds.Center.y, 0));
        }

        protected virtual void ConsoleWriteSeparator()
        {
            logger.WriteLine("".PadRight(79, '-'));
        }

        protected virtual void ConstructSettings(CommandLineOptions o)
        {
            foreach (var file in o.SettingsFiles)
            {
                printGeneratorManager.SettingsBuilder.ApplyJSONFile(file);
            }

            foreach (var snippet in o.SettingsOverride)
            {
                printGeneratorManager.SettingsBuilder.ApplyJSONSnippet(snippet);
            }
        }

        protected virtual void DropMeshToBuildPlate(DMesh3 mesh)
        {
            MeshTransforms.Translate(mesh, new Vector3d(0, 0, mesh.CachedBounds.Extents.z - mesh.CachedBounds.Center.z));
        }

        protected void GenerateGCode(DMesh3 mesh, out GCodeFile gcode, out IEnumerable<string> generationReport)
        {
            ConsoleWriteSeparator();
            logger.WriteLine($"GENERATION");
            logger.WriteLine();

            gcode = printGeneratorManager.GCodeFromMesh(mesh, out generationReport);
        }

        protected virtual void LoadMesh(CommandLineOptions o, out DMesh3 mesh)
        {
            if (printGeneratorManager.AcceptsParts)
            {
                string fMeshFilePath = Path.GetFullPath(o.MeshFilePath);
                ConsoleWriteSeparator();
                logger.WriteLine($"PARTS");
                logger.WriteLine();

                logger.Write("Loading mesh " + fMeshFilePath + "...");
                mesh = StandardMeshReader.ReadMesh(fMeshFilePath);
                logger.WriteLine(" done.");

                logger.Write("Repairing mesh... ");
                bool repaired = new MeshAutoRepair(mesh).Apply();
                logger.WriteLine(repaired ? "repaired." : "not repaired.");

                if (o.CenterXY) CenterMeshAboveOrigin(mesh);
                if (o.DropZ) DropMeshToBuildPlate(mesh);
            }
            else
            {
                mesh = null;
            }
        }

        protected virtual bool MeshFilePathIsValid(CommandLineOptions o)
        {
            if (printGeneratorManager.AcceptsParts && (o.MeshFilePath is null || !File.Exists(o.MeshFilePath)))
            {
                Console.WriteLine("Must provide valid mesh file path as third argument.");
                Console.WriteLine(Path.GetFullPath(o.MeshFilePath));
                return false;
            }
            return true;
        }

        protected virtual void OutputGenerationReport(IEnumerable<string> generationReport)
        {
            ConsoleWriteSeparator();

            foreach (var s in generationReport)
            {
                logger.WriteLine(s);
            }

            logger.WriteLine();
            logger.WriteLine("Print generation complete.");
        }

        protected virtual void OutputVersionInfo()
        {
            ConsoleWriteSeparator();
            var version = printGeneratorManager.PrintGeneratorAssemblyVersion;
            logger.WriteLine($"Using {printGeneratorManager.PrintGeneratorName} from {printGeneratorManager.PrintGeneratorAssemblyName} v{version.Major}.{version.Minor}.{version.Revision}");
            logger.WriteLine();
        }

        protected void ParsingSuccessful(CommandLineOptions o)
        {
            if (!printGeneratorDict.TryGetValue(o.Generator, out printGeneratorManager))
            {
                HandleInvalidGeneratorId(o.Generator);
            }

            OutputVersionInfo();

            if (!MeshFilePathIsValid(o)) return;

            if (!OutputFilePathIsValid(o)) return;

            ConstructSettings(o);

            LoadMesh(o, out var mesh);

            GenerateGCode(mesh, out var gcode, out var generationReport);

            WriteGCodeToFile(o.GCodeFilePath, gcode);

            OutputGenerationReport(generationReport);
        }

        protected virtual void HandleInvalidGeneratorId(string id)
        {
            logger.WriteLine($"Invalid generator id: {id}");
            logger.WriteLine();

            logger.WriteLine("Available generators:");
            ListAvailableGenerators();
        }

        private void ListAvailableGenerators()
        {
            foreach (var g in printGeneratorDict.Values)
            {
                logger.WriteLine($"{g.Id}: {g.PrintGeneratorName} - {g.Description}");
            }
        }

        protected virtual void ParsingUnsuccessful(IEnumerable<Error> errs, ParserResult<CommandLineOptions> parserResult)
        {
            logger.WriteLine("ERRORS:");
            foreach (var err in errs)
                logger.WriteLine(err);
            logger.WriteLine();

            logger.WriteLine("HELP:");
            var helpText = HelpText.AutoBuild(parserResult, h => h, e => e);
            logger.WriteLine(helpText.ToString());
            logger.WriteLine();

            logger.WriteLine("GENERATORS:");
            ListAvailableGenerators();
            logger.WriteLine();
        }

        protected virtual string VersionToString(Version v)
        {
            return $"v{v.Major}.{v.Minor}.{v.Revision}";
        }

        protected virtual void WriteGCodeToFile(string filePath, GCodeFile gcode)
        {
            string gcodeFilePath = Path.GetFullPath(filePath);
            logger.WriteLine($"Writing gcode to {gcodeFilePath}");
            using (StreamWriter w = new StreamWriter(gcodeFilePath))
            {
                printGeneratorManager.SaveGCodeToFile(w, gcode);
            }
        }
    }
}