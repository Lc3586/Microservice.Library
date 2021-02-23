using Business.Utils.Log;
using Library.NLogger.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
using NLog.Targets;
using System.IO;
using System.Text;

namespace Api.Configures
{
    /// <summary>
    /// NLog配置类
    /// </summary>
    public static class NLogConfigura
    {
        /// <summary>
        /// 注册NLog服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterNLog(this IServiceCollection services, SystemConfig config)
        {
            services.AddNLogger(s =>
            {
                s.TargetGeneratorOptions
                .Add(new TargetGeneratorOptions
                {
                    MinLevel = NLog.LogLevel.FromOrdinal(config.MinLogLevel),
                    Target = config.DefaultLoggerType switch
                    {
                        LoggerType.File => new FileTarget
                        {
                            Name = config.DefaultLoggerName,
                            Layout = config.DefaultLoggerLayout ?? NLoggerConfig.Layout,
                            FileName = Path.Combine(Directory.GetCurrentDirectory(), NLoggerConfig.FileDic, NLoggerConfig.FileName),
                            Encoding = Encoding.UTF8
                        },
                        LoggerType.RDBMS => new RDBMSTarget()
                        {
                            Name = config.DefaultLoggerName,
                            Layout = config.DefaultLoggerLayout ?? NLoggerConfig.Layout
                        },
                        LoggerType.ElasticSearch => new ElasticSearchTarget()
                        {
                            Name = config.DefaultLoggerName,
                            Layout = config.DefaultLoggerLayout ?? NLoggerConfig.Layout
                        },
                        _ => new ColoredConsoleTarget
                        {
                            Name = config.DefaultLoggerName,
                            Layout = config.DefaultLoggerLayout ?? NLoggerConfig.Layout
                        },
                    }
                });
            })
            .AddMSLogger();

            return services;
        }

        /// <summary>
        /// 配置NLog
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraNLog(this IApplicationBuilder app, SystemConfig config)
        {
            return app;
        }
    }
}
