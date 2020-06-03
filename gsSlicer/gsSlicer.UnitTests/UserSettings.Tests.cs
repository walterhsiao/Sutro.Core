using gs.engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace gs.UnitTests
{
    [TestClass]
    public class UserSettingsTests
    {
        private static void TestUserSettings(PrintUserSettingsFFF<SingleMaterialFFFSettings> userSettings)
        {
            var rawSettings = new GenericRepRapSettings();

            foreach (var setting in userSettings.Settings())
            {
                _ = setting.Name;
                _ = setting.Description;
            }

            userSettings.LoadFromRaw(rawSettings, userSettings.Settings());
            userSettings.ApplyToRaw(rawSettings, userSettings.Settings());
        }

        [TestMethod]
        public void CanConstructPrintUserSettingsFFF()
        {
            var userSettings = new PrintUserSettingsFFF<SingleMaterialFFFSettings>();

            TestUserSettings(userSettings);
        }

        [TestMethod]
        public void CanConstructMachineUserSettingsFFF()
        {
            var settings = new MachineUserSettingsFFF<SingleMaterialFFFSettings>();
            foreach (var setting in settings.Settings())
            {
            }
        }

        [TestMethod]
        public void CanConstructMaterialUserSettingsFFF()
        {
            var settings = new MaterialUserSettingsFFF<SingleMaterialFFFSettings>();
            foreach (var setting in settings.Settings())
            {
            }
        }
    }
}