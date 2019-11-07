using System.Collections.Generic;

namespace gs
{
    public interface IEngineData
    {
        string Name { get; }
        string Description { get; }
    }

    public interface IEngine
    {
        IGenerator Generator { get; }
        ISettingsManager SettingsManager { get; }
        List<IVisualizer> Visualizers { get; }
    }
}
