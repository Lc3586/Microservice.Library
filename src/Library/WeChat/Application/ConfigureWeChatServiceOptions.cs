using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    internal class ConfigureWeChatServiceOptions : IConfigureOptions<WeChatServiceOptions>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly WeChatGenOptions WeChatGenOptions;

        public ConfigureWeChatServiceOptions(
            IServiceProvider serviceProvider,
            IOptions<WeChatGenOptions> weChatGenOptionsAccessor)
        {
            ServiceProvider = serviceProvider;
            WeChatGenOptions = weChatGenOptionsAccessor.Value;
        }

        public void Configure(WeChatServiceOptions options)
        {
            options.ServiceProvider = ServiceProvider;
            DeepCopy(WeChatGenOptions.WeChatServiceOptions, options);
        }

        private void DeepCopy(WeChatServiceOptions source, WeChatServiceOptions target)
        {
            target = JsonConvert.DeserializeObject<WeChatServiceOptions>(JsonConvert.SerializeObject(source));
        }
    }
}
