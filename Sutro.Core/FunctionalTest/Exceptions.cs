using System;

namespace Sutro.Core.FunctionalTest
{
    public class FeatureCumulativeExtrusionMismatchException : Exception
    {
        public FeatureCumulativeExtrusionMismatchException(string s) : base(s)
        {
        }
    }

    public class FeatureCumulativeDistanceMismatchException : Exception
    {
        public FeatureCumulativeDistanceMismatchException(string s) : base(s)
        {
        }
    }

    public class FeatureCumulativeDurationMismatchException : Exception
    {
        public FeatureCumulativeDurationMismatchException(string s) : base(s)
        {
        }
    }

    public class FeatureCenterOfMassMismatchException : Exception
    {
        public FeatureCenterOfMassMismatchException(string s) : base(s)
        {
        }
    }

    public class FeatureBoundingBoxMismatchException : Exception
    {
        public FeatureBoundingBoxMismatchException(string s) : base(s)
        {
        }
    }

    public class MissingFeatureException : Exception
    {
        public MissingFeatureException(string s) : base(s)
        {
        }
    }

    public class LayerCountMismatchException : Exception
    {
        public LayerCountMismatchException(string s) : base(s)
        {
        }
    }
}