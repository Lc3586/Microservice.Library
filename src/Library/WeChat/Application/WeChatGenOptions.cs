using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Application
{
    /// <summary>
    /// WeChat配置
    /// </summary>
    public class WeChatGenOptions
    {
        /// <summary>
        /// 基础配置
        /// </summary>
        public WeChatBaseOptions WeChatBaseOptions { get; set; } = new WeChatBaseOptions();

        /// <summary>
        /// 开发配置
        /// </summary>
        public WeChatDevOptions WeChatDevOptions { get; set; } = new WeChatDevOptions();

        /// <summary>
        /// 授权配置
        /// </summary>
        public WeChatOAuthOptions WeChatOAuthOptions { get; set; } = new WeChatOAuthOptions();
    }
}
