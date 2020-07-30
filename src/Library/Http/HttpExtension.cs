using Library.JWT;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Library.Http
{
    public static class HttpExtension
    {
        public static bool IsAjaxRequest(this HttpRequestMessage req)
        {
            bool result = false;

            var xreq = req.Headers.Contains("x-requested-with");
            if (xreq)
            {
                result = req.Headers.GetValues("x-requested-with").FirstOrDefault() == "XMLHttpRequest";
            }

            return result;
        }

        /// <summary>
        /// 是否拥有某过滤器
        /// </summary>
        /// <typeparam name="T">过滤器类型</typeparam>
        /// <param name="actionExecutingContext">上下文</param>
        /// <returns></returns>
        public static bool ContainsFilter<T>(this ActionExecutingContext actionExecutingContext)
        {
            return actionExecutingContext.Filters != null && actionExecutingContext.Filters.Any(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// 判断是否为AJAX请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest req)
        {
            bool result = false;

            var xreq = req.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = req.Headers["x-requested-with"] == "XMLHttpRequest";
            }

            return result;
        }

        /// <summary>
        /// 获取去掉查询参数的Url
        /// </summary>
        /// <param name="req">请求</param>
        /// <returns></returns>
        public static string GetDisplayUrlNoQuery(this HttpRequest req)
        {
            var queryStr = req.QueryString.ToString();
            var displayUrl = req.GetDisplayUrl();

            return string.IsNullOrEmpty(queryStr) ? displayUrl : displayUrl.Replace(queryStr, "");
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="req">请求</param>
        /// <returns></returns>
        public static string GetToken(this HttpRequest req)
        {
            string tokenHeader = req.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(tokenHeader))
                throw new Exception("缺少token!");

            string pattern = "^Bearer (.*?)$";
            if (!Regex.IsMatch(tokenHeader, pattern))
                throw new Exception("token格式不对!格式为:Bearer {token}");

            string token = Regex.Match(tokenHeader, pattern).Groups[1]?.ToString();
            if (string.IsNullOrEmpty(token))
                throw new Exception("token不能为空!");

            return token;
        }

        /// <summary>
        /// 获取Token中的Payload
        /// </summary>
        /// <param name="req">请求</param>
        /// <returns></returns>
        public static JWTPayload GetJWTPayload(this HttpRequest req)
        {
            string token = req.GetToken();
            var payload = JWTHelper.GetPayload<JWTPayload>(token);

            return payload;
        }
    }
}
