namespace Sutro.Core.FunctionalTest
{
    public interface IResultAnalyzer
    {
        void CompareResults(string pathExpected, string pathActual);
    }
}