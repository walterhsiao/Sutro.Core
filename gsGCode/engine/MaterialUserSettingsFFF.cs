using System.Globalization;
using Sutro.PathWorks.Plugins.API;

namespace gs.engines
{
    public class MaterialUserSettingsFFF<TSettings> : UserSettingCollection<TSettings> where TSettings : SingleMaterialFFFSettings
    {
        #region Identifiers

        public static readonly UserSettingGroup GroupMaterialIdentifiers =
            new UserSettingGroup(() => UserSettingTranslations.GroupMaterialIdentifiers);

        public UserSettingString<TSettings> MaterialType = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.MaterialType",
            () => UserSettingTranslations.MaterialType_Name,
            () => UserSettingTranslations.MaterialType_Description,
            GroupMaterialIdentifiers,
            (settings) => settings.MaterialType,
            (settings, val) => settings.MaterialType = val);

        public UserSettingString<TSettings> MaterialSource = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.MaterialSource",
            () => UserSettingTranslations.MaterialSource_Name,
            () => UserSettingTranslations.MaterialSource_Description,
            GroupMaterialIdentifiers,
            (settings) => settings.MaterialSource,
            (settings, val) => settings.MaterialSource = val);

        public UserSettingString<TSettings> MaterialColor = new UserSettingString<TSettings>(
            "MaterialUserSettingsFFF.MaterialColor",
            () => UserSettingTranslations.MaterialColor_Name,
            () => UserSettingTranslations.MaterialColor_Description,
            GroupMaterialIdentifiers,
            (settings) => settings.MaterialColor,
            (settings, val) => settings.MaterialColor = val);

        #endregion Identifiers

        #region Basic

        public static readonly UserSettingGroup GroupBasic =
            new UserSettingGroup(() => UserSettingTranslations.GroupBasic);

        public UserSettingDouble<TSettings> FilamentDiamMM = new UserSettingDouble<TSettings>(
            "MaterialUserSettingsFFF.FilamentDiamMM",
            () => UserSettingTranslations.FilamentDiamMM_Name,
            () => UserSettingTranslations.FilamentDiamMM_Description,
            GroupBasic,
            (settings) => settings.Machine.FilamentDiamMM,
            (settings, val) => settings.Machine.FilamentDiamMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion Basic

        #region Temperature

        public static readonly UserSettingGroup GroupTemperature =
            new UserSettingGroup(() => UserSettingTranslations.GroupTemperature);

        public UserSettingInt<TSettings> ExtruderTempC = new UserSettingInt<TSettings>(
            "MaterialUserSettingsFFF.ExtruderTempC",
            () => UserSettingTranslations.ExtruderTempC_Name,
            () => UserSettingTranslations.ExtruderTempC_Description,
            GroupTemperature,
            (settings) => settings.ExtruderTempC,
            (settings, val) => settings.ExtruderTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        public UserSettingInt<TSettings> HeatedBedTempC = new UserSettingInt<TSettings>(
            "MaterialUserSettingsFFF.HeatedBedTempC",
            () => UserSettingTranslations.HeatedBedTempC_Name,
            () => UserSettingTranslations.HeatedBedTempC_Description,
            GroupTemperature,
            (settings) => settings.HeatedBedTempC,
            (settings, val) => settings.HeatedBedTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        #endregion Temperature

        #region Retraction

        public static readonly UserSettingGroup GroupRetraction =
            new UserSettingGroup(() => UserSettingTranslations.GroupRetraction);

        public UserSettingDouble<TSettings> MinRetractTravelLength = new UserSettingDouble<TSettings>(
            "MaterialUserSettingsFFF.MinRetractTravelLength",
            () => UserSettingTranslations.MinRetractTravelLength_Name,
            () => UserSettingTranslations.MinRetractTravelLength_Description,
            GroupRetraction,
            (settings) => settings.MinRetractTravelLength,
            (settings, val) => settings.MinRetractTravelLength = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<TSettings> RetractDistanceMM = new UserSettingDouble<TSettings>(
            "MaterialUserSettingsFFF.RetractDistanceMM",
            () => UserSettingTranslations.RetractDistanceMM_Name,
            () => UserSettingTranslations.RetractDistanceMM_Description,
            GroupRetraction,
            (settings) => settings.RetractDistanceMM,
            (settings, val) => settings.RetractDistanceMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<TSettings> RetractSpeed = new UserSettingDouble<TSettings>(
            "MaterialUserSettingsFFF.RetractSpeed",
            () => UserSettingTranslations.RetractSpeed_Name,
            () => UserSettingTranslations.RetractSpeed_Description,
            GroupRetraction,
            (settings) => settings.RetractSpeed,
            (settings, val) => settings.RetractSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        # endregion

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