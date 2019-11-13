using gs;
using gs.info;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gsCore.UnitTests
{
    internal class SettingsA : Settings
    {
        public int IntegerFieldA = 0;
        public int IntegerPropertyA { get; set;} = 0;
    }

    internal class SettingsB : SettingsA
    {
        public int IntegerFieldB = 0;
    }

    internal class SettingsC : SettingsA
    {
        public int IntegerFieldC = 0;
    }

    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void CopyFromSameType_Field_ValueType()
        {
            var orig = new SettingsA();
            var copy = new SettingsA();
            orig.IntegerFieldA = 3;
            
            copy.CopyValuesFrom(orig);

            Assert.AreEqual(3, copy.IntegerFieldA);
        }

        [TestMethod]
        public void CopyFromSameType_Property_ValueType()
        {
            var orig = new SettingsA();
            var copy = new SettingsA();
            orig.IntegerPropertyA = 5;

            copy.CopyValuesFrom(orig);

            Assert.AreEqual(5, copy.IntegerPropertyA);
        }

        [TestMethod]
        public void CopyFromChild_Field_ValueType()
        {
            var orig = new SettingsB();
            var copy = new SettingsA();
            orig.IntegerFieldA = 7;

            copy.CopyValuesFrom(orig);

            Assert.AreEqual(7, copy.IntegerFieldA);
        }

        [TestMethod]
        public void CopyFromParent_Field_ValueType()
        {
            var orig = new SettingsA();
            var copy = new SettingsB();
            orig.IntegerFieldA = 8;

            copy.CopyValuesFrom(orig);

            Assert.AreEqual(8, copy.IntegerFieldA);
        }

        [TestMethod]
        public void CopyFromSibling_Field_ValueType()
        {
            var orig = new SettingsB();
            var copy = new SettingsC();
            orig.IntegerFieldA = 9;

            copy.CopyValuesFrom(orig);

            Assert.AreEqual(9, copy.IntegerFieldA);
        }

    }
}
