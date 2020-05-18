namespace Sutro.Core.FunctionalTest
{
    public interface IResultGenerator
    {
        public void GenerateResultFile(string meshFilePath, string outputFilePath);
    }
}