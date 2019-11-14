using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces {
    public class UserSettingBool<TSettings> : UserSetting<TSettings, bool> where TSettings : PlanarAdditiveSettings {
        public UserSettingBool(
            Func<string> nameF,
            Func<string> descriptionF,
            UserSettingGroup group,
            Func<TSettings, bool> loadF,
            Action<TSettings, bool> applyF,
            Func<bool, ValidationResult> validateF = null) : base(nameF, descriptionF, group, loadF, applyF, validateF) {

        }
    }
}
