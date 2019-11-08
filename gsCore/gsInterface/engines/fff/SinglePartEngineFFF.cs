using g3;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using gs.interfaces;

namespace gs
{
    [Export(typeof(IEngine))]
    [ExportMetadata("Name", "core_fff")]
    [ExportMetadata("Description", "Provides access to the basic print generator included in gsCore. Can only create gcode for a single mesh with single material.")]
    public class EngineFFF : IEngine
    {
        public IGenerator Generator => new SinglePartGenerator<SingleMaterialFFFPrintGenerator, GenericRepRapSettings>();
        public ISettingsManager SettingsManager => new SingleMaterialFFFSettingsManager();
        public List<IVisualizer> Visualizers => null;
    }
}
