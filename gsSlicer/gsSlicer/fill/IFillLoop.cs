namespace gs
{
    public interface IFillLoop : IFillElement
    {
        double Perimeter { get; }

        public bool IsHoleShell { get; set; }
    }

}