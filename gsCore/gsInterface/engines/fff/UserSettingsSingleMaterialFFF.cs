using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces
{
    public class UserSettingsSingleMaterialFFF : UserSettingCollection<SingleMaterialFFFSettings>
    {
        public UserSettingInt<SingleMaterialFFFSettings> Shells = new UserSettingInt<SingleMaterialFFFSettings>(
            "Shells",
            (settings) => settings.Shells,
            (settings, val) => settings.Shells = val,
            UserSettingIntValidations.ValidateMin(0));

        public UserSettingInt<SingleMaterialFFFSettings> RoofLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            "RoofLayers",
            (settings) => settings.Shells,
            (settings, val) => settings.Shells = val,
            UserSettingIntValidations.ValidateMin(0));

        public UserSettingInt<SingleMaterialFFFSettings> FloorLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            "FloorLayers",
            (settings) => settings.Shells,
            (settings, val) => settings.Shells = val,
            UserSettingIntValidations.ValidateMin(0));
    }
}
