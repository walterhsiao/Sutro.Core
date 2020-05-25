namespace gs.FillTypes
{
    public class SolidFillType : BaseFillType
    {
        public SolidFillType(double volumeScale, double speedScale) : base(volumeScale, speedScale)
        {
        }

        public static string Label => "solid layer";

        public override string GetLabel()
        {
            return Label;
        }
    }
}