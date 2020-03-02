using Sutro.PathWorks.Plugins.API;

namespace gs
{
    public interface IGCodeAccumulator
    {
        void AddLine(GCodeLine line);
    }
}