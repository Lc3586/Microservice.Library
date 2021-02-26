using Business.Interface.System;
using Microservice.Library.Container;
using Microservice.Library.Extension;
using Microservice.Library.NLogger.Gen;
using Model.Utils.Config;
using Model.Utils.Log;
using NLog;
using System;

namespace Business.Utils.Log
{
    public static class Logger
    {
        static readonly SystemConfig config = AutofacHelper.GetScopeService<SystemConfig>();

        static readonly ILogger nLogger = AutofacHelper.GetScopeService<INLoggerProvider>()
                                                    .GetNLogger(config.DefaultLoggerName);

        static readonly IOperator @operator = AutofacHelper.GetScopeService<IOperator>();

        public static void Log(LogLevel logLevel, byte logType, string message, string data, Exception exception = null, bool withOP = true)
        {
            LogEventInfo log = new LogEventInfo(
                LogLevel.FromString(logLevel.ToString()),
                nLogger.Name,
                message + (exception == null ? "" : $"\r\n\t{exception.GetExceptionAllMsg()}"))
            {
                Exception = exception
            };

            log.Properties[NLoggerConfig.LogType] = LogType.GetName(logType);
            log.Properties[NLoggerConfig.Data] = data;

            if (withOP)
            {
                log.Properties[NLoggerConfig.CreatorName] = @operator?.UserInfo?.Name;
                log.Properties[NLoggerConfig.CreatorId] = @operator?.AuthenticationInfo?.Id;
            }

            nLogger.Log(log);
        }

        public static void Log(LogLevel logLevel, byte logType, string msg)
        {
            Log(logLevel, logType, msg, null);
        }

        public static void Debug(byte logType, string msg)
        {
            Log(LogLevel.Debug, logType, msg);
        }

        public static void Debug(byte logType, string msg, string data)
        {
            Log(LogLevel.Debug, logType, msg, data);
        }

        public static void Error(byte logType, string msg)
        {
            Log(LogLevel.Error, logType, msg);
        }

        public static void Error(byte logType, string msg, string data)
        {
            Log(LogLevel.Error, logType, msg, data);
        }

        public static void Error(Exception ex)
        {
            Log(LogLevel.Error, LogType.系统异常, ex.GetExceptionAllMsg());
        }

        public static void Fatal(byte logType, string msg)
        {
            Log(LogLevel.Fatal, logType, msg);
        }

        public static void Fatal(byte logType, string msg, string data)
        {
            Log(LogLevel.Fatal, logType, msg, data);
        }

        public static void Info(byte logType, string msg)
        {
            Log(LogLevel.Info, logType, msg);
        }

        public static void Info(byte logType, string msg, string data)
        {
            Log(LogLevel.Info, logType, msg, data);
        }

        public static void Trace(byte logType, string msg)
        {
            Log(LogLevel.Trace, logType, msg);
        }

        public static void Trace(byte logType, string msg, string data)
        {
            Log(LogLevel.Trace, logType, msg, data);
        }

        public static void Warn(byte logType, string msg)
        {
            Log(LogLevel.Warn, logType, msg);
        }

        public static void Warn(byte logType, string msg, string data)
        {
            Log(LogLevel.Warn, logType, msg, data);
        }
    }
}
