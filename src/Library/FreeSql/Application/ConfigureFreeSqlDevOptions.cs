using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.FreeSql.Application
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
            options = _freeSqlGenOptions.FreeSqlDevOptions;
        }

        public void DeepCopy(FreeSqlDevOptions source, FreeSqlDevOptions target)
        {

        }
    }
}
