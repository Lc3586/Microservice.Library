using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Library.Configuration;
using Library.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using UnitTest.Extension;

namespace UnitTest.Config
{
    /// <summary>
    /// Soap测试数据
    /// </summary>
    public class SoapConfig
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public static SoapConfig GetData()
        {
            return new ConfigHelper("jsonconfig/soap.json").GetModel<SoapConfig>("Datas");
        }

        /// <summary>
        /// WGV1_7  区平台
        /// </summary>
        public WGSoapTestSetting WGV1_7 { get; set; }

        /// <summary>
        /// WGV3_2  市平台
        /// </summary>
        public WGSoapTestSetting WGV3_2 { get; set; }
    }

    /// <summary>
    /// WG测试设置
    /// </summary>
    public class WGSoapTestSetting
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 请求主体
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string XmlStringTemplate { get; set; }

        /// <summary>
        /// 请求设置
        /// </summary>
        public Dictionary<string, List<WGSoapTestRequestSetting>> Request { get; set; }

        /// <summary>
        /// 获取请求设置集合数量
        /// </summary>
        /// <param name="key">标识</param>
        public int GetRequestListCount(string key)
        {
            return Request[key].Count;
        }

        /// <summary>
        /// 获取用于请求的xml字符串
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public string GetRequestXmlString(string key, int index)
        {
            Assert.IsTrue(
                Request.ContainsKey(key),
                $"请求设置中未包含业务号 {key}.");

            var setting = Request[key][index];

            var xmlString = XmlStringTemplate
                .Replace("{{Userid}}", setting.Userid)
                .Replace("{{Password}}", setting.Password)
                .Replace("{{TransNo}}", setting.TransNo)
                .Replace("{{Paramters}}", string.Join("", setting.Paramters?.Select(p => $"<{p.Key}>{p.Value}</{p.Key}>") ?? new List<string>()));

            return RequestBody
                .Replace("{{XmlString}}", HttpUtility.HtmlEncode(xmlString));
        }

        /// <summary>
        /// 检查输出信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index">索引</param>
        /// <param name="response"></param>
        public void CheckResponse(string key, int index, JObject response)
        {
            var request = Request[key][index];
            if (!request.SuccessResponse.Any_Ex())
                return;

            request.SuccessResponse.ForEach(r =>
                Assert.AreEqual(
                        r.Value,
                        response.Value<string>(r.Key),
                        $"{key}[{index}] {r.Key} 业务返回值和指定值不一致."));
        }
    }

    /// <summary>
    /// WG请求设置
    /// </summary>
    public class WGSoapTestRequestSetting
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Userid { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 业务号
        /// </summary>
        public string TransNo { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Paramters { get; set; }

        /// <summary>
        /// 应该返回的信息
        /// </summary>
        public Dictionary<string, object> SuccessResponse { get; set; }
    }
}
