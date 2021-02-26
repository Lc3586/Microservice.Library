using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.DataMapping.Application
{
    internal class ConfigureAutoMapperGeneratorOptions : IConfigureOptions<AutoMapperGeneratorOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AutoMapperGenOptions _autoMapperGenOptions;

        public ConfigureAutoMapperGeneratorOptions(
            IServiceProvider serviceProvider,
            IOptions<AutoMapperGenOptions> autoMapperOptionsAccessor)
        {
            _serviceProvider = serviceProvider;
            _autoMapperGenOptions = autoMapperOptionsAccessor.Value;
        }

        public void Configure(AutoMapperGeneratorOptions options)
        {
            options = _autoMapperGenOptions.AutoMapperGeneratorOptions;
        }

        public void DeepCopy(AutoMapperGeneratorOptions source, AutoMapperGeneratorOptions target)
        {

        }
    }
}
