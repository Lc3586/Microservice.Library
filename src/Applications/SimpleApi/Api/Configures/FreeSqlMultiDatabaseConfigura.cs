using Library.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using System;
using System.Linq;

namespace Api.Configures
{
    /// <summary>
    /// FreeSql多数据库配置类
    /// </summary>
    public class FreeSqlMultiDatabaseConfigura
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
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

                        NLog.LogManager.GetLogger("sysLogger").Trace(
                            new NLog.LogEventInfo(
                                NLog.LogLevel.Trace,
                                "dbLogger",
                                text));
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
                                NLog.LogManager.GetLogger("sysLogger").Trace(
                                    new NLog.LogEventInfo(
                                        NLog.LogLevel.Trace,
                                        "dbLogger",
                                        cmd.CommandText));
                            };
                            options.FreeSqlGeneratorOptions.MonitorCommandExecuted = (cmd, log) =>
                            {
                                NLog.LogManager.GetLogger("sysLogger").Trace(
                                    new NLog.LogEventInfo(
                                        NLog.LogLevel.Trace,
                                        "dbLogger",
                                        $"命令 {cmd},日志 {log}."));
                            };
                            options.FreeSqlGeneratorOptions.HandleCommandLog = (content) =>
                            {
                                NLog.LogManager.GetLogger("sysLogger").Trace(
                                    new NLog.LogEventInfo(
                                        NLog.LogLevel.Trace,
                                        "dbLogger",
                                        content));
                            };

                            options.FreeSqlDevOptions.AutoSyncStructure = config.FreeSqlAutoSyncStructure;
                            options.FreeSqlDevOptions.SyncStructureNameConvert = config.FreeSqlSyncStructureNameConvert;
                            options.FreeSqlDevOptions.SyncStructureOnStartup = config.FreeSqlSyncStructureOnStartup;

                            options.FreeSqlDbContextOptions.EnableAddOrUpdateNavigateList = true;
                            options.FreeSqlDbContextOptions.EntityAssembly = d.EntityAssembly;
                        });
                    });
            });

            #endregion
        }

        /// <summary>
        /// 配置应用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void RegisterApplication(IApplicationBuilder app, SystemConfig config)
        {
            //多库预热
            //app.ApplicationServices
            //    .GetService<IFreeSqlMultipleProvider<string>>()
            //    .GetAllFreeSql();
        }
    }
}
