using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace gs.interfaces
{
    /// <summary>
    /// Provides a common interface to raw settings. 
    /// </summary>
    /// <remarks>
    /// The common version of this interface is provided so that calling code 
    /// doesn't need to know about the type of the underlying raw setting.
    /// However, concrete classes should not use this interface directly;
    /// rather, they should implement the typed version IUserSettingCollection<TSettings>
    /// to enforce matching types for the raw settings object.
    /// </remarks>
    public interface IUserSettingCollection
    {
        IEnumerable<UserSetting> Settings();

        List<ValidationResult> Validate();

        void LoadFromRaw(object rawSettings,
                         IEnumerable<UserSetting> userSettings);

        void ApplyToRaw(object rawSettings, 
                        IEnumerable<UserSetting> userSettings);
    }

    public interface IUserSettingCollection<TSettings> : IUserSettingCollection
    {
        new IEnumerable<UserSetting<TSettings>> Settings();
        
        void LoadFromRaw(TSettings rawSettings, 
                         IEnumerable<UserSetting<TSettings>> userSettings);
        
        void ApplyToRaw(TSettings rawSettings, 
                        IEnumerable<UserSetting<TSettings>> userSettings);
    }

    public abstract class UserSettingCollection<TSettings> : IUserSettingCollection<TSettings>
    {
        /// <summary>
        /// Provides iteration through user settings typed with underlying raw settings type.
        /// </summary>
        public IEnumerable<UserSetting<TSettings>> Settings()
        {
            PropertyInfo[] properties = typeof(UserSettingCollection<TSettings>).GetProperties();
            foreach (PropertyInfo property in properties)
                if (typeof(UserSetting<TSettings>).IsAssignableFrom(property.PropertyType))
                    yield return (UserSetting<TSettings>)property.GetValue(this);
        }

        /// <summary>
        /// Provides iteration through user settings without needing underlying raw settings type. 
        /// </summary>
        /// <remarks>
        /// Common version of generic method IUserSettingCollection<TSettings>.Settings()
        /// </remarks>
        IEnumerable<UserSetting> IUserSettingCollection.Settings()
        {
            foreach (var setting in Settings())
            {
                yield return setting;
            }
        }

        /// <summary>
        /// Checks the individual validations of each user setting.
        /// </summary>
        /// <remarks>
        /// This method can be overridden in derived classes to add validations
        /// between combinations of user settings in addition to the individual checks.
        /// </remarks>
        public virtual List<ValidationResult> Validate()
        {
            var validations = new List<ValidationResult>();
            foreach (var setting in Settings())
            {
                var validation = setting.Validation;
                if (validation.Severity != ValidationResult.Level.None)
                {
                    validations.Add(validation);
                }
            }
            return validations;
        }

        /// <summary>
        /// Loads values from raw settings into a collection of user settings.
        /// </summary>
        public void LoadFromRaw(TSettings rawSettings, IEnumerable<UserSetting<TSettings>> userSettings)
        {
            foreach (var setting in userSettings)
            {
                setting.LoadFromRaw(rawSettings);
            }
        }

        /// <summary>
        /// Loads values from raw settings into a collection of user settings.
        /// </summary>
        public void LoadFromRaw(object rawSettings, IEnumerable<UserSetting> userSettings)
        {
            LoadFromRaw((TSettings)rawSettings, (IEnumerable<UserSetting<TSettings>>)userSettings);
        }

        /// <summary>
        /// Loads values from collection of user settings into raw settings.
        /// </summary>
        public void ApplyToRaw(TSettings rawSettings, IEnumerable<UserSetting<TSettings>> userSettings)
        {
            foreach (var setting in userSettings)
            {
                setting.ApplyToRaw(rawSettings);
            }
        }

        /// <summary>
        /// Loads values from collection of user settings into raw settings.
        /// </summary>
        public void ApplyToRaw(object rawSettings, IEnumerable<UserSetting> userSettings)
        {
            ApplyToRaw((TSettings)rawSettings, (IEnumerable<UserSetting<TSettings>>)userSettings);
        }
    }


}