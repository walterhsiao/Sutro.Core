namespace gs.FillTypes
{
    public class BridgeFillType : BaseFillType
    {
        private readonly double bridgeSpeed;

        public BridgeFillType(double volumeScale, double speed) : base(volumeScale)
        {
            bridgeSpeed = speed;
        }

        public static string Label => "bridge";

        public override string GetLabel()
        {
            return Label;
        }

        public override double ModifySpeed(double speed, SpeedHint speedHint)
        {
            return bridgeSpeed;
        }
    }
}