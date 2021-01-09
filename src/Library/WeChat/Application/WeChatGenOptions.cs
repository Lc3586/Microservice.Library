using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    /// <summary>
    /// WeChat配置
    /// </summary>
    public class WeChatGenOptions
    {
        /// <summary>
        /// 服务配置
        /// </summary>
        public WeChatServiceOptions WeChatServiceOptions { get; set; } = new WeChatServiceOptions();
    }
}
