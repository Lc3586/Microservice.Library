using System;
using System.Collections.Generic;
using System.Text;

namespace Library.WeChat.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class WeixinUserInfo
    {
        /// <summary>
        /// 应用下用户唯一标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 用户的性别
        /// </summary>
        /// <remarks>
        /// 值为1时是男性，
        /// 值为2时是女性，
        /// 值为0时是未知
        /// </remarks>
        public int sex { get; set; }

        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 国家，
        /// </summary>
        /// <remarks>
        /// CN为中国
        /// </remarks>
        public string country { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        /// <remarks>
        /// 最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），
        /// 用户没有头像时该项为空
        /// </remarks>
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户特权信息
        /// </summary>
        /// <remarks>
        /// 如微信沃卡用户为（chinaunicom）
        /// </remarks>
        public string[] privilege { get; set; }

        /// <summary>
        /// 开放平台用户唯一标识
        /// </summary>
        public string unionid { get; set; }
    }
}
