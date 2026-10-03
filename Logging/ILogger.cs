using System;

namespace DVG
{
    public interface ILogger
    {
        void Info(string message, string? method = null, string? filePath = null, object? context = null);

        void Warn(string message, string? method = null, string? filePath = null, object? context = null);

        void Assert(bool condition, string? method = null, string? filePath = null, object? context = null);

        void Error(Exception exception, string? method = null, string? filePath = null, object? context = null);

        void Throw(Exception exception, string? method = null, string? filePath = null, object? context = null);
    }
}
