using System;

namespace Microservice.Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// 消息包信息
    /// </summary>
    public class MessagePackageInfo
    {
        public MessagePackageInfo()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">十六进制数据</param>
        public MessagePackageInfo(byte[] buffer)
        {
            Buffer = buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionFlag">协议版本号标识</param>
        /// <param name="gnsscenterID">下级平台接入码</param>
        /// <param name="body">消息体</param>
        /// <param name="encryptFlag">是否加密</param>
        public MessagePackageInfo(byte[] versionFlag, UInt32 gnsscenterID, IMessageBody body, bool encryptFlag = false)
        {
            MessageHeader = new MessageHeader()
            {
                MsgGnsscenterID = gnsscenterID,
                VersionFlag = versionFlag,
                EncryptFlag = encryptFlag
            };
            MessageBody = body;
        }

        /// <summary>
        /// 元数据
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 头标识
        /// </summary>
        public byte[] HeadFlag { get; set; }

        /// <summary>
        /// 数据头
        /// </summary>
        public MessageHeader MessageHeader { get; set; }

        /// <summary>
        /// 数据体
        /// </summary>
        public IMessageBody MessageBody { get; set; }

        /// <summary>
        /// 数据CRC校验码
        /// 从数据头到校验码前的 CRC-CCITT 的校验值
        /// </summary>
        public byte[] CRCCode { get; set; }

        /// <summary>
        /// 尾标识
        /// </summary>
        public byte[] EndFlag { get; set; }

        /// <summary>
        /// 是否成功解析
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 消息包转字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MessageBody?.ToString();
        }

        public byte[] GetBytes()
        {
            return Buffer;
        }
    }
}
