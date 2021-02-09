using Library.SampleAuthentication.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 简易身份验证服务依赖注入扩展类
    /// </summary>
    public static class SampleAuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// 注册简易身份验证服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSampleAuthentication(
            this AuthenticationBuilder builder,
            Action<SampleAuthenticationOptions> configureOptions = null)
        {
            return builder.AddSampleAuthentication(CookieAuthenticationDefaults.AuthenticationScheme, "SA", configureOptions);
        }

        /// <summary>
        /// 注册简易身份验证服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="authenticationScheme">框架</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSampleAuthentication(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<SampleAuthenticationOptions> configureOptions = null)
        {
            return builder.AddScheme<SampleAuthenticationOptions, SampleAuthenticationHandler<SampleAuthenticationOptions>>(authenticationScheme, displayName, configureOptions);
        }
    }
}
