using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信财付通通知类型
    /// </summary>
    public enum WeChatNotifyType
    {
        /// <summary>
        /// 付款
        /// </summary>
        Pay,
        /// <summary>
        /// 退款
        /// </summary>
        Refund
    }
}
