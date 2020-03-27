using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace T4CAGC.Models
{
    public class IdentityConfig
    {
        /// <summary>
        /// 体系
        /// </summary>
        public string Scheme { get; set; }

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
        /// token测试地址
        /// </summary>
        public string TestTokenURI { get; set; }

        /// <summary>
        /// 配置地址
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 配置地址
        /// </summary>
        public string ConfigURI { get { return Config.IndexOf("http") < 0 ? $"http{(useSSL ? "s" : "")}://{IP}:{(useSSL ? sslPort : httpPort)}{Config}" : Config; } set { } }
    }
}
