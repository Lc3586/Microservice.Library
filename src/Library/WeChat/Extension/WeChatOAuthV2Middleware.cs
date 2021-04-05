using Microservice.Library.Extension;
using Microservice.Library.Http;
using Microservice.Library.WeChat.Application;
using Microservice.Library.WeChat.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信网页授权中间件
    /// </summary>
    /// <exception cref="WeChatOAuthException"></exception>
    public class WeChatOAuthV2Middleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="handler"></param>
        /// <param name="loggerProvider"></param>
        public WeChatOAuthV2Middleware(RequestDelegate next, WeChatGenOptions options, IWeChatOAuthHandler handler, ILoggerProvider loggerProvider)
        {
            Next = next;
            Options = options;
            Handler = handler;
            OAuthBaseRedirectUri = new PathString($"/{Guid.NewGuid().ToString("N")}");
            OAuthUserInfoRedirectUri = new PathString($"/{Guid.NewGuid().ToString("N")}");
            Logger = loggerProvider.CreateLogger(nameof(WeChatOAuthV2Middleware));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        #region 私有成员

        readonly RequestDelegate Next;
        readonly WeChatGenOptions Options;
        readonly IWeChatOAuthHandler Handler;
        readonly PathString OAuthBaseRedirectUri;
        readonly PathString OAuthUserInfoRedirectUri;
        readonly ILogger Logger;

        void RedirectToAuthorizeUrl(HttpContext context, string redirect_uri, string scope)
        {
            context.Response.Redirect($"{Options.WeChatOAuthOptions.AuthorizeUrl}" +
                $"?appid={Options.WeChatBaseOptions.AppId}" +
                $"&redirect_uri={UrlEncoder.Default.Encode($"{Options.WeChatOAuthOptions.WebRootUrl}{redirect_uri}") }" +
                $"&response_type=code" +
                $"&scope={scope}" +
                $"&state={(context.Request.Query.ContainsKey("state") ? context.Request.Query["state"].ToString() : Guid.NewGuid().ToString("N"))}" +
                $"#wechat_redirect");
        }

        OAuthAccessTokenReply GetAccessToken(string code, string grant_type)
        {
            var url = $"{Options.WeChatOAuthOptions.AccessTokenUrl}" +
                      $"?appid={Options.WeChatBaseOptions.AppId}" +
                      $"&secret={Options.WeChatBaseOptions.Appsecret}" +
                      $"&code={code}" +
                      $"&grant_type={grant_type}";

            (HttpStatusCode status, string response) = HttpHelper.GetDataWithState(url);

            Logger.LogDebug($"请求微信接口, \r\n\turl: {url}, \r\n\tHttpStatusCode: {status}, \r\n\tResponse: {response}");

            if (status != HttpStatusCode.OK)
                throw new ApplicationException($"微信接口请求失败, \r\n\turl: {url}, \r\n\tHttpStatusCode {status}.");

            var result = response.ToJObject();
            if (result.ContainsKey("errcode"))
                throw new ApplicationException($"微信接口返回异常, \r\n\turl: {url}, \r\n\tResponse {response}.");

            return result.ToObject<OAuthAccessTokenReply>();
        }

        OAuthUserInfo GetUserInfo(string access_token, string openid)
        {
            var url = $"{Options.WeChatOAuthOptions.UserInfoUrl}" +
                      $"?access_token={access_token}" +
                      $"&openid={openid}" +
                      $"&lang={Options.WeChatOAuthOptions.Language}";

            (HttpStatusCode status, string response) = HttpHelper.GetDataWithState(url);

            Logger.LogDebug($"微信接口返回数据, \r\n\turl: {url}, \r\n\tHttpStatusCode: {status}, \r\n\tResponse: {response}");

            if (status != HttpStatusCode.OK)
                throw new ApplicationException($"微信接口请求失败, \r\n\turl: {url}, \r\n\tHttpStatusCode {status}.");

            var bytes_iso_8859_1 = response.ToBytes(Encoding.GetEncoding("ISO-8859-1"));
            var char_utf_8 = new char[Encoding.UTF8.GetCharCount(bytes_iso_8859_1, 0, bytes_iso_8859_1.Length)];
            Encoding.UTF8.GetChars(bytes_iso_8859_1, 0, bytes_iso_8859_1.Length, char_utf_8, 0);
            var string_utf_8 = new string(char_utf_8);

            Logger.LogDebug($"微信接口返回数据解码, \r\n\tResponse: {string_utf_8}");

            var result = string_utf_8.ToJObject();

            if (result.ContainsKey("errcode"))
                throw new ApplicationException($"微信接口返回异常, \r\n\turl: {url}, \r\n\tResponse {string_utf_8}.");

            return result.ToObject<OAuthUserInfo>();
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
                if (context.Request.Method.Equals(System.Net.Http.HttpMethod.Get.Method)
                    && context.Request.Path.HasValue)
                {
                    if (context.Request.Path.Equals(Options.WeChatOAuthOptions.OAuthBaseUrl))
                    {
                        RedirectToAuthorizeUrl(context, OAuthBaseRedirectUri, "snsapi_base");
                        return;
                    }
                    else if (context.Request.Path.Equals(Options.WeChatOAuthOptions.OAuthUserInfoUrl))
                    {
                        RedirectToAuthorizeUrl(context, OAuthUserInfoRedirectUri, "snsapi_userinfo");
                        return;
                    }
                    else if (context.Request.Path.Equals(OAuthBaseRedirectUri))
                    {
                        var code = context.Request.Query["code"].ToString();
                        var result = GetAccessToken(code, "authorization_code");

                        await Handler.Handler(
                                context,
                                Options.WeChatBaseOptions.AppId,
                                result.openid,
                                result.scope,
                                context.Request.Query.ContainsKey("state") ? context.Request.Query["state"].ToString() : null
                            ).ConfigureAwait(false);

                        return;
                    }
                    else if (context.Request.Path.Equals(OAuthUserInfoRedirectUri))
                    {
                        var code = context.Request.Query["code"].ToString();
                        var result = GetAccessToken(code, "authorization_code");

                        var userinfo = GetUserInfo(result.access_token, result.openid);

                        await Handler.Handler(
                                context,
                                Options.WeChatBaseOptions.AppId,
                                userinfo,
                                context.Request.Query.ContainsKey("state") ? context.Request.Query["state"].ToString() : null
                            ).ConfigureAwait(false);

                        return;
                    }
                }

                await Next.Invoke(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new WeChatOAuthException(OAuthVersion.V2_0, "中间件运行时发生异常.", ex);
            }
        }

        #endregion
    }
}
