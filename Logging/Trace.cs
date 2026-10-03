using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace DVG
{
    public static class Trace
    {
        [Conditional("TRACE")]
        public static void Info(string message, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Info(message, method, filePath, context);

        [Conditional("TRACE")]
        public static void Warn(string message, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Warn(message, method, filePath, context);

        [Conditional("TRACE")]
        public static void Assert(bool condition, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Assert(condition, method, filePath, context);

        [Conditional("TRACE")]
        public static void Error(Exception exception, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
            => Logger.Instance.Error(exception, method, filePath, context);

        [Conditional("TRACE")]
        public static void Throw(Exception exception, [CallerMemberName] string? method = null, [CallerFilePath] string? filePath = null, object? context = null)
        {
            Logger.Instance.Throw(exception, method, filePath, context);
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
