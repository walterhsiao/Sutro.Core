using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces
{
    public class UserSettingInt<TSettings> : UserSetting<TSettings, int> where TSettings : PlanarAdditiveSettings
    {
        public UserSettingInt(
            string translationKey,
            Func<TSettings, int> loadF,
            Action<TSettings, int> applyF,
            Func<int, ValidationResult> validateF = null) : base(translationKey, loadF, applyF, validateF)
        {

        }
    }

    public static class UserSettingIntValidations
    {
        public static Func<int, ValidationResult> ValidateMin(int min)
        {
            return (val) =>
            {
                if (val < min)
                    return new ValidationResult(ValidationResult.Level.Warning, string.Format("Must be at least {0}", min));
                return new ValidationResult();
            };
        }

        public static Func<int, ValidationResult> ValidateMax(int max)
        {
            return (val) =>
            {
                if (val > max)
                    return new ValidationResult(ValidationResult.Level.Warning, string.Format("Must be no more than {0}", max));
                return new ValidationResult();
            };
        }

        public static Func<int, ValidationResult> ValidateMinMax(int min, int max)
        {
            return (val) =>
            {
                var result = ValidateMin(min).Invoke(val);
                if (result.Severity == ValidationResult.Level.None)
                    result = ValidateMax(max).Invoke(val);
                return result;
            };
        }
    }
}
