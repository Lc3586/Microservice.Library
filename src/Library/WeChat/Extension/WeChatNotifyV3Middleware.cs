using Library.WeChat.Application;
using Library.WeChat.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace Library.WeChat.Extension
{
    /// <summary>
    /// 微信服务财付通通知中间件
    /// <para>V3版本</para>
    /// </summary>
    public class WeChatNotifyV3Middleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="handler"></param>
        /// <param name="options"></param>
        public WeChatNotifyV3Middleware(RequestDelegate next, IWeChatNotifyHandler handler, WeChatServiceOptions options)
        {
            Next = next;
            Handler = handler;
            Options = options;
            SetUp();
        }

        #region 私有成员

        internal Dictionary<WeChatNotifyType, PathString> UrlDic;

        readonly RequestDelegate Next;
        readonly WeChatServiceOptions Options;

        readonly IWeChatNotifyHandler Handler;

        void SetUp()
        {
            UrlDic = new Dictionary<WeChatNotifyType, PathString>
            {
                {
                    WeChatNotifyType.Pay,

                    new PathString(
                    string.IsNullOrWhiteSpace(Options.PayNotifyUrl)
                    ? $"/wechat/notify/{Guid.NewGuid()}"
                    : new Uri(Options.PayNotifyUrl).LocalPath)
                },
                {
                    WeChatNotifyType.Refund,

                    new PathString(
                    string.IsNullOrWhiteSpace(Options.RefundNotifyUrl)
                    ? $"/wechat/notify/{Guid.NewGuid()}"
                    : new Uri(Options.RefundNotifyUrl).LocalPath)
                }
            };
        }

        async Task ResponseJson(HttpContext context, object obj)
        {
            await context.Response.WriteAsync(JsonConvert.SerializeObject(obj));
        }

        async Task ResponseXml(HttpContext context, object obj)
        {
            var jsonStr = JsonConvert.SerializeObject(obj);

            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, "xml");

            string xmlDocStr = xmlDoc.InnerXml.Replace("><", ">\r\n<");

            await context.Response.WriteAsync(xmlDocStr);
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
            if (context.Request.Method.Equals(HttpMethod.Post.Method)
                && context.Request.Path.HasValue)
            {
                if (UrlDic.ContainsValue(context.Request.Path))
                {
                    var data = new WeChatNotifyData(Options, WeChatNotifyType.Pay, context.Request);

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

        #endregion
    }
}
