using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    internal class ConfigureWeChatOAuthOptions : IConfigureOptions<WeChatOAuthOptions>
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly WeChatGenOptions WeChatGenOptions;

        public ConfigureWeChatOAuthOptions(
            IServiceProvider serviceProvider,
            IOptions<WeChatGenOptions> weChatGenOptionsAccessor)
        {
            ServiceProvider = serviceProvider;
            WeChatGenOptions = weChatGenOptionsAccessor.Value;
        }

        public void Configure(WeChatOAuthOptions options)
        {
            DeepCopy(WeChatGenOptions.WeChatOAuthOptions, options);
        }

        private void DeepCopy(WeChatOAuthOptions source, WeChatOAuthOptions target)
        {
            target = JsonConvert.DeserializeObject<WeChatOAuthOptions>(JsonConvert.SerializeObject(source));
        }
    }
}
