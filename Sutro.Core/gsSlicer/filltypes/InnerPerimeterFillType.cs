namespace gs.FillTypes
{
    public class InnerPerimeterFillType : BaseFillType
    {
        private double speedModifier;

        public static string Label => "inner perimeter";

        public override string GetLabel()
        {
            return Label;
        }

        public InnerPerimeterFillType(SingleMaterialFFFSettings settings)
        {
            speedModifier = settings.InnerPerimeterSpeedX;
        }

        public override double ModifySpeed(double speed, SchedulerSpeedHint speedHint)
        {
            return speedModifier * speed;
        }

        public override bool IsPartShell()
        {
            return true;
        }
    }
}