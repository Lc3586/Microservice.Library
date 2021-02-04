using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Model.System.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Configures
{
    /// <summary>
    /// 微信配置类
    /// </summary>
    public class WeChatServiceConfigura
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void RegisterServices(IServiceCollection services, SystemConfig config)
        {
            services.AddWeChatService(option =>
            {
                option.WeChatDevOptions.TokenVerificationUrl = new PathString(config.WeChatService.TokenVerificationUrl);
                option.WeChatDevOptions.Token = config.WeChatService.Token;

                option.WeChatOAuthOptions.WebRootUrl = config.WebRootUrl;
                option.WeChatOAuthOptions.OAuthBaseUrl = new PathString(config.WeChatService.OAuthBaseUrl);
                option.WeChatOAuthOptions.OAuthUserInfoUrl = new PathString(config.WeChatService.OAuthUserInfoUrl);
                option.WeChatOAuthOptions.AuthorizeUrl = config.WeChatService.AuthorizeUrl;
                option.WeChatOAuthOptions.AccessTokenUrl = config.WeChatService.AccessTokenUrl;
                option.WeChatOAuthOptions.UserInfoUrl = config.WeChatService.UserInfoUrl;

                option.WeChatBaseOptions.AppId = config.WeChatService.AppId;
                option.WeChatBaseOptions.Appsecret = config.WeChatService.Appsecret;
                option.WeChatBaseOptions.MchId = config.WeChatService.MchId;
                option.WeChatBaseOptions.Key = config.WeChatService.Key;
                option.WeChatBaseOptions.PayNotifyUrl = config.WeChatService.PayNotifyUrl;
                option.WeChatBaseOptions.RefundNotifyUrl = config.WeChatService.RefundNotifyUrl;
                option.WeChatBaseOptions.UserHostAddress = config.WeChatService.UserHostAddress;
                option.WeChatBaseOptions.CertFilePath = config.WeChatService.CertFilePath;
                option.WeChatBaseOptions.CertPassword = config.WeChatService.CertPassword;
                option.WeChatBaseOptions.PemFilePath = config.WeChatService.PemFilePath;
            });
        }

        /// <summary>
        /// 配置应用
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        public static void RegisterApplication(IApplicationBuilder app, SystemConfig config)
        {
            app.UseWeChatTokenVerification();
            app.UseWeChatOAuthV2();
        }
    }
}
