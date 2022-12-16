using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace Microservice.Library.SampleAuthentication.Application
{
    /// <summary>
    /// 建议身份验证服务配置选项
    /// </summary>
    public class SampleAuthenticationOptions
    {
        /// <summary>
        /// 配置Cookie身份验证方案
        /// </summary>
        public Action<CookieAuthenticationOptions> ConfigureCookieAuthenticationOptions { get; set; }


    }
}
