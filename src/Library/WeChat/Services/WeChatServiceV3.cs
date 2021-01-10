using Library.WeChat.Application;
using Library.WeChat.Extension;
using Library.WeChat.Model;
using Newtonsoft.Json;
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

namespace Library.WeChat.Services
{
    /// <summary>
    /// 微信服务
    /// V3版本
    /// </summary>
    /// <exception cref="WeChatServiceException"></exception>
    public class WeChatServiceV3 : IWeChatService
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

        /// <summary>
        /// 服务配置
        /// </summary>
        public readonly WeChatServiceOptions Options;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public WeChatServiceVersion ServiceVersion => WeChatServiceVersion.V3;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public WeChatPayApiVersion PayApiVersion => WeChatPayApiVersion.V3;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        #region 私有成员

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

        string GetNonceStr(WeChatUnifiedorderParamter paramter)
        {
            return string.IsNullOrWhiteSpace(paramter.NonceStr) ? TenPayV3Util.GetNoncestr() : paramter.NonceStr;
        }

        string GetDetail(WeChatUnifiedorderParamter paramter)
        {
            return paramter.Detail == null ? null : $"<![CDATA[{JsonConvert.SerializeObject(paramter.Detail)}]]>";
        }

        TenPayV3UnifiedorderRequestData_SceneInfo GetSceneInfo(WeChatUnifiedorderParamter paramter)
        {
            return paramter.SceneInfo == null
                ? null
                : new TenPayV3UnifiedorderRequestData_SceneInfo(paramter.SceneInfo.IsH5Pay, paramter.SceneInfo.H5_Info)
                {
                    store_info = new Store_Info
                    {
                        id = paramter.SceneInfo.id,
                        name = paramter.SceneInfo.name,
                        area_code = paramter.SceneInfo.area_code,
                        address = paramter.SceneInfo.address
                    }
                };
        }

        #endregion

        #region 公开方法

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public string TryGetAccessToken()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            if (TryGetAccessTokenTime.AddHours(1) < System.DateTime.Now || string.IsNullOrEmpty(ACCESS_TOKEN))
            {
                TryGetAccessTokenTime = DateTime.Now;
                ACCESS_TOKEN =
                    AccessTokenContainer.TryGetAccessToken(Options.AppId, Options.Appsecret, true);
            }

            return ACCESS_TOKEN;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public void GetAttach(string mediaId, Stream stream)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            MediaApi.Get(TryGetAccessToken(), mediaId, stream);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public WeChatSignatureInfo GetSign(string url)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
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

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public OAuthUserInfo GetUserInfo(string openId)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            return OAuthApi.GetUserInfo(TryGetAccessToken(), openId);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public OAuthUserInfo GetUserInfoByCode(string code)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            OAuthAccessTokenResult token = OAuthApi.GetAccessToken(Options.AppId, Options.Appsecret, code);

            return OAuthApi.GetUserInfo(token.access_token, token.openid);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public WeChatUnifiedorderResult Pay(WeChatUnifiedorderParamter paramter)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            string nonceStr = GetNonceStr(paramter);
            string timeStamp = TenPayV3Util.GetTimestamp();

            TenPayV3UnifiedorderRequestData tenPayV3UnifiedorderRequestData = new TenPayV3UnifiedorderRequestData(
                Options.AppId, Options.MchId, paramter.Body, paramter.OutTradeNo, paramter.TotalFee, Options.UserHostAddress,
                GetNotifyUrl(Options.PayNotifyUrl, WeChatNotifyType.Pay), paramter.TradeType, paramter.OpenId, Options.Key,
                nonceStr, paramter.DeviceInfo, paramter.TimeStart, paramter.TimeExpire, GetDetail(paramter), paramter.Attach,
                paramter.FeeType, paramter.GoodsTag, paramter.ProductId, paramter.LimitPay, GetSceneInfo(paramter), paramter.ProfitSharing);

            UnifiedorderResult unfortifiedResult = TenPayV3.Unifiedorder(tenPayV3UnifiedorderRequestData);

            if (unfortifiedResult.return_code != Model.ReturnCode.SUCCESS)
                throw new WeChatServiceException($"微信统一下单失败, return_code: {unfortifiedResult.return_code}, return_msg: {unfortifiedResult.return_msg}.",
                    unfortifiedResult, typeof(UnifiedorderResult));

            string package = $"prepay_id={unfortifiedResult.prepay_id}";

            return new WeChatUnifiedorderResult
            {
                ResultXml = unfortifiedResult.ResultXml,
                DeviceInfo = unfortifiedResult.device_info,
                TradeType = unfortifiedResult.trade_type,
                MWebUrl = unfortifiedResult.mweb_url,
                PrepayId = unfortifiedResult.prepay_id,

                TimeStamp = timeStamp,
                NonceStr = nonceStr,
                Package = package,
                CodeUrl = unfortifiedResult.code_url,
                JsPaySign = TenPayV3.GetJsPaySign(Options.AppId, timeStamp, nonceStr, package, Options.Key)
            };
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public WeChatUnifiedorderResult QRPay(WeChatUnifiedorderParamter paramter)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            return Pay(paramter);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public RefundResult Refund(WeChatRefundParamter paramter)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3RefundRequestData tenPayV3RefundRequestData = new TenPayV3RefundRequestData(Options.AppId,
                Options.MchId, Options.Key, paramter.DeviceInfo, nonceStr, paramter.TransactionId,
                paramter.OutTradeNo, paramter.OutRefundNo, paramter.TotalFee, paramter.RefundFee,
                Options.MchId, paramter.RefundAccount, paramter.RefundDescription,
                GetNotifyUrl(Options.RefundNotifyUrl, WeChatNotifyType.Refund), paramter.RefundFeeType);

            RefundResult refundResult = TenPayV3.Refund(Options.ServiceProvider, tenPayV3RefundRequestData, paramter.TimeOut);
            //,
            //$"{AppDomain.CurrentDomain.BaseDirectory}/apiclient_cert.p12", Options.CertPassword);

            return refundResult;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public RefundQueryResult RefundQuery(WeChatRefundQueryParamter paramter)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            TenPayV3RefundQueryRequestData tenPayV3RefundRequestData = new TenPayV3RefundQueryRequestData(Options.AppId,
                Options.MchId, Options.Key, nonceStr, paramter.DeviceInfo, paramter.TransactionId,
                paramter.OutTradeNo, paramter.OutRefundNo, paramter.RefundId, null, null, paramter.Offset);

            RefundQueryResult refundQueryResult = TenPayV3.RefundQuery(tenPayV3RefundRequestData);

            return refundQueryResult;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public SendTemplateMessageResult SendTemplateMessage(WeChatSendTemplateMessageParamter paramter)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        {
            return TemplateApi.SendTemplateMessage(TryGetAccessToken(), paramter.OpenId, paramter.TemplateId, paramter.Url, paramter.Data, paramter.MiniProgram, paramter.TimeOut);
        }

        #endregion
    }
}
