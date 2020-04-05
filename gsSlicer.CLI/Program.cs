using CommandLine;
using CommandLine.Text;
using g3;
using gs;
using System;
using System.Collections.Generic;
using System.IO;

namespace gsSlicer.CLI
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var logger = new ConsoleLogger();

            var cli = new CommandLineInterface(
                logger: logger,
                printGeneratorManager: new PrintGeneratorManager<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings>(logger, new GenericRepRapSettings()));

            cli.Execute(args);
        }
    }
}