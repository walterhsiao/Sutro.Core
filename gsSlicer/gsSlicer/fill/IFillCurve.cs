using g3;

namespace gs
{
    public interface IFillCurve : IFillElement
    {
        double ArcLength { get; }
        Vector2d End { get; }
        Vector2d Start { get; }

        void Reverse();

        Vector2d this[int i] { get; }
    }
}