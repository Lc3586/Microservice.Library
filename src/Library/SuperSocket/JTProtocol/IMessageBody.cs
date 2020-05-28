using System;
using System.Collections.Generic;
using System.Text;

namespace Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// 数据体
    /// </summary>
    public interface IMessageBody
    {
        /// <summary>
        /// 元数据
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 转字符串（使用换行符）
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
