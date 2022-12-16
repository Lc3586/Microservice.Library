using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.WeChat.Model
{
    /// <summary>
    /// 场景信息
    /// </summary>
    public class WeChatUnifiedorderSceneInnfo
    {
        /// <summary>
        /// 是否为H5支付
        /// </summary>
        public bool IsH5Pay { get; set; }

        /// <summary>
        /// 当IsH5Pay为true时填写，
        /// 可以使用<see cref="TenPayV3UnifiedorderRequestData_SceneInfo.GetH5InfoInstance"/>方法获得
        /// </summary>
        public IH5_Info H5_Info { get; set; }

        /// <summary>
        /// （非必填）门店id，门店唯一标识，
        /// String(32)
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// （非必填）门店名称，
        /// String(64)
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// （非必填）门店行政区划码，新县及县以上行政区划代码》：
        /// https://pay.weixin.qq.com/wiki/doc/api/download/store_adress.csv
        /// </summary>
        public string area_code { get; set; }

        /// <summary>
        /// （非必填）门店详细地址，
        /// String(128)
        /// </summary>
        public string address { get; set; }
    }
}
