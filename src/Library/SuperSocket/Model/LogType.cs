using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.SuperSocket.Model
{
    public enum LogType
    {
        系统,
        连接,
        断开,
        验证成功,
        验证失败,
        黑名单,
        解包,
        解码,
        处理,
        编码,
        发送,
        接收,
        入库
    }
}
