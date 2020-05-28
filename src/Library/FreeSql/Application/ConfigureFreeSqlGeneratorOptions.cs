using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Application
{
    internal class ConfigureFreeSqlGeneratorOptions : IConfigureOptions<FreeSqlGeneratorOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FreeSqlGenOptions _freeSqlGenOptions;

        public ConfigureFreeSqlGeneratorOptions(
            IServiceProvider serviceProvider,
            IOptions<FreeSqlGenOptions> freeSqlGenOptionsAccessor)
        {
            _serviceProvider = serviceProvider;
            _freeSqlGenOptions = freeSqlGenOptionsAccessor.Value;
        }

        public void Configure(FreeSqlGeneratorOptions options)
        {
            DeepCopy(_freeSqlGenOptions.FreeSqlGeneratorOptions, options);
        }

        private void DeepCopy(FreeSqlGeneratorOptions source, FreeSqlGeneratorOptions target)
        {
            target.DatabaseType = source.DatabaseType;
            target.ConnectionString = source.ConnectionString;
            target.HandleCommandLog = source.HandleCommandLog;
            target.MonitorCommandExecuting = source.MonitorCommandExecuting;
            target.MonitorCommandExecuted = source.MonitorCommandExecuted;
            target.LazyLoading = source.LazyLoading;
            target.NoneCommandParameter = source.NoneCommandParameter;
            target.GenerateCommandParameterWithLambda = source.GenerateCommandParameterWithLambda;
        }
    }
}
