namespace gs
{
    /// <summary>
    /// Things that are common to FillPolylineGeneric and FillPolylineGeneric
    /// </summary>
    public interface IFillElement
    {
        double CustomThickness { get; set; }

        IFillType FillType { get; set; }
    }
}