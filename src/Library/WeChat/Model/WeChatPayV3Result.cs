using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信支付V3版本返回数据
    /// </summary>
    public class WeChatPayV3Result
    {
        /// <summary>
        /// 返回状态码
        /// <see cref="ReturnCode"/>
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string return_msg { get; set; }

        /// <summary>
        /// XML内容
        /// </summary>
        public string ResultXml { get; set; }

        /// <summary>
        /// 微信分配的公众账号ID（付款到银行卡接口，此字段不提供）
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        ///微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 子商户公众账号ID
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 子商户号
        /// </summary>
        public string sub_mch_id { get; set; }

        /// <summary>
        /// 随机字符串，不长于32 位
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string err_code_des { get; set; }
    }
}
