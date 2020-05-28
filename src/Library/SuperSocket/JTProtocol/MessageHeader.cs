using System;
using System.Collections.Generic;
using System.Text;

namespace Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// 消息头
    /// </summary>
    public class MessageHeader
    {
        public MessageHeader()
        {

        }

        /// <summary>
        /// 元数据
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// 数据长度（包括头标识、数据头、数据体和尾标识）
        /// </summary>
        public UInt32 MsgLength { get; set; }

        /// <summary>
        /// 报文序列号
        /// </summary>
        public UInt32 Msg_SN
        {
            get
            {
                return Msg_SN_32 == default ? Msg_SN_16 : Msg_SN_32;
            }
            set
            {
                Msg_SN_32 = value;
                if (value < UInt16.MaxValue)
                    Msg_SN_16 = (UInt16)value;
            }
        }

        /// <summary>
        /// 报文序列号
        /// <para>占用四个字节，为发送信息的序列号，用于接收方检测是否有信息的丢失。上级平台和下级平台按自己发送数据包的个数计数，互不影响。程序开始运行时等于零，发送第一帧数据时开始计数，到最大数后自动归零</para>
        /// </summary>
        public UInt32 Msg_SN_32 { get; set; }

        /// <summary>
        /// 报文序列号
        /// <para>占用二个字节，为发送信息的序列号，用于接收方检测是否有信息的丢失。上级平台和下级平台按自己发送数据包的个数计数，互不影响。程序开始运行时等于零，发送第一帧数据时开始计数，到最大数后自动归零</para>
        /// </summary>
        public UInt16 Msg_SN_16 { get; set; }

        /// <summary>
        /// 消息体属性
        /// <para>占用二个字节,解析为二进制字符串</para>
        /// </summary>
        public string Msg_Attribute { get; set; }

        /// <summary>
        /// 协议号/业务数据类型
        /// </summary>
        public UInt16 Msg_ID { get; set; }

        /// <summary>
        /// 下级平台接入码，上级平台给下级平台分配的唯一标识号
        /// </summary>
        public UInt32 MsgGnsscenterID { get; set; }

        /// <summary>
        /// 协议版本号标识，上下级平台之间采用的标准协议版本编号；
        /// 长度为 3 个字节来表示：0x01 0x02 0x0F 表示的版本号是 V1.2.15，依此类推
        /// </summary>
        public byte[] VersionFlag { get; set; }

        /// <summary>
        /// 报文加密标识位：0 表示报文不加密，1 表示报文加密
        /// <para>用来区分报文是否进行加密，如果标识为 1，则说明对后续相应业务的数据体采用 ENCRYPT_KEY 对应的密钥进行加密处理。如果标识为 0，则说明不进行加密处理</para>
        /// </summary>
        public bool EncryptFlag { get; set; }

        /// <summary>
        /// 数据加密的密钥，长度为 4 个字节
        /// </summary>
        public UInt32 EncrtptKey { get; set; }
    }
}
