using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.SuperSocket.Model
{
    /// <summary>
    /// 会话客户端类型
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// 客户端
        /// </summary>
        client,
        /// <summary>
        /// 服务器
        /// </summary>
        server,
        /// <summary>
        /// 调试客户端
        /// </summary>
        debugClient
    }
}
