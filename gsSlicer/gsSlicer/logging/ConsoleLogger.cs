using System;

namespace gs
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}