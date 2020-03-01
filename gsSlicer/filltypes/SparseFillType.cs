namespace gs.FillTypes
{
    public class SparseFillType : BaseFillType
    {
        public static string Label => "infill";
        public static int Flag => 1 << 9;

        public override string GetLabel()
        {
            return Label;
        }
    }
}