using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IocServiceDemo
{
    /// <summary>
    /// 配置
    /// </summary>
    internal class ConfigureDemoServiceOptions : IConfigureOptions<DemoServiceOptions>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DemoServiceOptions _options;

        public ConfigureDemoServiceOptions(
           IServiceProvider serviceProvider,
           IOptions<DemoServiceOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public void Configure(DemoServiceOptions options)
        {
            foreach (var property in options.GetType().GetProperties())
            {
                property.SetValue(options, property.GetValue(_options));
            }
        }
    }
}
