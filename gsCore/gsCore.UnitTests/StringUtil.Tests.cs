using g3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ribbon.Lib;

namespace gsCore.UnitTests
{
    [TestClass]
    public class StringUtilTests
    {
        [TestMethod]
        public void FormatSettingOverride_Flat()
        {
            var actual = StringUtil.FormatSettingOverride("SettingName:SettingValue");
            var expected = "{\"SettingName\":SettingValue}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FormatSettingOverride_OneDeep()
        {
            var actual = StringUtil.FormatSettingOverride("Subsetting.SettingName:SettingValue");
            var expected = "{\"Subsetting\":{\"SettingName\":SettingValue}}";

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void FormatSettingOverride_ThreeDeep()
        {
            var actual = StringUtil.FormatSettingOverride("a.b.c:SettingValue");
            var expected = "{\"a\":{\"b\":{\"c\":SettingValue}}}";

            Assert.AreEqual(expected, actual);
        }
    }
}