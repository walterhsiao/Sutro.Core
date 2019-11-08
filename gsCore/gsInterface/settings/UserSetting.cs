using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces
{
    public abstract class UserSetting
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void LoadFromRaw(object settings);
        public abstract void ApplyToRaw(object settings);
        public abstract ValidationResult Validation { get; }
    }

    public abstract class UserSetting<TSettings> : UserSetting
    {
        public readonly string TranslationKey;
        public override string Name
        {
            get
            {
                var name = TranslationKey + ".Name";
                var result = ""; // TODO: Reenable translations
                //var result = WrapperClassTranslations.ResourceManager.GetString(name);
                if (string.IsNullOrEmpty(result))
                    result = name;
                return result;
            }
        }

        public override string Description
        {
            get
            {
                var name = TranslationKey + ".Description";
                var result = ""; // TODO: Reenable translations
                //var result = WrapperClassTranslations.ResourceManager.GetString(name);
                if (string.IsNullOrEmpty(result))
                    result = name;
                return result;
            }
        }
        public UserSetting(string translationKey)
        {
            TranslationKey = translationKey;
        }

        public override void LoadFromRaw(object settings)
        {
            LoadFromRaw((TSettings)settings);
        }
        public override void ApplyToRaw(object settings)
        {
            ApplyToRaw((TSettings)settings);
        }

        public abstract void LoadFromRaw(TSettings settings);
        public abstract void ApplyToRaw(TSettings settings);
    }

    public class UserSetting<TSettings, TValue> : UserSetting<TSettings>
    {
        public TValue Value { get; set; }

        private readonly Func<TValue, ValidationResult> validateF;
        private readonly Func<TSettings, TValue> loadF;
        private readonly Action<TSettings, TValue> applyF;

        public UserSetting(string translationKey,
            Func<TSettings, TValue> loadF,
            Action<TSettings, TValue> applyF,
            Func<TValue, ValidationResult> validateF = null) : base(translationKey)
        {
            this.validateF = validateF;
            this.applyF = applyF;
            this.loadF = loadF;
        }

        public override void LoadFromRaw(TSettings settings) { Value = loadF(settings); }
        public override void ApplyToRaw(TSettings settings) { applyF(settings, Value); }

        public override ValidationResult Validation
        {
            get
            {
                if (validateF != null)
                    return validateF(Value);
                return new ValidationResult();
            }
        }
    }
}
