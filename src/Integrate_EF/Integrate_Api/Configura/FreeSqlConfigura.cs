using FreeSql;
using Integrate_Business.Config;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Integrate_Api
{
    public class FreeSqlConfigura
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddFreeSql(s =>
            {
                s.FreeSqlGeneratorOptions.ConnectionString = SystemConfig.systemConfig.DefaultDatabaseConnectString;
                s.FreeSqlGeneratorOptions.DatabaseType = (DataType)SystemConfig.systemConfig.DefaultDatabaseType;
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
                //s.FreeSqlGeneratorOptions.HandleCommandLog = (content) =>
                //{
                //    NLog.LogManager.GetLogger("sysLogger").Trace(
                //        new NLog.LogEventInfo(
                //            NLog.LogLevel.Trace,
                //            "dbLogger",
                //            content));
                //};
                //s.FreeSqlGeneratorOptions.EntityPropertyNameConvert = FreeSql.Internal.StringConvertType.PascalCaseToUnderscoreWithLower;

                s.FreeSqlDevOptions.AutoSyncStructure = SystemConfig.systemConfig.FreeSqlAutoSyncStructure;
                s.FreeSqlDevOptions.SyncStructureNameConvert = SystemConfig.systemConfig.FreeSqlSyncStructureNameConvert;
                s.FreeSqlDevOptions.SyncStructureOnStartup = SystemConfig.systemConfig.FreeSqlSyncStructureOnStartup;

                s.FreeSqlDbContextOptions.EnableAddOrUpdateNavigateList = true;
                s.FreeSqlDbContextOptions.EntityAssembly = SystemConfig.systemConfig.EntityAssembly;
            });
        }

        public static void RegisterApplication(IApplicationBuilder app)
        {
            app.ApplicationServices
                .GetService<IFreeSqlProvider>()
                .GetFreeSql();
        }
    }
}
