using g3;

namespace gs
{
    public interface IFillLoop : IFillElement
    {
        Vector2d EntryExitPoint { get; }
        public bool IsHoleShell { get; set; }
        double Perimeter { get; }
        Vector2d this[int i] { get; }

        double DistanceSquared(Vector2d pt, out int iNearSeg, out double nearSegT);

        Segment2d GetSegment2dAfterVertex(int c);

        Segment2d GetSegment2dBeforeVertex(int c);

        void Roll(int index);

        void TrimEnd(double d);
    }
}