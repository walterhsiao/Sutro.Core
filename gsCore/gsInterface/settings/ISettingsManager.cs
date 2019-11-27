using System.Collections.Generic;

namespace gs.interfaces
{
    public interface ISettingsManager
    {
        List<object> FactorySettings { get; }

        object FactorySettingByManufacturerAndModel(string manufacturer, string model);

        void ApplyJSON(object settings, string json);
        void ApplyKeyValuePair(object settings, string keyValue);

        IUserSettingCollection UserSettings { get; }
    }


    public interface ISettingsManager<TSettings> : ISettingsManager
    {
        new List<TSettings> FactorySettings { get; }

        new TSettings FactorySettingByManufacturerAndModel(string manufacturer, string model);


        void ApplyJSON(TSettings settings, string json);
        void ApplyKeyValuePair(TSettings settings, string keyValue);

        new IUserSettingCollection<TSettings> UserSettings { get; }
    }
}