using Business.Utils.Log;
using Library.NLogger.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using NLog.Targets;
using System.IO;
using System.Text;

namespace Api.Configures
{
    /// <summary>
    /// NLog配置类
    /// </summary>
    public class NLoggerConfigura
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
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
                        LoggerType.RDBMS => null,
                        LoggerType.ElasticSearch => new ElasticSearchTarget()
                        {
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
        }

        /// <summary>
        /// 配置应用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void RegisterApplication(IApplicationBuilder app, SystemConfig config)
        {

        }
    }
}
