using System;
using System.Collections.Generic;
using System.Text;

namespace gs.utility
{
    public static class ToolpathSetExtensions
    {
        public static List<LinearToolpath3<T>> GetPaths<T>(this ToolpathSet toolpathSet) where T : IToolpathVertex
        {
            var paths = new List<LinearToolpath3<T>>();

            foreach (var path in toolpathSet)
            {
                if (path is LinearToolpath3<T> toolpath)
                    paths.Add(toolpath);
            }

            return paths;
        }
    }
}
