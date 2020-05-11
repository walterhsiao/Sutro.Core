using g3;
using gs;
using System;
using System.IO;

namespace gsCore.FunctionalTests.Utility
{
    public class ResultGenerator<TGenerator, TSettings> : IResultGenerator
        where TGenerator : IPrintGenerator<TSettings>, new()
        where TSettings : PlanarAdditiveSettings, new()
    {
        private readonly PrintGeneratorManager<TGenerator, TSettings> generator;
        private readonly ILogger logger;

        public ResultGenerator(PrintGeneratorManager<TGenerator, TSettings> generator, ILogger logger)
        {
            this.generator = generator;
            this.logger = logger;
        }

        protected void SaveGCode(string path, GCodeFile gcode)
        {
            logger.WriteLine($"Saving file to {path}");
            using var streamWriter = new StreamWriter(path);
            generator.SaveGCodeToFile(streamWriter, gcode);
        }

        public void GenerateResultFile(string meshFilePath, string outputFilePath)
        {
            var mesh = StandardMeshReader.ReadMesh(meshFilePath);
            var gcode = generator.GCodeFromMesh(mesh, out _);
            SaveGCode(outputFilePath, gcode);
        }
    }
}