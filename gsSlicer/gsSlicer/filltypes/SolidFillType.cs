namespace gs.FillTypes
{
    public class SolidFillType : BaseFillType
    {
        private double solidFillSpeedX;

        public SolidFillType(double solidFillSpeedX)
        {
            this.solidFillSpeedX = solidFillSpeedX;
        }

        public static string Label => "solid layer";

        public override string GetLabel()
        {
            return Label;
        }

        public override double ModifySpeed(double speed, SchedulerSpeedHint speedHint)
        {
            return speed * solidFillSpeedX;
        }
    }
}