using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    internal class ConfigureWeChatDevOptions : IConfigureOptions<WeChatDevOptions>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly WeChatGenOptions WeChatGenOptions;

        public ConfigureWeChatDevOptions(
            IServiceProvider serviceProvider,
            IOptions<WeChatGenOptions> weChatGenOptionsAccessor)
        {
            ServiceProvider = serviceProvider;
            WeChatGenOptions = weChatGenOptionsAccessor.Value;
        }

        public void Configure(WeChatDevOptions options)
        {
            options.ServiceProvider = ServiceProvider;
            DeepCopy(WeChatGenOptions.WeChatDevOptions, options);
        }

        private void DeepCopy(WeChatDevOptions source, WeChatDevOptions target)
        {
            target = JsonConvert.DeserializeObject<WeChatDevOptions>(JsonConvert.SerializeObject(source));
        }
    }
}
