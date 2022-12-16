using Microservice.Library.Soap.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using SoapCore.Extensibility;
using SoapCore.ServiceModel;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Library.Soap.Filter
{
    /// <summary>
    /// 消息拦截器
    /// </summary>
    public class MessageInspector2 : IMessageInspector2
    {
        /// <summary>
        /// 自定义输出
        /// </summary>
        public static Dictionary<Type, (Func<HttpContext> HttpContextGetter, string Response)> CustomResponses = new Dictionary<Type, (Func<HttpContext> HttpContextGetter, string Response)>();

        /// <summary>
        /// 接收后
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceDescription"></param>
        /// <returns></returns>
        public object AfterReceiveRequest(ref Message message, ServiceDescription serviceDescription)
        {
            return null;
        }

        /// <summary>
        /// 发送前
        /// </summary>
        /// <param name="reply"></param>
        /// <param name="serviceDescription"></param>
        /// <param name="correlationState"></param>
        public void BeforeSendReply(ref Message reply, ServiceDescription serviceDescription, object correlationState)
        {
            if (!CustomResponses.ContainsKey(serviceDescription.ServiceType))
                return;

            var option = CustomResponses[serviceDescription.ServiceType];

            var httpContext = option.HttpContextGetter?.Invoke();

            if (httpContext == null)
                throw new SoapException($"{serviceDescription.ServiceType.FullName} 服务由于未能获取到HttpContext对象, 所以无法输出自定义响应内容, 请检查SoapServerOptions.HttpContextGetter配置.");

            SetHttpResponse(httpContext, reply);

            var result = string.Empty;

            using (var xmlReader = reply.GetReaderAtBodyContents())
            {
                while (true)
                {
                    if (xmlReader.LocalName.Contains("Result"))
                    {
                        result += xmlReader.ReadInnerXml();
                        break;
                    }
                    else
                        xmlReader.ReadStartElement();
                }
            }

            httpContext.Response.WriteAsync(option.Response.Replace("{{result}}", result)).GetAwaiter().GetResult();

            var complete = httpContext.Response.GetType().GetMethod("CompleteAsync");
            if (complete == null)
                throw new SoapException("HttpResponse对象未包含CompleteAsync方法, 可能是Microsoft.AspNetCore.Http.Abstractions版本过低（低于3.0）.");

            ((Task)complete.Invoke(httpContext.Response, null)).GetAwaiter().GetResult();
        }

        private void SetHttpResponse(HttpContext httpContext, Message message)
        {
            if (!message.Properties.TryGetValue(HttpResponseMessageProperty.Name, out var value)
#pragma warning disable SA1119 // StatementMustNotUseUnnecessaryParenthesis
                || !(value is HttpResponseMessageProperty httpProperty))
#pragma warning restore SA1119 // StatementMustNotUseUnnecessaryParenthesis
            {
                return;
            }

            httpContext.Response.StatusCode = (int)httpProperty.StatusCode;

            var feature = httpContext.Features.Get<IHttpResponseFeature>();
            if (feature != null && !string.IsNullOrEmpty(httpProperty.StatusDescription))
            {
                feature.ReasonPhrase = httpProperty.StatusDescription;
            }

            foreach (string key in httpProperty.Headers.Keys)
            {
                httpContext.Response.Headers.Add(key, httpProperty.Headers.GetValues(key));
            }
        }
    }
}
