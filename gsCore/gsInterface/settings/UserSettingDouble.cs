using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces {
    public class UserSettingDouble<TSettings> : UserSetting<TSettings, double> where TSettings : PlanarAdditiveSettings {
        public UserSettingDouble(
            Func<string> nameF,
            Func<string> descriptionF,
            UserSettingGroup group,
            Func<TSettings, double> loadF,
            Action<TSettings, double> applyF,
            Func<double, ValidationResult> validateF = null) : base(nameF, descriptionF, group, loadF, applyF, validateF) {

        }
    }
}
