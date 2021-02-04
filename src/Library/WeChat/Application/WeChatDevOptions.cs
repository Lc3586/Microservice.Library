using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Application
{
    /// <summary>
    /// 微信开发配置
    /// </summary>
    public class WeChatDevOptions
    {
        internal IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Debug状态
        /// </summary>
        public bool IsDebug { get; set; } = false;

        /// <summary>
        /// 开发令牌
        /// </summary>
        /// <remarks>使用<see cref="Extension.WeChatTokenVerificationMiddleware"/>中间件时，中间件使用的Token。</remarks>
        public string Token { get; set; }

        /// <summary>
        /// 开发令牌验证接口
        /// </summary>
        /// <remarks>使用<see cref="Extension.WeChatTokenVerificationMiddleware"/>中间件时，中间件监听的接口。</remarks>
        public PathString TokenVerificationUrl { get; set; }

        /// <summary>
        /// 编码格式
        /// <para>默认utf-8</para>
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 请求签名在请求头中的键
        /// </summary>
        public string RequestHeaderSign { get; set; } = "Wechatpay-Serial";

        /// <summary>
        /// 时间戳在请求头中的键
        /// </summary>
        public string RequestHeaderTimestamp { get; set; } = "Wechatpay-Timestamp";

        /// <summary>
        /// 随机字符串在请求头中的键
        /// </summary>
        public string RequestHeaderNonce { get; set; } = "Wechatpay-Nonce";
    }
}
