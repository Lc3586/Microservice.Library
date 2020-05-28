using Library.Container;
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
    public class Helper
    {
        public static System.DateTime TryGetAccessTokenTime;

        public static string ACCESS_TOKEN = "";

        private static IServiceProvider ServiceProvider = AutofacHelper.GetService<IServiceProvider>();

        public static void Create()
        {
            const bool isGLobalDebug = true; //设置全局 Debug 状态
            SenparcSetting senparcSetting = new SenparcSetting(isGLobalDebug);
            IRegisterService register = RegisterService.Start(senparcSetting).UseSenparcGlobal(); //CO2NET全局注册，必须！

            const bool isWeixinDebug = true; //设置微信 Debug 状态
            SenparcWeixinSetting senparcWeixinSetting = new SenparcWeixinSetting(isWeixinDebug);
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting); ////微信全局注册，必须！
            AccessTokenContainer.RegisterAsync(WeixinConfig.AppId, WeixinConfig.Appsecret).GetAwaiter().GetResult();
        }

        public static string TryGetAccessToken()
        {
            if (TryGetAccessTokenTime.AddHours(1) < System.DateTime.Now || string.IsNullOrEmpty(ACCESS_TOKEN))
            {
                TryGetAccessTokenTime = System.DateTime.Now;
                ACCESS_TOKEN =
                    AccessTokenContainer.TryGetAccessToken(WeixinConfig.AppId, WeixinConfig.Appsecret, true);
            }

            return ACCESS_TOKEN;
        }

        public static void GetAttach(string mediaId, Stream stream)
        {
            MediaApi.Get(TryGetAccessToken(), mediaId, stream);
        }

        public static WeixinSignatureInfo GetSign(string url)
        {
            string ticket = JsApiTicketContainer.TryGetJsApiTicket(WeixinConfig.AppId, WeixinConfig.Appsecret);
            string timestamp = JSSDKHelper.GetTimestamp();
            string nonceStr = JSSDKHelper.GetNoncestr();
            string signature = JSSDKHelper.GetSignature(ticket, nonceStr, timestamp, url);

            return new WeixinSignatureInfo
            {
                Ticket = ticket,
                Timestamp = timestamp,
                NonceStr = nonceStr,
                Signature = signature
            };
        }

        public static WeixinUserInfo GetUserInfo(string openId)
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

        public static WeixinUserInfo GetUserInfoByCode(string code)
        {
            OAuthAccessTokenResult token = OAuthApi.GetAccessToken(WeixinConfig.AppId, WeixinConfig.Appsecret, code);

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

        public static Model.UnifiedorderResult QRPay(string orderNum, int price, string body, string productId)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string timeStamp = TenPayV3Util.GetTimestamp();

            TenPayV3UnifiedorderRequestData tenPayV3UnifiedorderRequestData = new TenPayV3UnifiedorderRequestData(
                WeixinConfig.AppId, WeixinConfig.MchId, body, orderNum, price, WeixinConfig.UserHostAddress,
                WeixinConfig.QRNotifyUrl, TenPayV3Type.NATIVE, null, WeixinConfig.Key, nonceStr, productId: productId);

            Senparc.Weixin.TenPay.V3.UnifiedorderResult unfortifiedResult = TenPayV3.Unifiedorder(tenPayV3UnifiedorderRequestData);

            string package = $"prepay_id={unfortifiedResult.prepay_id}";

            if (unfortifiedResult.return_code == "FAIL")
                throw new System.Exception("微信统一下单失败");

            return new Model.UnifiedorderResult
            {
                AppId = WeixinConfig.AppId,
                TimeStamp = timeStamp,
                NonceStr = nonceStr,
                Package = package,
                CodeUrl = unfortifiedResult.code_url,
                PaySign = TenPayV3.GetJsPaySign(WeixinConfig.AppId, timeStamp, nonceStr, package, WeixinConfig.Key)
            };
        }

        public static Model.UnifiedorderResult Pay(string openid, string orderNum, int price, string body)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string timeStamp = TenPayV3Util.GetTimestamp();

            TenPayV3UnifiedorderRequestData tenPayV3UnifiedorderRequestData = new TenPayV3UnifiedorderRequestData(
                WeixinConfig.AppId, WeixinConfig.MchId, body, orderNum, price, WeixinConfig.UserHostAddress,
                WeixinConfig.NotifyUrl, TenPayV3Type.JSAPI, openid, WeixinConfig.Key, nonceStr);

            Senparc.Weixin.TenPay.V3.UnifiedorderResult unfortifiedResult = TenPayV3.Unifiedorder(tenPayV3UnifiedorderRequestData);

            string package = $"prepay_id={unfortifiedResult.prepay_id}";

            return new Model.UnifiedorderResult
            {
                AppId = WeixinConfig.AppId,
                TimeStamp = timeStamp,
                NonceStr = nonceStr,
                Package = package,
                PaySign = TenPayV3.GetJsPaySign(WeixinConfig.AppId, timeStamp, nonceStr, package, WeixinConfig.Key)
            };
        }

        public static RefundResult Refund(string outTradeNo, string outRefundNo, int totalFee, int refundFee)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3RefundRequestData tenPayV3RefundRequestData = new TenPayV3RefundRequestData(WeixinConfig.AppId,
                WeixinConfig.MchId, WeixinConfig.Key, WeixinConfig.DeviceInfo, nonceStr, null,
                outTradeNo, outRefundNo, totalFee, refundFee, WeixinConfig.MchId, "REFUND_SOURCE_UNSETTLED_FUNDS", null, WeixinConfig.RefundNotifyUrl);

            RefundResult refundResult = TenPayV3.Refund(ServiceProvider, tenPayV3RefundRequestData);//,
                                                                                                    //$"{AppDomain.CurrentDomain.BaseDirectory}/apiclient_cert.p12", WeixinConfig.CertPassword);

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
        public static RefundQueryResult RefundQuery(string outTradeNo, string outRefundNo, string refundId, int? offset)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3RefundQueryRequestData tenPayV3RefundRequestData = new TenPayV3RefundQueryRequestData(WeixinConfig.AppId,
                WeixinConfig.MchId, WeixinConfig.Key, nonceStr, WeixinConfig.DeviceInfo, null,
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
        public static void SendTemplateMessage(string openid, string templateId, string url, object data)
        {
            SendTemplateMessageResult res = TemplateApi.SendTemplateMessage(TryGetAccessToken(), openid, templateId,
                url, data);
        }
    }
}
