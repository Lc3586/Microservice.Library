using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Library.Extention;

namespace MC.Models
{
    /// <summary>
    /// 身份验证配置信息
    /// </summary>
    public static class IdentityConfig
    {
        /// <summary>
        /// 身份验证配置信息
        /// </summary>
        public static Identity _IdentityConfig { get; set; }
    }

    /// <summary>
    /// 身份验证配置
    /// </summary>
    public class Identity
    {

        /// <summary>
        /// 方案
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// 令牌验证方案
        /// </summary>
        public string TockenAuthScheme { get; set; }

        /// <summary>
        /// 登录方案
        /// </summary>
        public string SignInScheme { get; set; }

        /// <summary>
        /// 报文类型
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端秘钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 是否使用安全连接
        /// </summary>
        public bool useSSL { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// HTTP端口
        /// </summary>
        public string httpPort { get; set; }

        /// <summary>
        /// SSL端口
        /// </summary>
        public string sslPort { get; set; }

        /// <summary>
        /// 完整地址
        /// </summary>
        public string URI
        {
            get { return $"http{(useSSL ? "s" : "")}://{IP}:{(useSSL ? sslPort : httpPort)}"; }
            set { }
        }

        /// <summary>
        /// 是否保存Token
        /// </summary>
        public bool? SaveTokens { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 权限集合
        /// </summary>
        public List<string> ScopeList { get { var _ScopeList = Scope.Split(' ').ToList(); _ScopeList.RemoveAll(o => o.IsNullOrEmpty()); return _ScopeList; } set { } }
    }
}
