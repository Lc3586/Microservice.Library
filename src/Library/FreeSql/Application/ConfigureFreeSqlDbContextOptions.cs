using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.FreeSql.Application
{
    internal class ConfigureFreeSqlDbContextOptions : IConfigureOptions<FreeSqlDbContextOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly FreeSqlGenOptions _freeSqlGenOptions;

        public ConfigureFreeSqlDbContextOptions(
            IServiceProvider serviceProvider,
            IOptions<FreeSqlGenOptions> freeSqlGenOptionsAccessor)
        {
            _serviceProvider = serviceProvider;
            _freeSqlGenOptions = freeSqlGenOptionsAccessor.Value;
        }

        public void Configure(FreeSqlDbContextOptions options)
        {
            DeepCopy(_freeSqlGenOptions.FreeSqlDbContextOptions, options);
        }

        private void DeepCopy(FreeSqlDbContextOptions source, FreeSqlDbContextOptions target)
        {
            target.EntityAssembly = source.EntityAssembly;
            target.EntityKey = source.EntityKey;
            target.EnableAddOrUpdateNavigateList = source.EnableAddOrUpdateNavigateList;
            target.OnEntityChange = source.OnEntityChange;
        }
    }
}
