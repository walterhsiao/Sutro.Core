using System;

namespace gs.interfaces
{
    public class UserSettingString<TSettings> : UserSetting<TSettings, string> where TSettings : PlanarAdditiveSettings
    {

        public UserSettingString(
            Func<string> nameF,
            Func<string> descriptionF,
            UserSettingGroup group,
            Func<TSettings, string> loadF,
            Action<TSettings, string> applyF,
            Func<string, ValidationResult> validateF = null) : base(nameF, descriptionF, group, loadF, applyF, validateF)
        {
        }
    }

    public static class UserSettingEnumValidations<T> where T : IComparable<T> {
        public static Func<T, ValidationResult> ValidateContains(T[] array, ValidationResult.Level level) {
            return (val) =>
            {
                if (!Array.Exists(array, (elem) => val.Equals(elem)))
                    return new ValidationResult(level, string.Format("Must be one of ", string.Join(", ", array)));
                return new ValidationResult();
            };
        }
    }
}
