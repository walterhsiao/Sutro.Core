using System;

namespace gs.interfaces
{
    public class UserSettingString<TSettings> : UserSetting<TSettings, string> where TSettings : PlanarAdditiveSettings
    {

        public UserSettingString(
            Func<string> nameF,
            Func<string> descriptionF,
            Func<TSettings, string> loadF,
            Action<TSettings, string> applyF,
            Func<string, ValidationResult> validateF = null) : base(nameF, descriptionF, loadF, applyF, validateF)
        {
        }
    }
}
