using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Application
{
    /// <summary>
    /// 微信基础配置
    /// </summary>
    public class WeChatBaseOptions
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 公众账号密钥
        /// </summary>
        public string Appsecret { get; set; }

        /// <summary>
        /// 服务商户号
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 服务商户秘钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 接收财付通通知的URL
        /// </summary>
        /// <remarks>使用付款通知中间件<see cref="Extension.WeChatNotifyV3Middleware"/>时可不配置此属性</remarks>
        public string PayNotifyUrl { get; set; }

        /// <summary>
        /// 异步接收微信支付退款结果通知的回调地址，通知URL必须为外网可访问的url，不允许带参数。
        /// </summary>
        /// <remarks>使用付款通知中间件<see cref="Extension.WeChatNotifyV3Middleware"/>时可不配置此属性</remarks>
        public string RefundNotifyUrl { get; set; }

        /// <summary>
        /// 当前服务器的公网IP
        /// </summary>
        public string UserHostAddress { get; set; }

        /// <summary>
        /// p12证书地址
        /// </summary>
        public string CertFilePath { get; set; }

        /// <summary>
        /// 证书密码
        /// <para>一般默认为商户号</para>
        /// </summary>
        public string CertPassword { get; set; }

        /// <summary>
        /// pem公钥文件地址
        /// </summary>
        public string PemFilePath { get; set; }
    }
}
