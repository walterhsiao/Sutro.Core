namespace gs.FillTypes
{

    public class OpenShellCurveFillType : BaseFillType
    {
        public static string Label => "solid layer";
        public static int Flag => 1 << 3;

        public override string GetLabel()
        {
            return Label;
        }
    }
}