using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace gs.interfaces
{
    public abstract class UserSetting
    {
        public readonly Func<string> NameF;
        public readonly Func<string> DescriptionF;
        public abstract void LoadFromRaw(object settings);
        public abstract void ApplyToRaw(object settings);
        public abstract ValidationResult Validation { get; }
        public UserSetting(Func<string> nameF, Func<string> descriptionF = null)
        {
            NameF = nameF;
            DescriptionF = descriptionF;
        }
    }

    public abstract class UserSetting<TSettings> : UserSetting
    {
        public UserSetting(Func<string> nameF, Func<string> descriptionF = null) : base(nameF, descriptionF)
        {
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

        public UserSetting(
            Func<string> nameF,
            Func<string> descriptionF,
            Func<TSettings, TValue> loadF,
            Action<TSettings, TValue> applyF,
            Func<TValue, ValidationResult> validateF = null) : base(nameF, descriptionF)
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
