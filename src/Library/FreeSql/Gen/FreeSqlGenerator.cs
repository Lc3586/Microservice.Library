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
    public class FreeSqlGenerator : IFreeSqlProvider
    {
        private readonly FreeSqlGenOptions _options;

        public FreeSqlGenerator(FreeSqlGenOptions options)
        {
            _options = options ?? new FreeSqlGenOptions();
        }

        private IFreeSql freeSql;

        public IFreeSql GetFreeSql()
        {
            if (freeSql != null)
                return freeSql;

            var freeSqlBuilder = new FreeSqlBuilder()
             .UseConnectionString(_options.FreeSqlGeneratorOptions.DatabaseType, _options.FreeSqlGeneratorOptions.ConnectionString);

            _options.FreeSqlDbContextOptions.EntityKey = _options.FreeSqlGeneratorOptions.ConnectionString;

            //基础配置
            if (_options.FreeSqlGeneratorOptions.LazyLoading.HasValue)
                freeSqlBuilder = freeSqlBuilder.UseGenerateCommandParameterWithLambda(_options.FreeSqlGeneratorOptions.LazyLoading.Value);

            if (_options.FreeSqlGeneratorOptions.NoneCommandParameter.HasValue)
                freeSqlBuilder = freeSqlBuilder.UseNoneCommandParameter(_options.FreeSqlGeneratorOptions.NoneCommandParameter.Value);

            if (_options.FreeSqlGeneratorOptions.GenerateCommandParameterWithLambda.HasValue)
                freeSqlBuilder = freeSqlBuilder.UseGenerateCommandParameterWithLambda(_options.FreeSqlGeneratorOptions.GenerateCommandParameterWithLambda.Value);

            if (_options.FreeSqlGeneratorOptions.HandleCommandLog != null ||
                _options.FreeSqlGeneratorOptions.MonitorCommandExecuting != null ||
                _options.FreeSqlGeneratorOptions.MonitorCommandExecuted != null)
                freeSqlBuilder = freeSqlBuilder.UseMonitorCommand(
                    _options.FreeSqlGeneratorOptions.MonitorCommandExecuting ??
                    new Action<System.Data.Common.DbCommand>((cmd) => { }),
                    _options.FreeSqlGeneratorOptions.MonitorCommandExecuted ??
                    new Action<System.Data.Common.DbCommand, string>((cmd, log) =>
                    {
                        if (_options.FreeSqlGeneratorOptions.HandleCommandLog != null)
                        {
                            _options.FreeSqlGeneratorOptions.HandleCommandLog.Invoke(log);
                        }
                    }));

            //开发配置
            if (_options.FreeSqlDevOptions?.AutoSyncStructure.HasValue == true)
                freeSqlBuilder = freeSqlBuilder.UseAutoSyncStructure(_options.FreeSqlDevOptions.AutoSyncStructure.Value);

            if (_options.FreeSqlDevOptions?.SyncStructureNameConvert.HasValue == true)
                freeSqlBuilder = freeSqlBuilder.UseNameConvert(_options.FreeSqlDevOptions.SyncStructureNameConvert.Value);

            if (_options.FreeSqlDevOptions?.ConfigEntityFromDbFirst.HasValue == true)
                freeSqlBuilder = freeSqlBuilder.UseConfigEntityFromDbFirst(_options.FreeSqlDevOptions.ConfigEntityFromDbFirst.Value);

            freeSql = freeSqlBuilder.Build();
            //freeSql.Ado.MasterPool.Statistics;
            if (_options.FreeSqlDevOptions?.AutoSyncStructure.HasValue == true && _options.FreeSqlDevOptions?.SyncStructureOnStartup == true)
                freeSql.CodeFirst.SyncStructure(new EntityFactory(_options.FreeSqlDbContextOptions).GetEntitys(_options.FreeSqlDbContextOptions.EntityKey).ToArray());

            return freeSql;
        }

        public BaseDbContext GetDbContext()
        {
            var db = new BaseDbContext(GetFreeSql(), _options.FreeSqlDbContextOptions);
            return db;
        }
    }
}
