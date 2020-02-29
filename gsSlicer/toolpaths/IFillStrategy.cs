namespace gs
{
    public interface IFillType
    {
        double ModifySpeed(double useSpeed);

        double AdjustVolume(double vol_scale);

        static string Label { get; }

        string GetLabel();

        static int Flag { get; } = 0;
    }
}