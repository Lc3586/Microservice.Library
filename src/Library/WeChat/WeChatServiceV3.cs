using Library.WeChat.Application;
using Library.WeChat.Extension;
using Library.WeChat.Model;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library.WeChat
{
    /// <summary>
    /// 微信服务
    /// </summary>
    public class WeChatServiceV3
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public WeChatServiceV3(WeChatServiceOptions options)
        {
            Options = options;
            Init();
        }

        #region 私有成员

        readonly WeChatServiceOptions Options;

        DateTime TryGetAccessTokenTime;

        string ACCESS_TOKEN = "";

        void Init()
        {
            SenparcSetting senparcSetting = new SenparcSetting(Options.IsDebug);
            IRegisterService register = RegisterService.Start(senparcSetting).UseSenparcGlobal(); //CO2NET全局注册，必须！

            SenparcWeixinSetting senparcWeixinSetting = new SenparcWeixinSetting(Options.IsDebug);
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting); ////微信全局注册，必须！
            AccessTokenContainer.RegisterAsync(Options.AppId, Options.Appsecret).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取通知Url
        /// </summary>
        /// <param name="configUrl">配置地址</param>
        /// <param name="notifyType">通知类型</param>
        /// <returns></returns>
        string GetNotifyUrl(string configUrl, WeChatNotifyType notifyType)
        {
            if (!string.IsNullOrWhiteSpace(configUrl))
                return configUrl;

            var middleware = (WeChatNotifyV3Middleware)Options.ServiceProvider.GetService(typeof(WeChatNotifyV3Middleware));
            if (middleware != null && middleware.UrlDic.ContainsKey(notifyType))
                return middleware.UrlDic[WeChatNotifyType.Pay];
            else
                throw new WeChatServiceException($"未配置接收通知的URL, WeChatNotifyType: {notifyType}.");
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public string TryGetAccessToken()
        {
            if (TryGetAccessTokenTime.AddHours(1) < System.DateTime.Now || string.IsNullOrEmpty(ACCESS_TOKEN))
            {
                TryGetAccessTokenTime = DateTime.Now;
                ACCESS_TOKEN =
                    AccessTokenContainer.TryGetAccessToken(Options.AppId, Options.Appsecret, true);
            }

            return ACCESS_TOKEN;
        }

        /// <summary>
        /// 获取临时素材
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="stream"></param>
        public void GetAttach(string mediaId, Stream stream)
        {
            MediaApi.Get(TryGetAccessToken(), mediaId, stream);
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WeChatSignatureInfo GetSign(string url)
        {
            string ticket = JsApiTicketContainer.TryGetJsApiTicket(Options.AppId, Options.Appsecret);
            string timestamp = JSSDKHelper.GetTimestamp();
            string nonceStr = JSSDKHelper.GetNoncestr();
            string signature = JSSDKHelper.GetSignature(ticket, nonceStr, timestamp, url);

            return new WeChatSignatureInfo
            {
                Ticket = ticket,
                Timestamp = timestamp,
                NonceStr = nonceStr,
                Signature = signature
            };
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeixinUserInfo GetUserInfo(string openId)
        {
            OAuthUserInfo oAuthUserInfo = OAuthApi.GetUserInfo(TryGetAccessToken(), openId);

            WeixinUserInfo fineWeixinUserInfo = new WeixinUserInfo
            {
                openid = oAuthUserInfo.openid,
                nickname = oAuthUserInfo.nickname,
                sex = oAuthUserInfo.sex,
                province = oAuthUserInfo.province,
                country = oAuthUserInfo.country,
                headimgurl = oAuthUserInfo.headimgurl,
                privilege = oAuthUserInfo.privilege,
                unionid = oAuthUserInfo.unionid
            };

            return fineWeixinUserInfo;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WeixinUserInfo GetUserInfoByCode(string code)
        {
            OAuthAccessTokenResult token = OAuthApi.GetAccessToken(Options.AppId, Options.Appsecret, code);

            OAuthUserInfo oAuthUserInfo = OAuthApi.GetUserInfo(token.access_token, token.openid);

            WeixinUserInfo fineWeixinUserInfo = new WeixinUserInfo
            {
                openid = oAuthUserInfo.openid,
                nickname = oAuthUserInfo.nickname,
                sex = oAuthUserInfo.sex,
                province = oAuthUserInfo.province,
                country = oAuthUserInfo.country,
                headimgurl = oAuthUserInfo.headimgurl,
                privilege = oAuthUserInfo.privilege,
                unionid = oAuthUserInfo.unionid
            };

            return fineWeixinUserInfo;
        }

        /// <summary>
        /// 扫码支付
        /// </summary>
        /// <param name="orderNum"></param>
        /// <param name="price"></param>
        /// <param name="body"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Model.UnifiedorderResult QRPay(string orderNum, int price, string body, string productId)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string timeStamp = TenPayV3Util.GetTimestamp();

            TenPayV3UnifiedorderRequestData tenPayV3UnifiedorderRequestData = new TenPayV3UnifiedorderRequestData(
                Options.AppId, Options.MchId, body, orderNum, price, Options.UserHostAddress,
               GetNotifyUrl(Options.PayNotifyUrl, WeChatNotifyType.Pay), TenPayV3Type.NATIVE, null, Options.Key, nonceStr, productId: productId);

            Senparc.Weixin.TenPay.V3.UnifiedorderResult unfortifiedResult = TenPayV3.Unifiedorder(tenPayV3UnifiedorderRequestData);

            string package = $"prepay_id={unfortifiedResult.prepay_id}";

            if (unfortifiedResult.return_code == "FAIL")
                throw new System.Exception("微信统一下单失败");

            return new Model.UnifiedorderResult
            {
                AppId = Options.AppId,
                TimeStamp = timeStamp,
                NonceStr = nonceStr,
                Package = package,
                CodeUrl = unfortifiedResult.code_url,
                PaySign = TenPayV3.GetJsPaySign(Options.AppId, timeStamp, nonceStr, package, Options.Key)
            };
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="orderNum"></param>
        /// <param name="price"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public Model.UnifiedorderResult Pay(string openid, string orderNum, int price, string body)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string timeStamp = TenPayV3Util.GetTimestamp();

            TenPayV3UnifiedorderRequestData tenPayV3UnifiedorderRequestData = new TenPayV3UnifiedorderRequestData(
                Options.AppId, Options.MchId, body, orderNum, price, Options.UserHostAddress,
                GetNotifyUrl(Options.PayNotifyUrl, WeChatNotifyType.Pay), TenPayV3Type.JSAPI, openid, Options.Key, nonceStr);

            Senparc.Weixin.TenPay.V3.UnifiedorderResult unfortifiedResult = TenPayV3.Unifiedorder(tenPayV3UnifiedorderRequestData);

            string package = $"prepay_id={unfortifiedResult.prepay_id}";

            return new Model.UnifiedorderResult
            {
                AppId = Options.AppId,
                TimeStamp = timeStamp,
                NonceStr = nonceStr,
                Package = package,
                PaySign = TenPayV3.GetJsPaySign(Options.AppId, timeStamp, nonceStr, package, Options.Key)
            };
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="outRefundNo"></param>
        /// <param name="totalFee"></param>
        /// <param name="refundFee"></param>
        /// <returns></returns>
        public RefundResult Refund(string outTradeNo, string outRefundNo, int totalFee, int refundFee)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3RefundRequestData tenPayV3RefundRequestData = new TenPayV3RefundRequestData(Options.AppId,
                Options.MchId, Options.Key, Options.DeviceInfo, nonceStr, null,
                outTradeNo, outRefundNo, totalFee, refundFee, Options.MchId, "REFUND_SOURCE_UNSETTLED_FUNDS", null, GetNotifyUrl(Options.RefundNotifyUrl, WeChatNotifyType.Refund));

            RefundResult refundResult = TenPayV3.Refund(Options.ServiceProvider, tenPayV3RefundRequestData);//,
                                                                                                            //$"{AppDomain.CurrentDomain.BaseDirectory}/apiclient_cert.p12", Options.CertPassword);

            return refundResult;
        }

        /// <summary>
        /// 查询退款
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="outRefundNo"></param>
        /// <param name="refundId"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public RefundQueryResult RefundQuery(string outTradeNo, string outRefundNo, string refundId, int? offset)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3RefundQueryRequestData tenPayV3RefundRequestData = new TenPayV3RefundQueryRequestData(Options.AppId,
                Options.MchId, Options.Key, nonceStr, Options.DeviceInfo, null,
                outTradeNo, outRefundNo, refundId, null, null, offset);

            RefundQueryResult refundQueryResult = TenPayV3.RefundQuery(tenPayV3RefundRequestData);
            return refundQueryResult;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="templateId"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public SendTemplateMessageResult SendTemplateMessage(string openid, string templateId, string url, object data)
        {
            return TemplateApi.SendTemplateMessage(TryGetAccessToken(), openid, templateId,
                url, data);
        }

        #endregion
    }
}
