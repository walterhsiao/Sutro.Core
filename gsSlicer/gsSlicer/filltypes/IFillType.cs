namespace gs
{
    public interface IFillType
    {
        double ModifySpeed(double speed, SchedulerSpeedHint speedHint);

        double AdjustVolume(double volume);

        static string Label { get; }

        string GetLabel();

        bool IsEntryLocationSpecified();

        bool IsPart() { return true; }

        bool IsPartShell() { return false; }
    }
}