using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 支付场景信息描述
    /// </summary>
    public class SceneInfo
    {
        /// <summary>
        /// 商户端设备号
        /// </summary>
        /// <remarks>终端设备号（门店号或收银设备ID）。</remarks>
        public string device_id { get; set; }
    }
}
