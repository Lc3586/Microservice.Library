using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 终端类型
    /// </summary>
    public enum TerminalType
    {
        /// <summary>
        /// 无
        /// </summary>
        none,
        /// <summary>
        /// 安卓
        /// </summary>
        android = 1,
        /// <summary>
        /// 苹果
        /// </summary>
        ios,
        /// <summary>
        /// 微信
        /// </summary>
        wechat,
        /// <summary>
        /// 后台网页
        /// </summary>
        web_admin,
        /// <summary>
        /// 前台网页
        /// </summary>
        web_web
    }
}
