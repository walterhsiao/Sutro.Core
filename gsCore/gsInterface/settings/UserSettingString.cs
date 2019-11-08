using System;

namespace gs.interfaces
{
    public class UserSettingString<TSettings> : UserSetting<TSettings, string> where TSettings : PlanarAdditiveSettings
    {

        public UserSettingString(
            string translationKey,
            Func<TSettings, string> loadF,
            Action<TSettings, string> applyF,
            Func<string, ValidationResult> validateF = null) : base(translationKey, loadF, applyF, validateF)
        {
        }
    }
}
