using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MySQLToCsharp.Tests.Helper
{
    public class TraceLoggerProvider : ILoggerProvider
    {
        readonly TraceLogger loggerDefault;

        public TraceLoggerProvider() => loggerDefault = new TraceLogger(LogLevel.Trace);
        public ILogger CreateLogger(string categoryName) => loggerDefault;
        public void Dispose() { }
    }
    public class TraceLogger : ILogger
    {
        public static Stack<(LogLevel logLevel, string msg)> Stack = new Stack<(LogLevel, string)>();
        readonly LogLevel minimumLogLevel;
        public TraceLogger(LogLevel minimumLogLevel)
        {
            Stack.Clear();
            this.minimumLogLevel = minimumLogLevel;
        }
        public IDisposable BeginScope<TState>(TState state) => NullDisposable.Instance;
        public bool IsEnabled(LogLevel logLevel) => minimumLogLevel <= logLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            if (minimumLogLevel > logLevel) return;
            var msg = formatter(state, exception);

            if (!string.IsNullOrEmpty(msg))
            {
                Stack.Push((logLevel, msg));
            }

            if (exception != null)
            {
                Stack.Push((logLevel, msg));
            }
        }

        class NullDisposable : IDisposable
        {
            public static readonly IDisposable Instance = new NullDisposable();
            public void Dispose()
            {
                return;
            }
        }
    }
    public static class TraceLoggerExtensions
    {
        /// <summary>
        /// use ConsoleAppFramework.Logging.SimpleConsoleLogger.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddTraceLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TraceLoggerProvider>());
            return builder;
        }

        /// <summary>
        /// Remove default ConsoleLoggerProvider and replace to SimpleConsoleLogger.
        /// </summary>
        public static ILoggingBuilder ReplaceToTraceLogger(this ILoggingBuilder builder)
        {
            // Use SimpleConsoleLogger instead of the default ConsoleLogger.
            var consoleLogger = builder.Services.FirstOrDefault(x => x.ImplementationType?.FullName == "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider");
            if (consoleLogger != null)
            {
                builder.Services.Remove(consoleLogger);
            }

            builder.AddTraceLogger();
            return builder;
        }
    }
}
