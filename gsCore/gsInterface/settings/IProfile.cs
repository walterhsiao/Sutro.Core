using System;
using System.Collections.Generic;
using System.Text;

namespace gs.interfaces
{
    public interface IProfile
    {
        string ManufacturerName { get; }
        string ModelIdentifier { get; }
        string ProfileName { get; }
    }
}
