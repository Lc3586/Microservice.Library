using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信返回状态码
    /// </summary>
    public static class ReturnCode
    {
        /// <summary>
        /// 通信成功
        /// </summary>
        [Description("成功")]
        public const string SUCCESS = "SUCCESS";

        /// <summary>
        /// 通信失败
        /// </summary>
        [Description("失败")]
        public const string FAIL = "FAIL";
    }
}
