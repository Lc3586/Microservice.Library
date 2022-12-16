using Microservice.Library.Extension;
using Microservice.Library.WeChat.Application;
using Microservice.Library.WeChat.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Senparc.NeuChar.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信服务财付通通知中间件
    /// <para>V3版本</para>
    /// </summary>
    /// <exception cref="WeChatNotifyException"></exception>
    public class WeChatNotifyV3Middleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="handler"></param>
        public WeChatNotifyV3Middleware(RequestDelegate next, WeChatGenOptions options, IWeChatNotifyHandler handler)
        {
            Next = next;
            Options = options;
            Handler = handler;
            SetUp();
        }

        #region 私有成员

        internal Dictionary<WeChatNotifyType, PathString> UrlDic;

        readonly RequestDelegate Next;
        readonly WeChatGenOptions Options;

        readonly IWeChatNotifyHandler Handler;

        void SetUp()
        {
            try
            {
                UrlDic = new Dictionary<WeChatNotifyType, PathString>
                {
                    {
                        WeChatNotifyType.Pay,

                        new PathString(
                        string.IsNullOrWhiteSpace(Options.WeChatBaseOptions.PayNotifyUrl)
                        ? $"/wechat/notify/{Guid.NewGuid()}"
                        : new Uri(Options.WeChatBaseOptions.PayNotifyUrl).LocalPath)
                    },
                    {
                        WeChatNotifyType.Refund,

                        new PathString(
                        string.IsNullOrWhiteSpace(Options.WeChatBaseOptions.RefundNotifyUrl)
                        ? $"/wechat/notify/{Guid.NewGuid()}"
                        : new Uri(Options.WeChatBaseOptions.RefundNotifyUrl).LocalPath)
                    }
                };
            }
            catch (Exception ex)
            {
                throw new WeChatNotifyException(WeChatPayApiVersion.V3, "中间件启动时发生异常.", ex);
            }
        }

        async Task ResponseJson(HttpContext context, object obj)
        {
            await context.Response.WriteAsync(JsonConvert.SerializeObject(obj));
        }

        async Task ResponseXml(HttpContext context, object obj)
        {
            var xml = obj.ConvertEntityToXmlString();

            //var jsonStr = JsonConvert.SerializeObject(obj);
            //var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, "xml");
            //string xmlDocStr = xmlDoc.InnerXml.Replace("><", ">\r\n<");

            await context.Response.WriteAsync(xml);
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Method.Equals(HttpMethod.Post.Method)
                && context.Request.Path.HasValue)
                {
                    if (UrlDic.ContainsValue(context.Request.Path))
                    {
                        var data = new WeChatNotifyData(Options, WeChatPayApiVersion.V3, WeChatNotifyType.Pay, context.Request);

                        await data.Init().ConfigureAwait(false);

                        switch (UrlDic.First(o => o.Value == context.Request.Path).Key)
                        {
                            case WeChatNotifyType.Pay:
                                var payNotifyReply = await Handler.PayNotify(data.GetPayNotifyInfo()).ConfigureAwait(false);
                                await ResponseJson(context, payNotifyReply).ConfigureAwait(false);
                                return;
                            case WeChatNotifyType.Refund:
                                var refundNotifyReply = await Handler.RefundNotify(data.GetRefundNotifyInfo()).ConfigureAwait(false);
                                await ResponseXml(context, refundNotifyReply).ConfigureAwait(false);
                                return;
                            default:
                                break;
                        }
                    }
                }

                await Next.Invoke(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new WeChatNotifyException(WeChatPayApiVersion.V3, "中间件运行时发生异常.", ex);
            }
        }

        #endregion
    }
}
