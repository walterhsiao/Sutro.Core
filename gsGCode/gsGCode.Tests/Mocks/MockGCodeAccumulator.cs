using gs;
using System.Collections.Generic;

namespace gsGCode.Tests.Mocks
{
    public class MockGCodeAccumulator : IGCodeAccumulator
    {
        public List<GCodeLine> Lines { get; }

        public MockGCodeAccumulator() => Lines = new List<GCodeLine>();

        public void AddLine(GCodeLine line)
        {
            Lines.Add(line);
        }
    }
}