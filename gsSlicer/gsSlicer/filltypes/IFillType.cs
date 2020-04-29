namespace gs.FillTypes
{
    public interface IFillType
    {
        double ModifySpeed(double speed, SchedulerSpeedHint speedHint);

        double AdjustVolume(double volume);

        string GetLabel();

        bool IsEntryLocationSpecified();

        bool IsPart() { return true; }

        bool IsPartShell() { return false; }
    }
}