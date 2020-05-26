using System;
using System.Collections.Generic;
using System.Text;

namespace gs
{
    public class Env
    {
#if DEBUG
        public static readonly bool Debugging = true;
#else
        public static readonly bool Debugging = false;
#endif
    }
}
