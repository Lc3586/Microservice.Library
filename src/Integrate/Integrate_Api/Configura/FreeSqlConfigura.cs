using FreeSql;
using Integrate_Business.Config;
using Library.Log;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integrate_Api
{
    public class FreeSqlConfigura
    {
        public static void RegisterServices(IServiceCollection services)
        {
            s.FreeSqlGeneratorOptions.ConnectionString = SystemConfig.systemConfig.DefaultDatabaseConnectString;
            s.FreeSqlGeneratorOptions.DatabaseType = (DataType)SystemConfig.systemConfig.DefaultDatabaseType;
            s.FreeSqlGeneratorOptions.LazyLoading = true;
            s.FreeSqlGeneratorOptions.HandleCommandLog = (content) =>
            {
                NLog.LogManager.GetLogger("sysLogger").Trace(
                    new NLog.LogEventInfo(
                        NLog.LogLevel.Trace,
                        "dbLogger",
                        content));
            };
            //s.FreeSqlGeneratorOptions.EntityPropertyNameConvert = FreeSql.Internal.StringConvertType.PascalCaseToUnderscoreWithLower;

            #region 开发选项，生产环境注释

            s.FreeSqlDevOptions.AutoSyncStructure = true;
            //s.FreeSqlDevOptions.SyncStructureToLower = true;
            s.FreeSqlDevOptions.SyncStructureOnStartup = true;

            #endregion

            s.FreeSqlDbContextOptions.EnableAddOrUpdateNavigateList = true;
            s.FreeSqlDbContextOptions.EntityAssembly = SystemConfig.systemConfig.EntityAssembly;
        });
        }
    }
}
