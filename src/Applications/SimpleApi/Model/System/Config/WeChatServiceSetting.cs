using System;
using System.Collections.Generic;
using System.Text;

namespace Model.System.Config
{
    /// <summary>
    /// 微信服务配置
    /// </summary>
    public class WeChatServiceSetting
    {
        #region 基础配置

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
        /// <remarks>使用<see cref="Library.WeChat.Extension.WeChatNotifyV3Middleware"/>中间件时可不配置此属性</remarks>
        public string PayNotifyUrl { get; set; }

        /// <summary>
        /// 异步接收微信支付退款结果通知的回调地址，通知URL必须为外网可访问的url，不允许带参数。
        /// </summary>
        /// <remarks>使用<see cref="Library.WeChat.Extension.WeChatNotifyV3Middleware"/>中间件时可不配置此属性</remarks>
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

        #endregion

        #region 开发配置

        /// <summary>
        /// 开发令牌
        /// </summary>
        /// <remarks>不使用<see cref="Library.WeChat.Extension.WeChatTokenVerificationMiddleware"/>中间件时可不配置此属性</remarks>
        public string Token { get; set; }

        /// <summary>
        /// 开发令牌验证接口
        /// </summary>
        /// <remarks>不使用<see cref="Library.WeChat.Extension.WeChatTokenVerificationMiddleware"/>中间件时可不配置此属性</remarks>
        public string TokenVerificationUrl { get; set; }

        #endregion

        #region 网页授权配置

        /// <summary>
        /// 获取Code接口
        /// </summary>
        public string AuthorizeUrl { get; set; }

        /// <summary>
        /// code换取网页授权access_token接口
        /// </summary>
        public string AccessTokenUrl { get; set; }

        /// <summary>
        /// 拉取用户信息接口
        /// </summary>
        public string UserInfoUrl { get; set; }

        /// <summary>
        /// 授权接口
        /// </summary>
        /// <remarks>
        /// <para>scope为snsapi_base</para>
        /// <para>使用<see cref="Library.WeChat.Extension.WeChatOAuthV2Middleware"/>中间件时，中间件监听的接口。</para>
        /// </remarks>
        public string OAuthBaseUrl { get; set; }

        /// <summary>
        /// 授权接口
        /// </summary>
        /// <remarks>
        /// <para>scope为snsapi_userinfo</para>
        /// <para>使用<see cref="Library.WeChat.Extension.WeChatOAuthV2Middleware"/>中间件时，中间件监听的接口。</para>
        /// </remarks>
        public string OAuthUserInfoUrl { get; set; }

        #endregion
    }
}
