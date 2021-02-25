using Business.Utils.Log;
using Library.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
using Model.Utils.Log;
using System;
using System.Linq;

namespace Api.Configures
{
    /// <summary>
    /// FreeSql多数据库配置类
    /// </summary>
    public static class FreeSqlMultiDatabaseConfigura
    {
        /// <summary>
        /// 注册FreeSql多数据库服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterFreeSqlMultiDatabase(this IServiceCollection services, SystemConfig config)
        {
            #region 多库

            services.AddFreeSql<string>(options =>
            {
                options
                    //设置空闲时间
                    .SetUpIdle(TimeSpan.FromMinutes(30))
                    //禁用IdleBus
                    //.DisableIdleBus()
                    //监听通知
                    .SetUpNotice((arg) =>
                    {
                        var text = $"Type : \"{arg.NoticeType}\", Key : \"{arg.Key}\", Exception : \"{arg.Exception?.GetExceptionAllMsg()}\", Log : \"{arg.Log}\"";
                        Console.WriteLine(text);

                        Logger.Log(
                            NLog.LogLevel.Trace,
                            LogType.系统跟踪,
                            "FreeSql-IdleBus-Notice",
                            text,
                            null,
                            false);
                    });

                //设置生成配置
                config.Databases
                    .Where(d => d.Enable)
                    .ForEach(d =>
                    {
                        options.Add(d.Name, options =>
                        {
                            //自行配置Builder
                            //options.SetupBuilder = builder =>
                            //{
                            //    builder.UseConnectionString(DataType.Odbc, "");
                            //    //...
                            //};

                            options.FreeSqlGeneratorOptions.ConnectionString = d.ConnectString;
                            options.FreeSqlGeneratorOptions.DatabaseType = d.DatabaseType;
                            options.FreeSqlGeneratorOptions.LazyLoading = true;

                            options.FreeSqlGeneratorOptions.MonitorCommandExecuting = (cmd) =>
                            {
                                Logger.Log(
                                    NLog.LogLevel.Trace,
                                    LogType.系统跟踪,
                                    "FreeSql-MonitorCommandExecuting",
                                    cmd.CommandText,
                                    null,
                                    false);
                            };
                            options.FreeSqlGeneratorOptions.MonitorCommandExecuted = (cmd, log) =>
                            {
                                Logger.Log(
                                    NLog.LogLevel.Trace,
                                    LogType.系统跟踪,
                                    "FreeSql-MonitorCommandExecuted",
                                    $"命令: {cmd.CommandText},\r\n日志: {log}.",
                                    null,
                                    false);
                            };
                            options.FreeSqlGeneratorOptions.HandleCommandLog = (content) =>
                            {
                                Logger.Log(
                                    NLog.LogLevel.Trace,
                                    LogType.系统跟踪,
                                    "FreeSql-MonitorCommandExecuting",
                                    content,
                                    null,
                                    false);
                            };

                            options.FreeSqlDevOptions.AutoSyncStructure = config.FreeSql.AutoSyncStructure;
                            options.FreeSqlDevOptions.SyncStructureNameConvert = config.FreeSql.SyncStructureNameConvert;
                            options.FreeSqlDevOptions.SyncStructureOnStartup = config.FreeSql.SyncStructureOnStartup;

                            options.FreeSqlDbContextOptions.EnableAddOrUpdateNavigateList = true;
                            options.FreeSqlDbContextOptions.EntityAssembly = d.EntityAssembly;
                        });
                    });
            });

            #endregion

            return services;
        }

        /// <summary>
        /// 配置FreeSql多数据库服务
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraFreeSqlMultiDatabase(this IApplicationBuilder app, SystemConfig config)
        {
            //多库预热
            //app.ApplicationServices
            //    .GetService<IFreeSqlMultipleProvider<string>>()
            //    .GetAllFreeSql();

            return app;
        }
    }
}
