using Microservice.Library.WeChat.Application;
using Microservice.Library.WeChat.Model;
using Microservice.Library.WeChat.Services;
using System.Collections.Generic;

namespace Microservice.Library.WeChat.Gen
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
            Services = new Dictionary<WeChatServiceVersion, IWeChatService>();
        }

        #region 私有成员

        readonly WeChatGenOptions Options;


        readonly Dictionary<WeChatServiceVersion, IWeChatService> Services;

        IWeChatService GetWeChatServices(WeChatServiceVersion serviceVersion)
        {
            if (Services.ContainsKey(serviceVersion))
                return Services[serviceVersion];

            switch (serviceVersion)
            {
                case WeChatServiceVersion.V3:
                    Services[serviceVersion] = new WeChatServiceV3(Options);
                    break;
                default:
                    throw new WeChatServiceException($"不支持的微信服务版本: {serviceVersion}");
            }

            return Services[serviceVersion];
        }

        #endregion

        #region 公共方法

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public IWeChatService GetWeChatServicesV3()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            return GetWeChatServices(WeChatServiceVersion.V3);
        }

        #endregion
    }
}
