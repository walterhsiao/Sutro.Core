namespace gs.FillTypes
{
    public class DefaultFillType : BaseFillType
    {
        public static string Label => "unknown";

        public static int Flag => 0;

        public override string GetLabel()
        {
            return Label;
        }
    }
}