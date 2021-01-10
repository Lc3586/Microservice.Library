using Library.WeChat.Application;
using Library.WeChat.Model;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library.WeChat.Services
{
    /// <summary>
    /// 微信服务
    /// </summary>
    /// <exception cref="WeChatServiceException"></exception>
    public interface IWeChatService
    {
        /// <summary>
        /// 服务版本
        /// </summary>
        WeChatServiceVersion ServiceVersion { get; }

        /// <summary>
        /// 支付接口版本
        /// </summary>
        WeChatPayApiVersion PayApiVersion { get; }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        string TryGetAccessToken();

        /// <summary>
        /// 获取临时素材
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        void GetAttach(string mediaId, Stream stream);

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        WeChatSignatureInfo GetSign(string url);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openId">用户唯一标识</param>
        /// <returns></returns>
        OAuthUserInfo GetUserInfo(string openId);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <returns></returns>
        OAuthUserInfo GetUserInfoByCode(string code);

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="paramter">
        /// 参数, 
        /// 建议使用<see cref="WeChatUnifiedorderParamter.GetSimpleParamter(string, int, string, string, Senparc.Weixin.TenPay.TenPayV3Type)"/>方法获取
        /// </param>
        /// <returns></returns>
        WeChatUnifiedorderResult Pay(WeChatUnifiedorderParamter paramter);

        /// <summary>
        /// 扫码支付
        /// </summary>
        /// <param name="paramter">
        /// 参数, 
        /// 建议使用<see cref="WeChatUnifiedorderParamter.GetSimpleParamter(string, string, int, string, string, Senparc.Weixin.TenPay.TenPayV3Type)"/>方法获取
        /// </param>
        /// <returns></returns>
        WeChatUnifiedorderResult QRPay(WeChatUnifiedorderParamter paramter);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="paramter">
        /// 参数, 
        /// 建议使用<see cref="WeChatRefundParamter.GetSimpleParamter(string, string, int, int)"/>方法获取
        /// </param>
        /// <returns></returns>
        RefundResult Refund(WeChatRefundParamter paramter);

        /// <summary>
        /// 查询退款
        /// </summary>
        /// <param name="paramter">
        /// 参数, 
        /// 建议使用<see cref="WeChatRefundQueryParamter.GetSimpleParamter(string, string, string, int?)"/>方法获取
        /// </param>
        /// <returns></returns>
        RefundQueryResult RefundQuery(WeChatRefundQueryParamter paramter);

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="paramter">
        /// 参数, 
        /// 建议使用<see cref="WeChatSendTemplateMessageParamter.GetSimpleParamter(string, string, object, string)"/>方法获取
        /// </param>
        /// <returns></returns>
        SendTemplateMessageResult SendTemplateMessage(WeChatSendTemplateMessageParamter paramter);
    }
}
