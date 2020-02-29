namespace gs
{
    public class DefaultFillType : IFillType
    {
        public static string Label => "unknown";
        public static int Flag => 0;

        public virtual double AdjustVolume(double vol_scale)
        {
            return vol_scale;
        }

        public virtual string GetLabel()
        {
            return Label;
        }

        public virtual double ModifySpeed(double speed, SchedulerSpeedHint speedHint = SchedulerSpeedHint.Default)
        {
            return speed;
        }
    }
}