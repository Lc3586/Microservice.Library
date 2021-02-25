using Business.Utils.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Model.Utils.Config;
using Model.Utils.Log;

namespace Api.Configures
{
    /// <summary>
    /// FreeSql配置类
    /// </summary>
    public static class FreeSqlConfigura
    {
        /// <summary>
        /// 注册FreeSql服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static IServiceCollection RegisterFreeSql(this IServiceCollection services, SystemConfig config)
        {
            services.AddFreeSql(s =>
            {
                s.FreeSqlGeneratorOptions.ConnectionString = config.Database.ConnectString;
                s.FreeSqlGeneratorOptions.DatabaseType = config.Database.DatabaseType;
                s.FreeSqlGeneratorOptions.LazyLoading = true;
                s.FreeSqlGeneratorOptions.MonitorCommandExecuting = (cmd) =>
                {
                    Logger.Log(
                        NLog.LogLevel.Trace,
                        LogType.系统跟踪,
                        "FreeSql-MonitorCommandExecuting",
                        cmd.CommandText,
                        null,
                        false);
                };
                s.FreeSqlGeneratorOptions.MonitorCommandExecuted = (cmd, log) =>
                {
                    Logger.Log(
                        NLog.LogLevel.Trace,
                        LogType.系统跟踪,
                        "FreeSql-MonitorCommandExecuted",
                         $"命令: {cmd.CommandText},\r\n日志: {log}.",
                        null,
                        false);
                };
                s.FreeSqlGeneratorOptions.HandleCommandLog = (content) =>
                {
                    Logger.Log(
                        NLog.LogLevel.Trace,
                        LogType.系统跟踪,
                        "FreeSql-HandleCommandLog",
                        content,
                        null,
                        false);
                };
                s.FreeSqlDevOptions.SyncStructureNameConvert = FreeSql.Internal.NameConvertType.PascalCaseToUnderscoreWithLower;

                s.FreeSqlDevOptions.AutoSyncStructure = config.FreeSql.AutoSyncStructure;
                s.FreeSqlDevOptions.SyncStructureNameConvert = config.FreeSql.SyncStructureNameConvert;
                s.FreeSqlDevOptions.SyncStructureOnStartup = config.FreeSql.SyncStructureOnStartup;

                s.FreeSqlDbContextOptions.EnableAddOrUpdateNavigateList = true;
                s.FreeSqlDbContextOptions.EntityAssembly = config.Database.EntityAssembly;
            });

            return services;
        }

        /// <summary>
        /// 配置FreeSql
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static IApplicationBuilder ConfiguraFreeSql(this IApplicationBuilder app, SystemConfig config)
        {
            //单库预热
            //app.ApplicationServices
            //   .GetService<IFreeSqlProvider>()
            //   .GetFreeSql();

            return app;
        }
    }
}
