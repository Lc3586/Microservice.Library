using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 微信通知优惠类型
    /// </summary>
    public static class PromotionType
    {
        /// <summary>
        /// 充值 
        /// </summary>
        [Description("充值")]
        public const string CASH = "CASH";

        /// <summary>
        /// 预充值 
        /// </summary>
        [Description("预充值")]
        public const string NOCASH = "NOCASH";
    }
}
