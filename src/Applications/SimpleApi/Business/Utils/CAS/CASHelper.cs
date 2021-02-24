using HtmlAgilityPack;
using Library.Container;
using Library.Extension;
using Library.Http;
using Model.Utils.CAS.CASDTO;
using Model.Utils.Config;
using Model.Utils.Result;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Business.Utils.CAS
{
    /// <summary>
    /// CAS帮助类
    /// </summary>
    public static class CASHelper
    {
        static readonly SystemConfig Config = AutofacHelper.GetScopeService<SystemConfig>();

        /// <summary>
        /// 获取TGT
        /// </summary>
        /// <param name="getTGT"></param>
        /// <returns></returns>
        public static async Task<string> GetTGT(GetTGT getTGT)
        {
            (HttpStatusCode, string) response = HttpHelper.PostDataWithState(Config.CAS.TGTUrl, new Dictionary<string, object>() { { "username", getTGT.Username }, { "password", getTGT.Password } });
            if (response.Item1 != HttpStatusCode.Created)
                throw new MessageException("用户名或密码有误");
            if (!GetTGT(response.Item2, out string tgt))
                throw new MessageException("系统繁忙");
            return await Task.FromResult(tgt);
        }

        /// <summary>
        /// 删除TGT
        /// </summary>
        /// <param name="logOut"></param>
        /// <returns></returns>
        public static async Task<string> DeleteTGT(LogOut logOut)
        {
            (HttpStatusCode, string) response = HttpHelper.RequestData(HttpMethod.Delete, string.Format(Config.CAS.DeleteSTUrl, logOut.TGT));
            if (response.Item1 != HttpStatusCode.OK)
                throw new MessageException("注销失败");
            return await Task.FromResult(response.Item2);
        }

        /// <summary>
        /// 获取ST
        /// </summary>
        /// <param name="getST"></param>
        /// <returns></returns>
        public static async Task<string> GetST(GetST getST)
        {
            (HttpStatusCode, string) response = HttpHelper.PostDataWithState(string.Format(Config.CAS.STUrl + "?service={1}", getST.TGT, getST.Service ?? Config.WebRootUrl));
            if (response.Item1 != HttpStatusCode.OK)
                throw new MessageException(response.Item2);
            return await Task.FromResult(response.Item2);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="getUserInfo"></param>
        /// <returns></returns>
        public static async Task<UserInfo> GetUserInfo(GetUserInfo getUserInfo)
        {
            (HttpStatusCode, string) response = HttpHelper.GetDataWithState(Config.CAS.UserInfoUrl, new Dictionary<string, object>() { { "service", getUserInfo.Service ?? Config.WebRootUrl }, { "ticket", getUserInfo.ST } });
            if (response.Item1 != HttpStatusCode.OK)
                throw new MessageException("验证失败", ErrorCode.validation);
            if (!GetUserInfo(response.Item2, out UserInfo userInfo))
                throw new MessageException("系统繁忙");
            return await Task.FromResult(userInfo);
        }

        /// <summary>
        /// 解析html以获取tgt
        /// </summary>
        /// <param name="html">html字符串</param>
        /// <param name="tgt">tgt</param>
        /// <returns></returns>
        private static bool GetTGT(string html, out string tgt)
        {
            tgt = string.Empty;
            if (html.IsNullOrWhiteSpace())
                return false;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectSingleNode("//form");
            if (node == null)
                return false;
            var attr = node.Attributes["action"];
            if (attr == null)
                return false;
            tgt = attr.Value[(attr.Value.LastIndexOf('/') + 1)..];
            return true;
        }

        /// <summary>
        /// 解析xml以获取用户信息
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        private static bool GetUserInfo(string xml, out UserInfo userInfo)
        {
            userInfo = new UserInfo();
            if (xml.IsNullOrWhiteSpace())
                return false;
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var nameSpaceManager = new XmlNamespaceManager(doc.NameTable);
                nameSpaceManager.AddNamespace("cas", "http://www.yale.edu/tp/cas");
                if (doc.SelectSingleNode("//cas:authenticationSuccess", nameSpaceManager) == null)
                    return false;
                userInfo.name = doc.SelectSingleNode("//cas:name", nameSpaceManager)?.InnerText;
                userInfo.id = doc.SelectSingleNode("//cas:id", nameSpaceManager)?.InnerText;
                userInfo.account = doc.SelectSingleNode("//cas:account", nameSpaceManager)?.InnerText;
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
            {
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}