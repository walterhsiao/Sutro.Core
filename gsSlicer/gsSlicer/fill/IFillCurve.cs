using g3;
using System.Collections.Generic;

namespace gs
{
    public interface IFillCurve : IFillElement
    {
        double ArcLength { get; }
        Vector2d End { get; }
        Vector2d Start { get; }

        void Reverse();

        Vector2d this[int i] { get; }

        void TrimBack(double trimDistance);
        void TrimFront(double trimDistance);

        List<IFillCurve> SplitAtDistances(List<double> splitDistances);
    }
}