namespace gs
{
    public class SupportFillType : DefaultFillType
    {
        new public static string Label => "support";
        new public static int Flag => 1 << 10;

        public override string GetLabel()
        {
            return Label;
        }

        private readonly SingleMaterialFFFSettings settings;

        public SupportFillType(SingleMaterialFFFSettings settings)
        {
            this.settings = settings;
        }

        public override double AdjustVolume(double volume)
        {
            return volume * settings.SupportVolumeScale;
        }

        public override double ModifySpeed(double speed, SchedulerSpeedHint speedHint = SchedulerSpeedHint.Default)
        {
            if (speedHint == SchedulerSpeedHint.Careful)
                return speed * settings.OuterPerimeterSpeedX;
            else
                return speed;
        }

        public bool IsPart()
        {
            return false;
        }
    }
}