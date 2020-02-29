namespace gs
{
    public class BridgeFillType : DefaultFillType
    {
        private readonly SingleMaterialFFFSettings settings;

        public BridgeFillType(SingleMaterialFFFSettings settings)
        {
            this.settings = settings;
        }

        new public static string Label => "bridge";

        public override string GetLabel()
        {
            return Label;
        }

        new public static int Flag => 1 << 11;

        public override double AdjustVolume(double volume)
        {
            return volume * settings.BridgeVolumeScale;
        }

        public override double ModifySpeed(double speed)
        {
            return settings.CarefulExtrudeSpeed * settings.BridgeExtrudeSpeedX;
        }
    }
}