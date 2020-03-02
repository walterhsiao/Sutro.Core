using gs;
using Sutro.PathWorks.Plugins.API;
using System.Collections.Generic;

namespace gsGCode.builders.mockClasses
{
    public class MockIGCodeAccumulator : IGCodeAccumulator
    {
        public List<GCodeLine> Lines { get; }

        public MockIGCodeAccumulator() => Lines = new List<GCodeLine>();

        public void AddLine(GCodeLine line)
        {
            Lines.Add(line);
        }
    }
}