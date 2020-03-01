namespace gs.FillTypes
{
    public class InnerPerimeterFillType : BaseFillType
    {
        public static string Label => "inner perimeter";
        public static int Flag => 1;

        public override string GetLabel()
        {
            return Label;
        }
    }
}