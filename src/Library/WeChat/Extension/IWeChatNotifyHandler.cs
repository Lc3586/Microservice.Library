using Microservice.Library.WeChat.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Library.WeChat.Extension
{
    /// <summary>
    /// 微信财付通通知处理接口
    /// </summary>
    public interface IWeChatNotifyHandler
    {
        /// <summary>
        /// 付款通知
        /// </summary>
        /// <param name="notifyInfo">通知信息</param>
        /// <returns>通知应答信息</returns>
        Task<PayNotifyReply> PayNotify(PayNotifyInfo notifyInfo);

        /// <summary>
        /// 退款通知
        /// </summary>
        /// <param name="notifyInfo">通知信息</param>
        /// <returns>通知应答信息</returns>
        Task<RefundNotifyReply> RefundNotify(RefundNotifyInfo notifyInfo);
    }
}
