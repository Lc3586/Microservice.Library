using Microservice.Library.WeChat.Application;
using Microservice.Library.WeChat.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Senparc.CO2NET.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信财付通通知数据
    /// </summary>
    /// <exception cref="WeChatServiceException"></exception>
    /// <remarks></remarks>
    public class WeChatNotifyData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options">微信服务配置</param>
        /// <param name="version">微信支付接口版本</param>
        /// <param name="notifyType">微信财付通通知类型</param>
        /// <param name="request">请求信息</param>
        public WeChatNotifyData(WeChatGenOptions options, WeChatPayApiVersion version, WeChatNotifyType notifyType, HttpRequest request)
        {
            NotifyType = notifyType;
            Version = version;
            Options = options;
            Request = request;
        }

        #region 私有成员

        /// <summary>
        /// 微信支付接口版本
        /// </summary>
        public WeChatPayApiVersion Version;

        /// <summary>
        /// 微信财付通通知类型
        /// </summary>
        readonly WeChatNotifyType NotifyType;

        /// <summary>
        /// 微信服务配置
        /// </summary>
        readonly WeChatGenOptions Options;

        readonly HttpRequest Request;

        string Body;

        SecurityHelper Security { get; set; }

        /// <summary>
        /// 获取请求中的签名
        /// </summary>
        /// <returns>base64字符串</returns>
        string GetRequestSign()
        {
            if (!Request.Headers.ContainsKey(Options.WeChatDevOptions.RequestHeaderSign))
                throw new WeChatNotifyException(Version, $"获取签名失败，请求头中未包含: {Options.WeChatDevOptions.RequestHeaderSign}.");

            return Request.Headers[Options.WeChatDevOptions.RequestHeaderSign];
        }

        /// <summary>
        /// 检查签名并读取报文内容
        /// </summary>
        async Task CheckSignAndLoadXml()
        {
            if (!Request.Headers.ContainsKey(Options.WeChatDevOptions.RequestHeaderTimestamp))
                throw new WeChatNotifyException(Version, $"验证签名失败，请求头中未包含: {Options.WeChatDevOptions.RequestHeaderTimestamp}.");

            var timestamp = Request.Headers[Options.WeChatDevOptions.RequestHeaderTimestamp];

            if (!Request.Headers.ContainsKey(Options.WeChatDevOptions.RequestHeaderNonce))
                throw new WeChatNotifyException(Version, $"验证签名失败，请求头中未包含: {Options.WeChatDevOptions.RequestHeaderNonce}.");

            var nonce = Request.Headers[Options.WeChatDevOptions.RequestHeaderNonce];

            using (var reader = new StreamReader(Request.Body))
                Body = await reader.ReadToEndAsync();

            IsEmpty = Body.Length == 0;
            //throw new WeChatNotifyException(NotifyType, "验证签名失败: 通知报文内容为空.");

            var requestSign = GetRequestSign();

            var data = $"{timestamp}\n{nonce}\n";

            if (Body.Length != 0)
                data += $"{Body}\n";

            var sign = Security.GetSignBase64WithSHA256_RSA(data);

            if (!requestSign.Equals(sign))
                throw new WeChatNotifyException(Version, $"验证签名失败: 请求签名 = > [{requestSign}] <> 实际签名 => [{sign}].");
        }

        /// <summary>
        /// 获取Xml数据哈希表
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        Hashtable GetXmlMap(string xmlString)
        {
            var document = new XmlDocument();
            document.Load(xmlString);

            var xmlMap = new Hashtable();

            var root = document.FirstChild;
            var xnl = root.ChildNodes;

            foreach (XmlNode xnf in xnl)
            {
                xmlMap.Add(xnf.Name, xnf.InnerText);
            }

            return xmlMap;
        }

        /// <summary>
        /// 读取哈希表数据并写入指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlMap"></param>
        void SetValueFromXmlMap<T>(T obj, Hashtable xmlMap)
        {
            var propertys = typeof(T).GetType().GetProperties();

            foreach (var property in propertys)
            {
                if (xmlMap.ContainsKey(property.Name))
                    property.SetValue(obj, xmlMap[property.Name]);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 报文内容是否为空
        /// </summary>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            Security = new SecurityHelper(Options);
            await CheckSignAndLoadXml();
        }

        /// <summary>
        /// 获取支付通知信息
        /// </summary>
        public PayNotifyInfo GetPayNotifyInfo()
        {
            var data = JsonConvert.DeserializeObject<PayNotifyInfo>(Body);

            if (data.resource != null)
            {
                string resourceData;

                switch (data.resource.algorithm)
                {
                    case AlgorithmType.AEAD_AES_256_GCM:
                        resourceData = Security.DecryptWithAES_256_GCM
                            (
                                data.resource.ciphertext,
                                data.resource.nonce,
                                data.resource.associated_data
                            );
                        break;
                    default:
                        throw new WeChatNotifyException(Version, $"不支持的加密类型: {data.resource.algorithm}");
                }

                data.resource.Data = JsonConvert.DeserializeObject<PayNotifyResourceInfo>(resourceData);
            }

            return data;
        }

        /// <summary>
        /// 获取退款通知信息
        /// </summary>
        public RefundNotifyInfo GetRefundNotifyInfo()
        {
            var xmlMap = GetXmlMap(Body);

            var data = new RefundNotifyInfo();

            SetValueFromXmlMap(data, xmlMap);

            if (!string.IsNullOrWhiteSpace(data.req_info))
            {
                var req_info = Security.DecryptWithAES_256_ECB(data.req_info);

                var reqInfoXmlMap = GetXmlMap(req_info);

                data.ReqInfo = new RefundNotifyReqInfo();

                SetValueFromXmlMap(data.ReqInfo, reqInfoXmlMap);
            }

            return data;
        }

        #endregion
    }
}
