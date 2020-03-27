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
            options = _freeSqlGenOptions.FreeSqlGeneratorOptions;
        }

        public void DeepCopy(FreeSqlGeneratorOptions source, FreeSqlGeneratorOptions target)
        {

        }
    }
}
