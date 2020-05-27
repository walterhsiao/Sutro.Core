namespace Sutro.Core.CLI
{
    internal class Env
    {
        public static bool Debugging =>
#if DEBUG
            true;
#else
            false;
#endif
    }
}
