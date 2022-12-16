using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Application
{
    internal class ConfigureWeChatServiceOptions : IConfigureOptions<WeChatBaseOptions>
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

        public void Configure(WeChatBaseOptions options)
        {
            DeepCopy(WeChatGenOptions.WeChatBaseOptions, options);
        }

        private void DeepCopy(WeChatBaseOptions source, WeChatBaseOptions target)
        {
            target = JsonConvert.DeserializeObject<WeChatBaseOptions>(JsonConvert.SerializeObject(source));
        }
    }
}
