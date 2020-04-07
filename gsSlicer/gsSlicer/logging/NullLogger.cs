using System;

namespace gs
{
    public class NullLogger : ILogger
    {
        public void Write(string s, ConsoleColor? color = null)
        {
        }

        public void WriteLine()
        {
        }

        public void WriteLine(string s, ConsoleColor? color = null)
        {
        }
    }
}