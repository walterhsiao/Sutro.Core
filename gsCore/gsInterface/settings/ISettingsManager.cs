using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using gs;

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

    public abstract class SettingsManager<TSettings> : ISettingsManager<TSettings> where TSettings : PlanarAdditiveSettings
    {
        public abstract List<TSettings> FactorySettings { get; }
        public abstract IUserSettingCollection<TSettings> UserSettings { get; }

        List<object> ISettingsManager.FactorySettings
        {
            get {
                var settings = new List<object>();
                foreach (TSettings setting in FactorySettings)
                    settings.Add(setting);
                return settings;
            }
        }

        IUserSettingCollection ISettingsManager.UserSettings => UserSettings;

        private static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,
        };

        public virtual void ApplyJSON(TSettings settings, string json)
        {
            JsonConvert.PopulateObject(json, settings, jsonSerializerSettings);
        }

        public void ApplyJSON(object settings, string json)
        {
            ApplyJSON((TSettings)settings, json);
        }

        public virtual void ApplyKeyValuePair(TSettings settings, string keyValue)
        {
            // TODO: Make this more strict to avoid converting values unintentionally
            var sFormatted = StringUtil.FormatSettingOverride(keyValue);
            JsonConvert.PopulateObject(sFormatted, settings, jsonSerializerSettings);
        }

        public void ApplyKeyValuePair(object settings, string keyValue)
        {
            ApplyKeyValuePair((TSettings)settings, keyValue);
        }

        public virtual TSettings FactorySettingByManufacturerAndModel(string manufacturer, string model)
        {
            Func<string, string> SimplifyName = (s) => s.Replace(" ", "").Replace("_", "").ToLower();

            Func<string, string, bool> MatchName = (a, b) => SimplifyName(a) == SimplifyName(b); 


            var profiles = (from profile in FactorySettings
                            where MatchName(profile.BaseMachine.ManufacturerName, manufacturer) &&
                                  MatchName(profile.BaseMachine.ModelIdentifier, model)
                            select profile).ToArray();

            if (profiles.Length == 0)
                throw new KeyNotFoundException($"Matching profile not found for: {manufacturer} {model}");

            return profiles[0];
        }

        object ISettingsManager.FactorySettingByManufacturerAndModel(string manufacturer, string model)
        {
            return FactorySettingByManufacturerAndModel(manufacturer, model);
        }
    }
}