using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.FreeSql.Application
{
    internal class ConfigureFreeSqlDevOptions : IConfigureOptions<FreeSqlDevOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FreeSqlGenOptions _freeSqlGenOptions;

        public ConfigureFreeSqlDevOptions(
            IServiceProvider serviceProvider,
            IOptions<FreeSqlGenOptions> freeSqlGenOptionsAccessor)
        {
            _serviceProvider = serviceProvider;
            _freeSqlGenOptions = freeSqlGenOptionsAccessor.Value;
        }

        public void Configure(FreeSqlDevOptions options)
        {
            DeepCopy(_freeSqlGenOptions.FreeSqlDevOptions, options);
        }

        private void DeepCopy(FreeSqlDevOptions source, FreeSqlDevOptions target)
        {
            target.SyncStructureOnStartup = source.SyncStructureOnStartup;
            target.AutoSyncStructure = source.AutoSyncStructure;
            target.SyncStructureNameConvert = source.SyncStructureNameConvert;
            target.ConfigEntityFromDbFirst = source.ConfigEntityFromDbFirst;
        }
    }
}
