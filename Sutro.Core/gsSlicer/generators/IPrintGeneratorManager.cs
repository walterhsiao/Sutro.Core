using g3;
using Sutro.Core.Models.GCode;
using System;
using System.Collections.Generic;
using System.IO;

namespace gs
{
    public interface IPrintGeneratorManager
    {
        string Id { get; }
        string Description { get; }

        Version PrintGeneratorAssemblyVersion { get; }
        string PrintGeneratorAssemblyName { get; }
        string PrintGeneratorName { get; }

        bool AcceptsParts { get; }
        ISettingsBuilder SettingsBuilder { get; }

        GCodeFile GCodeFromMesh(DMesh3 mesh, out IEnumerable<string> generationReport);

        void SaveGCodeToFile(TextWriter output, GCodeFile file);
    }
}