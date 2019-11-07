using System.Collections.Generic;

namespace gs
{
    public interface ISettingsManager
    {
        List<PlanarAdditiveSettings> FactorySettings { get; }

        void MigrateJSON();
        void ApplyJSON(ISettings settings, string json);
        void ApplyKeyValuePair(ISettings settings, string keyValue);

        ISettingsWrapped WrappedSettings { get; }
    }
}