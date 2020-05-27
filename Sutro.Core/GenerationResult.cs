using Sutro.Core.Logging;
using Sutro.Core.Models.GCode;
using System;
using System.Collections.Generic;

namespace Sutro.Core
{
    public enum GenerationResultStatus
    {
        Success,
        Failure,
        Canceled
    }

    public class GenerationResult
    {
        public GCodeFile GCode { get; set; }
        public List<Tuple<LoggingLevel, string>> Log { get; } = new List<Tuple<LoggingLevel, string>>();
        public List<string> Report { get; } = new List<string>();
        public GenerationResultStatus Status { get; set; } = GenerationResultStatus.Failure;

        public void LogMessage(LoggingLevel level, string message)
        {
            Log.Add(Tuple.Create(level, message));
        }
    }
}