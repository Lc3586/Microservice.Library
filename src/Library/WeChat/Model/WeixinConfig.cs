using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 配置
    /// </summary>
    public static class WeixinConfig
    {
        public static string AppId { get; set; }
        public static string Appsecret { get; set; }
        public static string Key { get; set; }
        public static string MchId { get; set; }
        public static string NotifyUrl { get; set; }
        public static string QRNotifyUrl { get; set; }
        public static string RefundNotifyUrl { get; set; }
        public static string UserHostAddress { get; set; }
        public static string DeviceInfo { get; set; }
        public static string OPPassword { get; set; }
        public static string CertPassword { get; set; }
    }
}
