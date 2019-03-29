using gs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gsCore.UnitTests
{
    [TestClass]
    public class AdditiveSettingsTests
    {
        [TestMethod]
        public void CloneAs_ValuesCloneCorrectly()
        {
            // arrange
            var orig = new SingleMaterialFFFSettings();
            orig.Shells = 10;
            orig.Machine.NozzleDiamMM = 20;
            orig.Machine.ManufacturerName = "A";

            // act
            SingleMaterialFFFSettings copy = orig.CloneAs<SingleMaterialFFFSettings>();

            // assert
            Assert.AreEqual(10, copy.Shells);
            Assert.AreEqual(20, copy.Machine.NozzleDiamMM);
            Assert.AreEqual("A", orig.Machine.ManufacturerName);
            Assert.AreNotSame(copy.Machine, orig.Machine);
        }

        [TestMethod]
        public void CloneAs_CloneValuesDoNotAffectOriginal()
        {
            // arrange
            var orig = new GenericRepRapSettings();
            orig.Shells = 10;
            orig.Machine.NozzleDiamMM = 20;
            orig.Machine.ManufacturerName = "A";

            // act
            SingleMaterialFFFSettings copy = orig.CloneAs<SingleMaterialFFFSettings>();
            copy.Shells *= 2;
            copy.Machine.NozzleDiamMM *= 20;
            copy.Machine.ManufacturerName = "B";
                
            // assert
            Assert.AreEqual(10, orig.Shells);
            Assert.AreEqual(20, orig.Machine.NozzleDiamMM);
            Assert.AreEqual("A", orig.Machine.ManufacturerName);
            Assert.AreNotSame(copy.Machine, orig.Machine);
        }
    }
}
