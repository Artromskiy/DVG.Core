using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace DVG
{
    public static class Debug
    {
        [Conditional("DEBUG")]
        public static void Info(string message, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Info(message, method, filePath, context);

        [Conditional("DEBUG")]
        public static void Warn(string message, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Warn(message, method, filePath, context);

        [Conditional("DEBUG")]
        public static void Assert(bool condition, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Assert(condition, method, filePath, context);

        [Conditional("DEBUG")]
        public static void Error(Exception exception, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Error(exception, method, filePath, context);

        [Conditional("DEBUG")]
        public static void Throw(Exception exception, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
        {
            Logger.Instance.Throw(exception, method, filePath, context);
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
