using Microservice.Library.WeChat.Application;
using Microservice.Library.WeChat.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Gen
{
    /// <summary>
    /// 微信服务构造器
    /// </summary>
    public interface IWeChatServiceProvider
    {
        /// <summary>
        /// 获取微信服务
        /// <para>V3版本</para>
        /// </summary>
        /// <returns></returns>
        IWeChatService GetWeChatServicesV3();
    }
}
