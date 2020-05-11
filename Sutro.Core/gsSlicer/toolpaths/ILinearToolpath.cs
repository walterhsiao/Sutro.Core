using g3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gs
{

    public interface ILinearToolpath<T> : IToolpath, IEnumerable<T>
    {
        T this[int key] { get; }
    }
}