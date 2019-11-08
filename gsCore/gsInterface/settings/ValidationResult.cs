namespace gs.interfaces
{
    public struct ValidationResult
    {
        public ValidationResult(Level severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        public enum Level { None, Warning, Error }
        public Level Severity;
        public string Message;
    }
}
