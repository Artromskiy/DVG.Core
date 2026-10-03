namespace DVG
{
    public static class Logger
    {
        private sealed class NullLogger : ILogger
        {
            public void Info(string message, string? method, object? context) { }

            public void Warn(string message, string? method, object? context) { }

            public void Assert(bool condition, string? method, object? context) { }

            public void Error(System.Exception exception, string? method, object? context) { }

            public void Throw(System.Exception exception, string? method, object? context) { }
        }

        private static readonly ILogger _nullLogger = new NullLogger();
        private static ILogger? _instance;

        public static ILogger Instance
        {
            get => _instance ?? _nullLogger;
            set => _instance = value;
        }
    }
}
