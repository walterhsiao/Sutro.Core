using Sutro.Core.Models.GCode;

namespace gs
{
    public interface IGCodeAccumulator
    {
        void AddLine(GCodeLine line);
    }
}