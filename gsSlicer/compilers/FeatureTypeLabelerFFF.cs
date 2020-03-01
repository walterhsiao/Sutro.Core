using System;

namespace gs
{
    public static class FeatureTypeLabelerFFF
    {
        private static FeatureTypeLabeler singleton = new FeatureTypeLabeler(new Tuple<FillTypeFlags, string>[]
            {
            });

        public static FeatureTypeLabeler Value { get => singleton; }
    }
}