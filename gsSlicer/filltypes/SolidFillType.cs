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

    public class InteriorShellFillType : BaseFillType
    {
        public static string Label => DefaultFillType.Label;
        public static int Flag => 1 << 2;

        public override string GetLabel()
        {
            return Label;
        }
    }
}