using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces
{
    public class UserSettingGroup
    {
        private readonly Func<string> NameF;

        public string Name => NameF();

        public UserSettingGroup(Func<string> nameF, Func<string> descriptionF = null)
        {
            NameF = nameF;
        }


    }
}
