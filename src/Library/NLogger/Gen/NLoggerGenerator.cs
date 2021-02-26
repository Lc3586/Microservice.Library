using Microservice.Library.NLogger.Application;

namespace Microservice.Library.NLogger.Gen
{
    /// <summary>
    /// 日志组件生成器
    /// </summary>
    public class NLoggerGenerator : INLoggerProvider
    {
        public NLoggerGenerator(NLoggerGenOptions options)
        {
            Options = options ?? new NLoggerGenOptions();
            Init();
        }

        readonly NLoggerGenOptions Options;

        void Init()
        {
            var config = new NLog.Config.LoggingConfiguration();

            Options.TargetGeneratorOptions.ForEach(o =>
            {
                config.AddTarget(o.Target);
                config.AddRule(o.MinLevel, o.MaxLevel, o.Target);
            });

            NLog.LogManager.Configuration = config;
        }

        public NLog.ILogger GetNLogger(string name)
        {
            return NLog.LogManager.GetLogger(name);
        }
    }
}
