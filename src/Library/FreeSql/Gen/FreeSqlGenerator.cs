using FreeSql;
using FreeSql.Internal;
using Library.FreeSql.Application;
using Library.FreeSql.Extention;
using Library.FreeSql.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.FreeSql.Gen
{
    /// <summary>
    /// 单库生成器
    /// </summary>
    public class FreeSqlGenerator : IFreeSqlProvider
    {
        private readonly FreeSqlGenOptions Options;

        public FreeSqlGenerator(FreeSqlGenOptions options)
        {
            Options = options ?? new FreeSqlGenOptions();
        }

        protected IFreeSql Orm;

        public FreeSqlBuilder GetFreeSqlBuilder()
        {
            var freeSqlBuilder = new FreeSqlBuilder()
                .UseConnectionString(Options.FreeSqlGeneratorOptions.DatabaseType, Options.FreeSqlGeneratorOptions.ConnectionString);

            Options.FreeSqlDbContextOptions.EntityKey = Options.FreeSqlGeneratorOptions.ConnectionString;

            //基础配置
            if (Options.FreeSqlGeneratorOptions.LazyLoading.HasValue)
                freeSqlBuilder.UseGenerateCommandParameterWithLambda(Options.FreeSqlGeneratorOptions.LazyLoading.Value);

            if (Options.FreeSqlGeneratorOptions.NoneCommandParameter.HasValue)
                freeSqlBuilder.UseNoneCommandParameter(Options.FreeSqlGeneratorOptions.NoneCommandParameter.Value);

            if (Options.FreeSqlGeneratorOptions.GenerateCommandParameterWithLambda.HasValue)
                freeSqlBuilder.UseGenerateCommandParameterWithLambda(Options.FreeSqlGeneratorOptions.GenerateCommandParameterWithLambda.Value);

            if (Options.FreeSqlGeneratorOptions.HandleCommandLog != null ||
                Options.FreeSqlGeneratorOptions.MonitorCommandExecuting != null ||
                Options.FreeSqlGeneratorOptions.MonitorCommandExecuted != null)
                freeSqlBuilder.UseMonitorCommand(
                   Options.FreeSqlGeneratorOptions.MonitorCommandExecuting ??
                   new Action<System.Data.Common.DbCommand>((cmd) => { }),
                   Options.FreeSqlGeneratorOptions.MonitorCommandExecuted ??
                   new Action<System.Data.Common.DbCommand, string>((cmd, log) =>
                   {
                       if (Options.FreeSqlGeneratorOptions.HandleCommandLog != null)
                       {
                           Options.FreeSqlGeneratorOptions.HandleCommandLog.Invoke(log);
                       }
                   }));

            //开发配置
            if (Options.FreeSqlDevOptions?.AutoSyncStructure.HasValue == true)
                freeSqlBuilder.UseAutoSyncStructure(Options.FreeSqlDevOptions.AutoSyncStructure.Value);

            if (Options.FreeSqlDevOptions?.SyncStructureNameConvert.HasValue == true)
                freeSqlBuilder.UseNameConvert(Options.FreeSqlDevOptions.SyncStructureNameConvert.Value);

            if (Options.FreeSqlDevOptions?.ConfigEntityFromDbFirst.HasValue == true)
                freeSqlBuilder.UseConfigEntityFromDbFirst(Options.FreeSqlDevOptions.ConfigEntityFromDbFirst.Value);

            Options.SetupBuilder?.Invoke(freeSqlBuilder);

            return freeSqlBuilder;
        }

        public IFreeSql GetFreeSql()
        {
            if (Orm != null)
                return Orm;

            Orm = GetFreeSqlBuilder().Build();

            SyncStructure();

            return Orm;
        }

        public void SyncStructure()
        {
            //freeSql.Ado.MasterPool.Statistics;
            if (Options.FreeSqlDevOptions?.AutoSyncStructure == true && Options.FreeSqlDevOptions?.SyncStructureOnStartup == true)
                Orm.CodeFirst.SyncStructure(new EntityFactory(Options.FreeSqlDbContextOptions).GetEntitys(Options.FreeSqlDbContextOptions.EntityKey).ToArray());
        }

        public BaseDbContext GetDbContext()
        {
            var db = new BaseDbContext(GetFreeSql(), Options.FreeSqlDbContextOptions);
            return db;
        }
    }
}
