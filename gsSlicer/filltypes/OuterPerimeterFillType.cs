namespace gs.FillTypes
{
    public class OuterPerimeterFillType : BaseFillType
    {
        public static string Label => "outer perimeter";
        public static int Flag => 1 | 1 << 1;

        public override string GetLabel()
        {
            return Label;
        }

        private double speedModifier;

        public OuterPerimeterFillType(SingleMaterialFFFSettings settings)
        {
            speedModifier = settings.OuterPerimeterSpeedX;
        }

        public override double ModifySpeed(double speed, SchedulerSpeedHint speedHint)
        {
            return speedModifier * speed;
        }
    }
}