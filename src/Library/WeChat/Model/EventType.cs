using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信通知类型
    /// </summary>
    public static class EventType
    {
        /// <summary>
        /// 支付成功
        /// </summary>
        [Description("支付成功")]
        public const string TRANSACTION_SUCCESS = "TRANSACTION.SUCCESS";
    }
}
