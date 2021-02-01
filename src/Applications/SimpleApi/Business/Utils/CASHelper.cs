using Library.Extension;
using Library.Models;
using Library.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using Model.System;
using Library.Container;
using Model.System.Config;

namespace Business.Utils
{
    /// <summary>
    /// CAS帮助类
    /// </summary>
    public class CASHelper
    {
        static readonly SystemConfig Config = AutofacHelper.GetScopeService<SystemConfig>();

        /// <summary>
        /// 获取TGT
        /// </summary>
        /// <param name="getTGT"></param>
        /// <returns></returns>
        public static async Task<string> GetTGT(CASModel.GetTGT getTGT)
        {
            (HttpStatusCode, string) response = HttpHelper.PostDataWithState(Config.CASTGTUrl, new Dictionary<string, object>() { { "username", getTGT.Username }, { "password", getTGT.Password } });
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
        public static async Task<string> DeleteTGT(CASModel.LogOut logOut)
        {
            (HttpStatusCode, string) response = HttpHelper.RequestData(HttpMethod.Delete, string.Format(Config.CASDeleteSTUrl, logOut.TGT));
            if (response.Item1 != HttpStatusCode.OK)
                throw new MessageException("注销失败");
            return await Task.FromResult(response.Item2);
        }

        /// <summary>
        /// 获取ST
        /// </summary>
        /// <param name="getST"></param>
        /// <returns></returns>
        public static async Task<string> GetST(CASModel.GetST getST)
        {
            (HttpStatusCode, string) response = HttpHelper.PostDataWithState(string.Format(Config.CASSTUrl + "?service={1}", getST.TGT, getST.Service ?? Config.WebRootUrl));
            if (response.Item1 != HttpStatusCode.OK)
                throw new MessageException(response.Item2);
            return await Task.FromResult(response.Item2);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="getUserInfo"></param>
        /// <returns></returns>
        public static async Task<CASModel.UserInfo> GetUserInfo(CASModel.GetUserInfo getUserInfo)
        {
            (HttpStatusCode, string) response = HttpHelper.GetDataWithState(Config.CASUserInfoUrl, new Dictionary<string, object>() { { "service", getUserInfo.Service ?? Config.WebRootUrl }, { "ticket", getUserInfo.ST } });
            if (response.Item1 != HttpStatusCode.OK)
                throw new MessageException("验证失败", ErrorCode.validation);
            if (!GetUserInfo(response.Item2, out CASModel.UserInfo userInfo))
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
            tgt = attr.Value.Substring(attr.Value.LastIndexOf('/') + 1);
            return true;
        }

        /// <summary>
        /// 解析xml以获取用户信息
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        private static bool GetUserInfo(string xml, out CASModel.UserInfo userInfo)
        {
            userInfo = new CASModel.UserInfo();
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}