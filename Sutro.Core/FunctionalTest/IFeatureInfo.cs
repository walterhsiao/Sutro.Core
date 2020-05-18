namespace Sutro.Core.FunctionalTest
{
    public interface IFeatureInfo
    {
        string FillType { get; set; }

        void Add(IFeatureInfo other);

        void AssertEqualsExpected(IFeatureInfo other);
    }
}