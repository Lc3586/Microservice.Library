using Library.WeChat.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Gen
{
    /// <summary>
    /// 微信服务生成器
    /// </summary>
    public class WeChatServiceGenerator : IWeChatServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public WeChatServiceGenerator(WeChatGenOptions options)
        {
            Options = options ?? new WeChatGenOptions();
        }

        readonly WeChatGenOptions Options;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public WeChatServiceV3 GetWeChatServicesV3()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            return new WeChatServiceV3(Options.WeChatServiceOptions);
        }
    }
}
