using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace gs.interfaces
{
    [Export(typeof(IEngine))]
    [ExportMetadata("Name", "fff")]
    [ExportMetadata("Description", "Provides access to the basic print generator included in gsCore. Can only create gcode for a single mesh with single material.")]
    public class EngineFFF : Engine<SingleMaterialFFFSettings>
    {
        public override List<IVisualizer> Visualizers => null;

        public override IGenerator<SingleMaterialFFFSettings> Generator => 
            new SinglePartGenerator<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings>();

        public override ISettingsManager<SingleMaterialFFFSettings> SettingsManager => 
            new SettingsManagerFFF();
    }
}
