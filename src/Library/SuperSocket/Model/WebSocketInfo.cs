using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 套接字信息
    /// </summary>
    public class WebSocketInfo
    {
        /// <summary>
        /// 客户端的 IP 地址
        /// </summary>
        public string UserHostAddress { get; set; }

        /// <summary>
        /// 客户端的 DNS 名称
        /// </summary>
        public string UserHostName { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectedTime { get; set; }

        /// <summary>
        /// 验证状态
        /// </summary>
        public bool Verification { get; set; }

        /// <summary>
        /// 验证失败次数
        /// </summary>
        public int VerificationfailureTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PWD { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public TerminalType TerminalType { get; set; }

        /// <summary>
        /// 连接对象
        /// </summary>
        public System.Net.WebSockets.WebSocket socket { get; set; }

        /// <summary>
        /// 在线状态通知对象UID集合
        /// </summary>

        public List<string> OnlineStateNotificationUIDList { get; set; }

        /// <summary>
        /// 连接对象类型（0:默认,1:sharp）
        /// </summary>
        public byte socketType { get; set; }

        /// <summary>
        /// 当前登录用户名
        /// </summary>
        public string IdentityName { get; set; }

        /// <summary>
        /// 当前连接对象登录状态
        /// </summary>
        public bool IsAuthenticated { get; set; }
    }
}
