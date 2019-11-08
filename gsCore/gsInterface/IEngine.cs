using System.Collections.Generic;

namespace gs.interfaces
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

    public interface IEngine<TSettings> : IEngine
    {
        new IGenerator<TSettings> Generator { get; }
        new ISettingsManager<TSettings> SettingsManager { get; }
    }
}
