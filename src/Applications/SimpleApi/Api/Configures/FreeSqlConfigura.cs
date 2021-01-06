using FreeSql;
using Library.FreeSql.Gen;
using Library.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Model.System;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Library.TypeTool;

namespace Api.Configures
{
    /// <summary>
    /// FreeSql配置类
    /// </summary>
    public class FreeSqlConfigura
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
        {
            services.AddFreeSql(s =>
            {
                s.FreeSqlGeneratorOptions.ConnectionString = config.DefaultDatabaseConnectString;
                s.FreeSqlGeneratorOptions.DatabaseType = (DataType)config.DefaultDatabaseType;
                s.FreeSqlGeneratorOptions.LazyLoading = true;
                s.FreeSqlGeneratorOptions.MonitorCommandExecuting = (cmd) =>
                {
                    NLog.LogManager.GetLogger("sysLogger").Log(
                        new NLog.LogEventInfo(
                            NLog.LogLevel.Trace,
                            "dbLogger-Executing",
                            cmd.CommandText));
                };
                s.FreeSqlGeneratorOptions.MonitorCommandExecuted = (cmd, log) =>
                {
                    NLog.LogManager.GetLogger("sysLogger").Log(
                        new NLog.LogEventInfo(
                            NLog.LogLevel.Trace,
                            "dbLogger-Executed",
                            log));
                };
                s.FreeSqlGeneratorOptions.HandleCommandLog = (content) =>
                {
                    NLog.LogManager.GetLogger("sysLogger").Trace(
                        new NLog.LogEventInfo(
                            NLog.LogLevel.Trace,
                            "dbLogger",
                            content));
                };
                s.FreeSqlDevOptions.SyncStructureNameConvert = FreeSql.Internal.NameConvertType.PascalCaseToUnderscoreWithLower;

                s.FreeSqlDevOptions.AutoSyncStructure = config.FreeSqlAutoSyncStructure;
                s.FreeSqlDevOptions.SyncStructureNameConvert = config.FreeSqlSyncStructureNameConvert;
                s.FreeSqlDevOptions.SyncStructureOnStartup = config.FreeSqlSyncStructureOnStartup;

                s.FreeSqlDbContextOptions.EnableAddOrUpdateNavigateList = true;
                s.FreeSqlDbContextOptions.EntityAssembly = config.EntityAssembly;
            });
        }

        /// <summary>
        /// 配置应用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void RegisterApplication(IApplicationBuilder app, SystemConfig config)
        {
            //单库预热
            //app.ApplicationServices
            //   .GetService<IFreeSqlProvider>()
            //   .GetFreeSql();
        }
    }
}
