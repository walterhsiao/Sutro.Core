namespace gs
{
    public interface IFillType
    {
        double ModifySpeed(double useSpeed, SchedulerSpeedHint speedHint);

        double AdjustVolume(double vol_scale);

        static string Label { get; }

        string GetLabel();

        bool IsPart()
        {
            return true;
        }

        static int Flag { get; } = 0;
    }
}