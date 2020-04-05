namespace gs
{
    public class NullLogger : ILogger
    {
        public void Write(string s)
        {
        }

        public void WriteLine(string s)
        {
        }

        public void WriteLine()
        {
        }
    }
}