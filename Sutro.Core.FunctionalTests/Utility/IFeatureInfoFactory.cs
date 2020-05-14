using gs;
using gsCore.FunctionalTests.Models;
using Sutro.Core.Models.GCode;

namespace gsCore.FunctionalTests.Utility
{
    public interface IFeatureInfoFactory<out TFeatureInfo> where TFeatureInfo : IFeatureInfo
    {
        TFeatureInfo SwitchFeature(string featureType);

        void ObserveGcodeLine(GCodeLine line);

        void Initialize();
    }
}