using System.Globalization;

namespace gs.interfaces {
    public class UserSettingsFFF : UserSettingCollection<SingleMaterialFFFSettings> {

        #region Advanced

        public static readonly UserSettingGroup GroupAdvanced =
            new UserSettingGroup(() => UserSettingTranslations.GroupAdvanced);

        public UserSettingBool<SingleMaterialFFFSettings> EnableAutoBedLeveling = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.EnableAutoBedLeveling_Name,
            () => UserSettingTranslations.EnableAutoBedLeveling_Description,
            GroupAdvanced,
            (settings) => settings.Machine.EnableAutoBedLeveling,
            (settings, val) => settings.Machine.EnableAutoBedLeveling = val);

        #endregion
        #region Basic

        public static readonly UserSettingGroup GroupBasic =
            new UserSettingGroup(() => UserSettingTranslations.GroupBasic);

        public UserSettingBool<SingleMaterialFFFSettings> EnableBridging = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.EnableBridging_Name,
            () => UserSettingTranslations.EnableBridging_Description,
            GroupBasic,
            (settings) => settings.EnableBridging,
            (settings, val) => settings.EnableBridging = val);

        public UserSettingInt<SingleMaterialFFFSettings> ExtruderTempC = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ExtruderTempC_Name,
            () => UserSettingTranslations.ExtruderTempC_Description,
            GroupBasic,
            (settings) => settings.ExtruderTempC,
            (settings, val) => settings.ExtruderTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> FilamentDiamMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.FilamentDiamMM_Name,
            () => UserSettingTranslations.FilamentDiamMM_Description,
            GroupBasic,
            (settings) => settings.Machine.FilamentDiamMM,
            (settings, val) => settings.Machine.FilamentDiamMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> FloorLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.FloorLayers_Name,
            () => UserSettingTranslations.FloorLayers_Description,
            GroupBasic,
            (settings) => settings.FloorLayers,
            (settings, val) => settings.FloorLayers = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingBool<SingleMaterialFFFSettings> GenerateSupport = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.GenerateSupport_Name,
            () => UserSettingTranslations.GenerateSupport_Description,
            GroupBasic,
            (settings) => settings.GenerateSupport,
            (settings, val) => settings.GenerateSupport = val);

        public UserSettingInt<SingleMaterialFFFSettings> HeatedBedTempC = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.HeatedBedTempC_Name,
            () => UserSettingTranslations.HeatedBedTempC_Description,
            GroupBasic,
            (settings) => settings.HeatedBedTempC,
            (settings, val) => settings.HeatedBedTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> LayerHeightMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.LayerHeightMM_Name,
            () => UserSettingTranslations.LayerHeightMM_Description,
            GroupBasic,
            (settings) => settings.LayerHeightMM,
            (settings, val) => settings.LayerHeightMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> MinExtrudeSpeed = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinExtrudeSpeed_Name,
            () => UserSettingTranslations.MinExtrudeSpeed_Description,
            GroupBasic,
            (settings) => settings.MinExtrudeSpeed,
            (settings, val) => settings.MinExtrudeSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> RoofLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.RoofLayers_Name,
            () => UserSettingTranslations.RoofLayers_Description,
            GroupBasic,
            (settings) => settings.RoofLayers,
            (settings, val) => settings.RoofLayers = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> Shells = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.Shells_Name,
            () => UserSettingTranslations.Shells_Description,
            GroupBasic,
            (settings) => settings.Shells,
            (settings, val) => settings.Shells = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Bridging

        public static readonly UserSettingGroup GroupBridging =
            new UserSettingGroup(() => UserSettingTranslations.GroupBridging);

        public UserSettingDouble<SingleMaterialFFFSettings> BridgeExtrudeSpeedX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.BridgeExtrudeSpeedX_Name,
            () => UserSettingTranslations.BridgeExtrudeSpeedX_Description,
            GroupBridging,
            (settings) => settings.BridgeExtrudeSpeedX,
            (settings, val) => settings.BridgeExtrudeSpeedX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> BridgeFillNozzleDiamStepX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.BridgeFillNozzleDiamStepX_Name,
            () => UserSettingTranslations.BridgeFillNozzleDiamStepX_Description,
            GroupBridging,
            (settings) => settings.BridgeFillNozzleDiamStepX,
            (settings, val) => settings.BridgeFillNozzleDiamStepX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> BridgeVolumeScale = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.BridgeVolumeScale_Name,
            () => UserSettingTranslations.BridgeVolumeScale_Description,
            GroupBridging,
            (settings) => settings.BridgeVolumeScale,
            (settings, val) => settings.BridgeVolumeScale = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> MaxBridgeWidthMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxBridgeWidthMM_Name,
            () => UserSettingTranslations.MaxBridgeWidthMM_Description,
            GroupBridging,
            (settings) => settings.MaxBridgeWidthMM,
            (settings, val) => settings.MaxBridgeWidthMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Capabilities

        public static readonly UserSettingGroup GroupCapabilities =
            new UserSettingGroup(() => UserSettingTranslations.GroupCapabilities);

        public UserSettingBool<SingleMaterialFFFSettings> HasAutoBedLeveling = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.HasAutoBedLeveling_Name,
            () => UserSettingTranslations.HasAutoBedLeveling_Description,
            GroupCapabilities,
            (settings) => settings.Machine.HasAutoBedLeveling,
            (settings, val) => settings.Machine.HasAutoBedLeveling = val);

        public UserSettingBool<SingleMaterialFFFSettings> HasHeatedBed = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.HasHeatedBed_Name,
            () => UserSettingTranslations.HasHeatedBed_Description,
            GroupCapabilities,
            (settings) => settings.Machine.HasHeatedBed,
            (settings, val) => settings.Machine.HasHeatedBed = val);

        public UserSettingDouble<SingleMaterialFFFSettings> MaxLayerHeightMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxLayerHeightMM_Name,
            () => UserSettingTranslations.MaxLayerHeightMM_Description,
            GroupCapabilities,
            (settings) => settings.Machine.MaxLayerHeightMM,
            (settings, val) => settings.Machine.MaxLayerHeightMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> MinLayerHeightMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinLayerHeightMM_Name,
            () => UserSettingTranslations.MinLayerHeightMM_Description,
            GroupCapabilities,
            (settings) => settings.Machine.MinLayerHeightMM,
            (settings, val) => settings.Machine.MinLayerHeightMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Extruder

        public static readonly UserSettingGroup GroupExtruder =
            new UserSettingGroup(() => UserSettingTranslations.GroupExtruder);

        public UserSettingInt<SingleMaterialFFFSettings> MaxExtruderTempC = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxExtruderTempC_Name,
            () => UserSettingTranslations.MaxExtruderTempC_Description,
            GroupExtruder,
            (settings) => settings.Machine.MaxExtruderTempC,
            (settings, val) => settings.Machine.MaxExtruderTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> MinExtruderTempC = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinExtruderTempC_Name,
            () => UserSettingTranslations.MinExtruderTempC_Description,
            GroupExtruder,
            (settings) => settings.Machine.MinExtruderTempC,
            (settings, val) => settings.Machine.MinExtruderTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> NozzleDiamMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.NozzleDiamMM_Name,
            () => UserSettingTranslations.NozzleDiamMM_Description,
            GroupExtruder,
            (settings) => settings.Machine.NozzleDiamMM,
            (settings, val) => settings.Machine.NozzleDiamMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region FirstLayer

        public static readonly UserSettingGroup GroupFirstLayer =
            new UserSettingGroup(() => UserSettingTranslations.GroupFirstLayer);

        public UserSettingDouble<SingleMaterialFFFSettings> CarefulExtrudeSpeed = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.CarefulExtrudeSpeed_Name,
            () => UserSettingTranslations.CarefulExtrudeSpeed_Description,
            GroupFirstLayer,
            (settings) => settings.CarefulExtrudeSpeed,
            (settings, val) => settings.CarefulExtrudeSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> StartLayerHeightMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.StartLayerHeightMM_Name,
            () => UserSettingTranslations.StartLayerHeightMM_Description,
            GroupFirstLayer,
            (settings) => settings.StartLayerHeightMM,
            (settings, val) => settings.StartLayerHeightMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> StartLayers = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.StartLayers_Name,
            () => UserSettingTranslations.StartLayers_Description,
            GroupFirstLayer,
            (settings) => settings.StartLayers,
            (settings, val) => settings.StartLayers = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region HeatedBed

        public static readonly UserSettingGroup GroupHeatedBed =
            new UserSettingGroup(() => UserSettingTranslations.GroupHeatedBed);

        public UserSettingInt<SingleMaterialFFFSettings> MaxBedTempC = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxBedTempC_Name,
            () => UserSettingTranslations.MaxBedTempC_Description,
            GroupHeatedBed,
            (settings) => settings.Machine.MaxBedTempC,
            (settings, val) => settings.Machine.MaxBedTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> MinBedTempC = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinBedTempC_Name,
            () => UserSettingTranslations.MinBedTempC_Description,
            GroupHeatedBed,
            (settings) => settings.Machine.MinBedTempC,
            (settings, val) => settings.Machine.MinBedTempC = val,
            UserSettingNumericValidations<int>.ValidateMin(-273, ValidationResult.Level.Error));

        #endregion
        #region Hidden

        public static readonly UserSettingGroup GroupHidden =
            new UserSettingGroup(() => UserSettingTranslations.GroupHidden);

        public UserSettingString<SingleMaterialFFFSettings> ManufacturerName = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ManufacturerName_Name,
            () => UserSettingTranslations.ManufacturerName_Description,
            GroupHidden,
            (settings) => settings.Machine.ManufacturerName,
            (settings, val) => settings.Machine.ManufacturerName = val);

        public UserSettingString<SingleMaterialFFFSettings> ManufacturerUUID = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ManufacturerUUID_Name,
            () => UserSettingTranslations.ManufacturerUUID_Description,
            GroupHidden,
            (settings) => settings.Machine.ManufacturerUUID,
            (settings, val) => settings.Machine.ManufacturerUUID = val);

        public UserSettingString<SingleMaterialFFFSettings> ModelIdentifier = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ModelIdentifier_Name,
            () => UserSettingTranslations.ModelIdentifier_Description,
            GroupHidden,
            (settings) => settings.Machine.ModelIdentifier,
            (settings, val) => settings.Machine.ModelIdentifier = val);

        public UserSettingString<SingleMaterialFFFSettings> ModelUUID = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ModelUUID_Name,
            () => UserSettingTranslations.ModelUUID_Description,
            GroupHidden,
            (settings) => settings.Machine.ModelUUID,
            (settings, val) => settings.Machine.ModelUUID = val);

        public UserSettingString<SingleMaterialFFFSettings> Class = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.Class_Name,
            () => UserSettingTranslations.Class_Description,
            GroupHidden,
            (settings) => settings.Machine.Class.ToString(),
            (settings, val) => settings.Machine.Class = (MachineClass)System.Enum.Parse(typeof(MachineClass), val),
            UserSettingEnumValidations<string>.ValidateContains(System.Enum.GetNames(typeof(MachineClass)), ValidationResult.Level.Error));

        public UserSettingString<SingleMaterialFFFSettings> ClassTypeName = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ClassTypeName_Name,
            () => UserSettingTranslations.ClassTypeName_Description,
            GroupHidden,
            (settings) => settings.ClassTypeName,
            (settings, val) => throw new System.Exception("ClassTypeName is read-only!"));

        public UserSettingString<SingleMaterialFFFSettings> Identifier = new UserSettingString<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.Identifier_Name,
            () => UserSettingTranslations.Identifier_Description,
            GroupHidden,
            (settings) => settings.Identifier,
            (settings, val) => settings.Identifier = val);

        public UserSettingDouble<SingleMaterialFFFSettings> MinPointSpacingMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinPointSpacingMM_Name,
            () => UserSettingTranslations.MinPointSpacingMM_Description,
            GroupHidden,
            (settings) => settings.Machine.MinPointSpacingMM,
            (settings, val) => settings.Machine.MinPointSpacingMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Miscellaneous

        public static readonly UserSettingGroup GroupMiscellaneous =
            new UserSettingGroup(() => UserSettingTranslations.GroupMiscellaneous);

        public UserSettingDouble<SingleMaterialFFFSettings> MinLayerTime = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinLayerTime_Name,
            () => UserSettingTranslations.MinLayerTime_Description,
            GroupMiscellaneous,
            (settings) => settings.MinLayerTime,
            (settings, val) => settings.MinLayerTime = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Perimeters

        public static readonly UserSettingGroup GroupPerimeters =
            new UserSettingGroup(() => UserSettingTranslations.GroupPerimeters);

        public UserSettingBool<SingleMaterialFFFSettings> ClipSelfOverlaps = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ClipSelfOverlaps_Name,
            () => UserSettingTranslations.ClipSelfOverlaps_Description,
            GroupPerimeters,
            (settings) => settings.ClipSelfOverlaps,
            (settings, val) => settings.ClipSelfOverlaps = val);

        public UserSettingInt<SingleMaterialFFFSettings> InteriorSolidRegionShells = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.InteriorSolidRegionShells_Name,
            () => UserSettingTranslations.InteriorSolidRegionShells_Description,
            GroupPerimeters,
            (settings) => settings.InteriorSolidRegionShells,
            (settings, val) => settings.InteriorSolidRegionShells = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingBool<SingleMaterialFFFSettings> OuterShellLast = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.OuterShellLast_Name,
            () => UserSettingTranslations.OuterShellLast_Description,
            GroupPerimeters,
            (settings) => settings.OuterShellLast,
            (settings, val) => settings.OuterShellLast = val);

        public UserSettingDouble<SingleMaterialFFFSettings> SelfOverlapToleranceX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SelfOverlapToleranceX_Name,
            () => UserSettingTranslations.SelfOverlapToleranceX_Description,
            GroupPerimeters,
            (settings) => settings.SelfOverlapToleranceX,
            (settings, val) => settings.SelfOverlapToleranceX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> ShellsFillNozzleDiamStepX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ShellsFillNozzleDiamStepX_Name,
            () => UserSettingTranslations.ShellsFillNozzleDiamStepX_Description,
            GroupPerimeters,
            (settings) => settings.ShellsFillNozzleDiamStepX,
            (settings, val) => settings.ShellsFillNozzleDiamStepX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region PrintVolume

        public static readonly UserSettingGroup GroupPrintVolume =
           new UserSettingGroup(() => UserSettingTranslations.GroupPrintVolume);

        public UserSettingDouble<SingleMaterialFFFSettings> BedSizeXMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.BedSizeXMM_Name,
            () => UserSettingTranslations.BedSizeXMM_Description,
            GroupPrintVolume,
            (settings) => settings.Machine.BedSizeXMM,
            (settings, val) => settings.Machine.BedSizeXMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> BedSizeYMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.BedSizeYMM_Name,
            () => UserSettingTranslations.BedSizeYMM_Description,
            GroupPrintVolume,
            (settings) => settings.Machine.BedSizeYMM,
            (settings, val) => settings.Machine.BedSizeYMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> MaxHeightMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxHeightMM_Name,
            () => UserSettingTranslations.MaxHeightMM_Description,
            GroupPrintVolume,
            (settings) => settings.Machine.MaxHeightMM,
            (settings, val) => settings.Machine.MaxHeightMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Retraction

        public static readonly UserSettingGroup GroupRetraction =
            new UserSettingGroup(() => UserSettingTranslations.GroupRetraction);

        public UserSettingDouble<SingleMaterialFFFSettings> MinRetractTravelLength = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MinRetractTravelLength_Name,
            () => UserSettingTranslations.MinRetractTravelLength_Description,
            GroupRetraction,
            (settings) => settings.MinRetractTravelLength,
            (settings, val) => settings.MinRetractTravelLength = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> RetractDistanceMM = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.RetractDistanceMM_Name,
            () => UserSettingTranslations.RetractDistanceMM_Description,
            GroupRetraction,
            (settings) => settings.RetractDistanceMM,
            (settings, val) => settings.RetractDistanceMM = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> RetractSpeed = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.RetractSpeed_Name,
            () => UserSettingTranslations.RetractSpeed_Description,
            GroupRetraction,
            (settings) => settings.RetractSpeed,
            (settings, val) => settings.RetractSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region SolidFill

        public static readonly UserSettingGroup GroupSolidFill =
            new UserSettingGroup(() => UserSettingTranslations.GroupSolidFill);

        public UserSettingDouble<SingleMaterialFFFSettings> SolidFillBorderOverlapX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SolidFillBorderOverlapX_Name,
            () => UserSettingTranslations.SolidFillBorderOverlapX_Description,
            GroupSolidFill,
            (settings) => settings.SolidFillBorderOverlapX,
            (settings, val) => settings.SolidFillBorderOverlapX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SolidFillNozzleDiamStepX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SolidFillNozzleDiamStepX_Name,
            () => UserSettingTranslations.SolidFillNozzleDiamStepX_Description,
            GroupSolidFill,
            (settings) => settings.SolidFillNozzleDiamStepX,
            (settings, val) => settings.SolidFillNozzleDiamStepX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region SparseFill

        public static readonly UserSettingGroup GroupSparseFill =
            new UserSettingGroup(() => UserSettingTranslations.GroupSparseFill);

        public UserSettingDouble<SingleMaterialFFFSettings> SparseFillBorderOverlapX = new UserSettingDouble<SingleMaterialFFFSettings>(
           () => UserSettingTranslations.SparseFillBorderOverlapX_Name,
           () => UserSettingTranslations.SparseFillBorderOverlapX_Description,
           GroupSparseFill,
           (settings) => settings.SparseFillBorderOverlapX,
           (settings, val) => settings.SparseFillBorderOverlapX = val,
           UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SparseLinearInfillStepX = new UserSettingDouble<SingleMaterialFFFSettings>(
           () => UserSettingTranslations.SparseLinearInfillStepX_Name,
           () => UserSettingTranslations.SparseLinearInfillStepX_Description,
           GroupSparseFill,
           (settings) => settings.SparseLinearInfillStepX,
           (settings, val) => settings.SparseLinearInfillStepX = val,
           UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Speeds

        public static readonly UserSettingGroup GroupSpeeds =
            new UserSettingGroup(() => UserSettingTranslations.GroupSpeeds);

        public UserSettingInt<SingleMaterialFFFSettings> MaxExtrudeSpeedMMM = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxExtrudeSpeedMMM_Name,
            () => UserSettingTranslations.MaxExtrudeSpeedMMM_Description,
            GroupSpeeds,
            (settings) => settings.Machine.MaxExtrudeSpeedMMM,
            (settings, val) => settings.Machine.MaxExtrudeSpeedMMM = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> MaxRetractSpeedMMM = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxRetractSpeedMMM_Name,
            () => UserSettingTranslations.MaxRetractSpeedMMM_Description,
            GroupSpeeds,
            (settings) => settings.Machine.MaxRetractSpeedMMM,
            (settings, val) => settings.Machine.MaxRetractSpeedMMM = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> MaxTravelSpeedMMM = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxTravelSpeedMMM_Name,
            () => UserSettingTranslations.MaxTravelSpeedMMM_Description,
            GroupSpeeds,
            (settings) => settings.Machine.MaxTravelSpeedMMM,
            (settings, val) => settings.Machine.MaxTravelSpeedMMM = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> MaxZTravelSpeedMMM = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.MaxZTravelSpeedMMM_Name,
            () => UserSettingTranslations.MaxZTravelSpeedMMM_Description,
            GroupSpeeds,
            (settings) => settings.Machine.MaxZTravelSpeedMMM,
            (settings, val) => settings.Machine.MaxZTravelSpeedMMM = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> OuterPerimeterSpeedX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.OuterPerimeterSpeedX_Name,
            () => UserSettingTranslations.OuterPerimeterSpeedX_Description,
            GroupSpeeds,
            (settings) => settings.OuterPerimeterSpeedX,
            (settings, val) => settings.OuterPerimeterSpeedX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> RapidExtrudeSpeed = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.RapidExtrudeSpeed_Name,
            () => UserSettingTranslations.RapidExtrudeSpeed_Description,
            GroupSpeeds,
            (settings) => settings.RapidExtrudeSpeed,
            (settings, val) => settings.RapidExtrudeSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> RapidTravelSpeed = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.RapidTravelSpeed_Name,
            () => UserSettingTranslations.RapidTravelSpeed_Description,
            GroupSpeeds,
            (settings) => settings.RapidTravelSpeed,
            (settings, val) => settings.RapidTravelSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> ZTravelSpeed = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.ZTravelSpeed_Name,
            () => UserSettingTranslations.ZTravelSpeed_Description,
            GroupSpeeds,
            (settings) => settings.ZTravelSpeed,
            (settings, val) => settings.ZTravelSpeed = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion
        #region Support

        public static readonly UserSettingGroup GroupSupport =
            new UserSettingGroup(() => UserSettingTranslations.GroupSupport);

        public UserSettingBool<SingleMaterialFFFSettings> EnableSupportReleaseOpt = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.EnableSupportReleaseOpt_Name,
            () => UserSettingTranslations.EnableSupportReleaseOpt_Description,
            GroupSupport,
            (settings) => settings.EnableSupportReleaseOpt,
            (settings, val) => settings.EnableSupportReleaseOpt = val);

        public UserSettingBool<SingleMaterialFFFSettings> EnableSupportShell = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.EnableSupportShell_Name,
            () => UserSettingTranslations.EnableSupportShell_Description,
            GroupSupport,
            (settings) => settings.EnableSupportShell,
            (settings, val) => settings.EnableSupportShell = val);

        public UserSettingDouble<SingleMaterialFFFSettings> SupportAreaOffsetX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportAreaOffsetX_Name,
            () => UserSettingTranslations.SupportAreaOffsetX_Description,
            GroupSupport,
            (settings) => settings.SupportAreaOffsetX,
            (settings, val) => settings.SupportAreaOffsetX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingBool<SingleMaterialFFFSettings> SupportMinZTips = new UserSettingBool<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportMinZTips_Name,
            () => UserSettingTranslations.SupportMinZTips_Description,
            GroupSupport,
            (settings) => settings.SupportMinZTips,
            (settings, val) => settings.SupportMinZTips = val);

        public UserSettingDouble<SingleMaterialFFFSettings> SupportOverhangAngleDeg = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportOverhangAngleDeg_Name,
            () => UserSettingTranslations.SupportOverhangAngleDeg_Description,
            GroupSupport,
            (settings) => settings.SupportOverhangAngleDeg,
            (settings, val) => settings.SupportOverhangAngleDeg = val,
            UserSettingNumericValidations<double>.ValidateMinMax(0, 90, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SupportPointDiam = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportPointDiam_Name,
            () => UserSettingTranslations.SupportPointDiam_Description,
            GroupSupport,
            (settings) => settings.SupportPointDiam,
            (settings, val) => settings.SupportPointDiam = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingInt<SingleMaterialFFFSettings> SupportPointSides = new UserSettingInt<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportPointSides_Name,
            () => UserSettingTranslations.SupportPointSides_Description,
            GroupSupport,
            (settings) => settings.SupportPointSides,
            (settings, val) => settings.SupportPointSides = val,
            UserSettingNumericValidations<int>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SupportRegionJoinTolX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportRegionJoinTolX_Name,
            () => UserSettingTranslations.SupportRegionJoinTolX_Description,
            GroupSupport,
            (settings) => settings.SupportRegionJoinTolX,
            (settings, val) => settings.SupportRegionJoinTolX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SupportReleaseGap = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportReleaseGap_Name,
            () => UserSettingTranslations.SupportReleaseGap_Description,
            GroupSupport,
            (settings) => settings.SupportReleaseGap,
            (settings, val) => settings.SupportReleaseGap = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SupportSolidSpace = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportSolidSpace_Name,
            () => UserSettingTranslations.SupportSolidSpace_Description,
            GroupSupport,
            (settings) => settings.SupportSolidSpace,
            (settings, val) => settings.SupportSolidSpace = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SupportSpacingStepX = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportSpacingStepX_Name,
            () => UserSettingTranslations.SupportSpacingStepX_Description,
            GroupSupport,
            (settings) => settings.SupportSpacingStepX,
            (settings, val) => settings.SupportSpacingStepX = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        public UserSettingDouble<SingleMaterialFFFSettings> SupportVolumeScale = new UserSettingDouble<SingleMaterialFFFSettings>(
            () => UserSettingTranslations.SupportVolumeScale_Name,
            () => UserSettingTranslations.SupportVolumeScale_Description,
            GroupSupport,
            (settings) => settings.SupportVolumeScale,
            (settings, val) => settings.SupportVolumeScale = val,
            UserSettingNumericValidations<double>.ValidateMin(0, ValidationResult.Level.Error));

        #endregion

        /// <summary>
        /// Sets the culture for name & description strings.
        /// </summary>
        /// <remarks>
        /// Any translation resources used in derived classes should override to set the culture 
        /// on the resource manager, while still calling the base method.
        /// </remarks>
        /// <param name="cultureInfo"></param>
        public override void SetCulture(CultureInfo cultureInfo) {
            UserSettingTranslations.Culture = cultureInfo;
        }
    }
}
