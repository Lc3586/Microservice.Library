using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 微信通知优惠范围
    /// </summary>
    public static class PromotionScope
    {
        /// <summary>
        /// 全场代金券 
        /// </summary>
        [Description("全场代金券")]
        public const string GLOBAL = "GLOBAL";

        /// <summary>
        /// 单品优惠 
        /// </summary>
        [Description("单品优惠")]
        public const string SINGLE = "SINGLE";
    }
}
