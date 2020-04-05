using gs;
using gsCore.FunctionalTests.Models;
using gsCore.FunctionalTests.Utility;

namespace gsSlicer.FunctionalTests.Utility
{
    public static class TestRunnerFactoryFFF
    {
        public static PrintTestRunner CreateTestRunner(string caseName, SingleMaterialFFFSettings settings)
        {
            var resultGenerator = CreateResultGenerator(settings);
            var resultAnalyzer = new ResultAnalyzer<FeatureInfo>(new FeatureInfoFactoryFFF(), new ConsoleLogger());
            return new PrintTestRunner(caseName, resultGenerator, resultAnalyzer);
        }

        public static PrintGeneratorManager<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings> CreateResultGenerator(SingleMaterialFFFSettings settings)
        {
            return new PrintGeneratorManager<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings>()
                new PrintGeneratorManager<SingleMaterialFFFPrintGenerator, SingleMaterialFFFSettings>(), settings, new ConsoleLogger());
        }
    }
}