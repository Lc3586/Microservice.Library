using Integrate_Business.Config;
using Library.Container;
using Library.Extension;
using Library.Models;
using Library.Log;
using System;
using System.IO;
using System.Text;

namespace Integrate_Business
{
    public class Logger : ILogger, IDependency
    {
        /// <summary>
        /// 配置Logger
        /// </summary>
        static Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            string layout = LoggerConfig.Layout;

            //控制台
            if (SystemConfig.systemConfig.DefaultLoggerType.HasFlag(LoggerType.Console))
            {
                AddTarget(new NLog.Targets.ColoredConsoleTarget
                {
                    Name = LoggerConfig.LoggerName,
                    Layout = layout
                });
            }

            //文件
            if (SystemConfig.systemConfig.DefaultLoggerType.HasFlag(LoggerType.File))
            {
                AddTarget(new NLog.Targets.FileTarget
                {
                    Name = LoggerConfig.LoggerName,
                    Layout = layout,
                    FileName = Path.Combine(Directory.GetCurrentDirectory(), "logs", "${date:format=yyyy-MM-dd}.txt"),
                    Encoding = Encoding.UTF8
                });
            }

            //数据库
            if (SystemConfig.systemConfig.DefaultLoggerType.HasFlag(LoggerType.RDBMS))
            {
                AddTarget(new RDBMSTarget
                {
                    Layout = layout
                });
            }

            //ElasticSearch
            if (SystemConfig.systemConfig.DefaultLoggerType.HasFlag(LoggerType.ElasticSearch))
            {
                AddTarget(new ElasticSearchTarget
                {
                    Layout = layout
                });
            }

            NLog.LogManager.Configuration = config;
            void AddTarget(NLog.Targets.Target target)
            {
                config.AddTarget(target);
                config.AddRuleForAllLevels(target);
            }
        }
        private IOperator _operator { get => AutofacHelper.GetScopeService<IOperator>(); }

        public void Log(LogLevel logLevel, LogType logType, string msg, string data)
        {
            NLog.Logger _nLogger = NLog.LogManager.GetLogger("sysLogger");

            NLog.LogEventInfo log = new NLog.LogEventInfo(NLog.LogLevel.FromString(logLevel.ToString()), "sysLogger", msg);
            log.Properties[LoggerConfig.Data] = data;
            log.Properties[LoggerConfig.LogType] = logType.ToString();
            try
            {
                log.Properties[LoggerConfig.CreatorRealName] = _operator?.Property?.UserName;
                log.Properties[LoggerConfig.CreatorId] = _operator?.UserId;
            }
            catch
            {

            }

            _nLogger.Log(log);
        }

        public void Log(LogLevel logLevel, LogType logType, string msg)
        {
            Log(logLevel, logType, msg, null);
        }

        public void Debug(LogType logType, string msg)
        {
            Log(LogLevel.Debug, logType, msg);
        }

        public void Debug(LogType logType, string msg, string data)
        {
            Log(LogLevel.Debug, logType, msg, data);
        }

        public void Error(LogType logType, string msg)
        {
            Log(LogLevel.Error, logType, msg);
        }

        public void Error(LogType logType, string msg, string data)
        {
            Log(LogLevel.Error, logType, msg, data);
        }

        public void Error(Exception ex)
        {
            Log(LogLevel.Error, LogType.系统异常, ex.GetExceptionAllMsg());
        }

        public void Fatal(LogType logType, string msg)
        {
            Log(LogLevel.Fatal, logType, msg);
        }

        public void Fatal(LogType logType, string msg, string data)
        {
            Log(LogLevel.Fatal, logType, msg, data);
        }

        public void Info(LogType logType, string msg)
        {
            Log(LogLevel.Info, logType, msg);
        }

        public void Info(LogType logType, string msg, string data)
        {
            Log(LogLevel.Info, logType, msg, data);
        }

        public void Trace(LogType logType, string msg)
        {
            Log(LogLevel.Trace, logType, msg);
        }

        public void Trace(LogType logType, string msg, string data)
        {
            Log(LogLevel.Trace, logType, msg, data);
        }

        public void Warn(LogType logType, string msg)
        {
            Log(LogLevel.Warn, logType, msg);
        }

        public void Warn(LogType logType, string msg, string data)
        {
            Log(LogLevel.Warn, logType, msg, data);
        }
    }
}
