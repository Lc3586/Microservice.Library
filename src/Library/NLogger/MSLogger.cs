using Library.NLogger.Gen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Library.NLogger
{
    /// <summary>
    /// MS日志
    /// </summary>
    public class MSLogger : ILogger
    {
        public MSLogger(INLoggerProvider nLoggerProvider, string categoryName)
        {
            NLoggerProvider = nLoggerProvider;
            CategoryName = categoryName;
        }

        readonly INLoggerProvider NLoggerProvider;

        readonly string CategoryName;

        NLog.ILogger NLogger;

        NLog.ILogger GetNLogger()
        {
            if (NLogger == null)
                NLogger = NLoggerProvider.GetNLogger(CategoryName);

            return NLogger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return GetNLogger().IsEnabled(NLog.LogLevel.FromOrdinal((int)logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log = new NLog.LogEventInfo(
                NLog.LogLevel.FromString(logLevel.ToString()),
                GetNLogger().Name,
                formatter(state, exception));
            log.Properties.Add("Microsoft.Extensions.Logging.LogLevel", logLevel);
            log.Exception = exception;
            GetNLogger().Log(log);
        }
    }
}
