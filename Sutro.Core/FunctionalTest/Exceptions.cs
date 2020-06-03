using System;

namespace Sutro.Core.FunctionalTest.FeatureMismatchExceptions
{
    public class CumulativeExtrusionException : Exception
    {
        public CumulativeExtrusionException(string s) : base(s)
        {
        }
    }

    public class CumulativeDistanceException : Exception
    {
        public CumulativeDistanceException(string s) : base(s)
        {
        }
    }

    public class CumulativeDurationException : Exception
    {
        public CumulativeDurationException(string s) : base(s)
        {
        }
    }

    public class CenterOfMassException : Exception
    {
        public CenterOfMassException(string s) : base(s)
        {
        }
    }

    public class BoundingBoxException : Exception
    {
        public BoundingBoxException(string s) : base(s)
        {
        }
    }

    public class MissingFeatureException : Exception
    {
        public MissingFeatureException(string s) : base(s)
        {
        }
    }

    public class LayerCountException : Exception
    {
        public LayerCountException(string s) : base(s)
        {
        }
    }
}