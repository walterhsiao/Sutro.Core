using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace gs.interfaces
{
    public interface ISettingsManager
    {
        List<object> FactorySettings { get; }

        void ApplyJSON(object settings, string json);
        void ApplyKeyValuePair(object settings, string keyValue);

        IUserSettingCollection UserSettings { get; }
    }

    public interface ISettingsManager<TSettings> : ISettingsManager
    {
        new List<TSettings> FactorySettings { get; }

        void ApplyJSON(TSettings settings, string json);
        void ApplyKeyValuePair(TSettings settings, string keyValue);

        new IUserSettingCollection<TSettings> UserSettings { get; }
    }

    public abstract class SettingsManager<TSettings> : ISettingsManager<TSettings>
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

        public void ApplyJSON(TSettings settings, string json)
        {
            JsonConvert.PopulateObject(json, settings, jsonSerializerSettings);
        }

        public void ApplyJSON(object settings, string json)
        {
            ApplyJSON((TSettings)settings, json);
        }

        public void ApplyKeyValuePair(TSettings settings, string keyValue)
        {
            // TODO: Make this more strict to avoid converting values unintentionally
            string[] keyValueSplit = keyValue.Split(':');
            if (keyValueSplit.Length != 2)
                throw new Exception("Need setting in \"KeyName:Value\" format; got " + keyValue);
            string sFormatted = "{\"" + keyValueSplit[0] + "\":" + keyValueSplit[1] + "}";

            JsonConvert.PopulateObject(sFormatted, settings, jsonSerializerSettings);
        }

        public void ApplyKeyValuePair(object settings, string keyValue)
        {
            ApplyKeyValuePair((TSettings)settings, keyValue);
        }

    }
}