namespace gs.FillTypes
{
    public class SolidFillType : BaseFillType
    {
        public static string Label => "solid layer";
        public static int Flag => 1 << 8;

        public override string GetLabel()
        {
            return Label;
        }
    }
}