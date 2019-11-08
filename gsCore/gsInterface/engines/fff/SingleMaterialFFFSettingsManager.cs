using System.Collections.Generic;
using gs.info;
using gs.interfaces;

namespace gs
{
    public class SingleMaterialFFFSettingsManager : SettingsManager<SingleMaterialFFFSettings>
    {
        public override List<SingleMaterialFFFSettings> FactorySettings => 
            new List<SingleMaterialFFFSettings>()
            {
                new GenericRepRapSettings(),
                new MakerbotSettings()
            };

        public override IUserSettingCollection<SingleMaterialFFFSettings> UserSettings =>
            new UserSettingsSingleMaterialFFF();
    }
}
