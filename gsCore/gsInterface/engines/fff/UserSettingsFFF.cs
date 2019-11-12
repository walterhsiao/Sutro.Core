using System.Globalization;

namespace gs.interfaces
{
    public class UserSettingsFFF : UserSettingCollection<SingleMaterialFFFSettings>
    {
        public static readonly UserSettingGroup GroupBasic = 
            new UserSettingGroup(() => UserSettingTranslations.GroupBasic);

        public UserSettingInt<SingleMaterialFFFSettings> Shells = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.Shells_Name,
            () => UserSettingTranslations.Shells_Description,
            GroupBasic,
            (settings) => settings.Shells,
            (settings, val) => settings.Shells = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> RoofLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.RoofLayers_Name,
            () => UserSettingTranslations.RoofLayers_Description,
            GroupBasic,
            (settings) => settings.RoofLayers,
            (settings, val) => settings.RoofLayers = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> FloorLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.FloorLayers_Name,
            () => UserSettingTranslations.FloorLayers_Description,
            GroupBasic,
            (settings) => settings.FloorLayers,
            (settings, val) => settings.FloorLayers = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        /// <summary>
        /// Sets the culture for name & description strings.
        /// </summary>
        /// <remarks>
        /// Any translation resources used in derived classes should override to set the culture 
        /// on the resource manager, while still calling the base method.
        /// </remarks>
        /// <param name="cultureInfo"></param>
        public override void SetCulture(CultureInfo cultureInfo)
        {
            UserSettingTranslations.Culture = cultureInfo;
        }
    }
}
