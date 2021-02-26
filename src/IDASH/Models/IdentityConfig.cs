using Microservice.Library.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace IDASH.Models
{
    /// <summary>
    /// 身份验证服务配置
    /// </summary>
    public class IdentityConfig
    {
        public void InitExAuthentication(IConfiguration Configuration)
        {
            QQ = Configuration.GetSection("Identity:QQ").Get<QQConfig>();
            Google = Configuration.GetSection("Identity:Google").Get<GoogleConfig>();
            OpenIDConnect = Configuration.GetSection("Identity:OpenIDConnect").Get<OpenIDConnect>();
        }

        /// <summary>
        /// 使用开发环境
        /// </summary>
        public bool UseDevelopment { get; set; }

        /// <summary>
        /// 安全证书
        /// </summary>
        public string SigningCredential { get; set; }

        /// <summary>
        /// QQ登录配置
        /// </summary>
        public QQConfig QQ { get; set; }

        /// <summary>
        /// 谷歌登录配置
        /// </summary>
        public GoogleConfig Google { get; set; }

        /// <summary>
        /// OpenID配置
        /// </summary>
        public OpenIDConnect OpenIDConnect { get; set; }

        /// <summary>
        /// 是否存在任何的外部登录配置
        /// </summary>
        public bool AnyExAuthentication { get { return QQ != null || Google != null || OpenIDConnect != null; } }
    }

    /// <summary>
    /// QQ登录配置信息
    /// </summary>
    public class QQConfig
    {
        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 是否保存Token
        /// </summary>
        public bool? SaveTokens { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get { return !AppId.IsNullOrEmpty() && !AppKey.IsNullOrEmpty(); } }
    }

    /// <summary>
    /// 谷歌登录配置信息
    /// </summary>
    public class GoogleConfig
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端秘钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 是否保存Token
        /// </summary>
        public bool? SaveTokens { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get { return !ClientId.IsNullOrEmpty() && !ClientSecret.IsNullOrEmpty(); } }
    }

    /// <summary>
    /// OpenID配置信息
    /// </summary>
    public class OpenIDConnect
    {
        /// <summary>
        /// 方案
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// 方案名称
        /// </summary>
        public string SchemeDisplayName { get; set; }

        /// <summary>
        /// 发放地址
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端秘钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 名称声明
        /// </summary>
        public string NameClaimType { get; set; }

        /// <summary>
        /// 角色声明
        /// </summary>
        public string RoleClaimType { get; set; }

        /// <summary>
        /// 是否保存Token
        /// </summary>
        public bool? SaveTokens { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get { return !Scheme.IsNullOrEmpty() && !Authority.IsNullOrEmpty() && !ClientId.IsNullOrEmpty() && !ClientSecret.IsNullOrEmpty(); } }
    }
}
