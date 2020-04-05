namespace gs
{
    public interface ILogger
    {
        void Write(string s);

        void WriteLine(string s);

        void WriteLine();

        void WriteLine(object o)
        {
            WriteLine(o.ToString());
        }
    }
}