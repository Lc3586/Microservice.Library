using System;
using System.Collections.Generic;
using System.Text;

namespace Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// 编码信息
    /// </summary>
    public class CodeInfo
    {
        /// <summary>
        /// 编码类型
        /// </summary>
        public CodeType CodeType { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 实体
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 编码类型
    /// </summary>
    public enum CodeType
    {
        /// <summary>
        /// 
        /// </summary>
        binary,
        /// <summary>
        /// BitConverter.ToBoolean(bytes);
        /// </summary>
        @boolean,
        @byte,
        @bytes,
        /// <summary>
        /// NativeSocketMethod.htonl(BitConverter.ToUInt16(data, 0))
        /// </summary>
        @uint16,
        /// <summary>
        /// NativeSocketMethod.htonl(BitConverter.ToUInt32(data, 0))
        /// </summary>
        @uint32,
        /// <summary>
        /// NativeSocketMethod.htonl(BitConverter.ToInt16(data, 0))
        /// </summary>
        @int16,
        /// <summary>
        /// NativeSocketMethod.htonl(BitConverter.ToInt32(data, 0))
        /// </summary>
        @int32,
        /// <summary>
        /// Enum.Parse(type, Encoding.GetEncoding("gbk").GetString(data).Replace("\0", ""))
        /// </summary>
        @enum,
        /// <summary>
        /// Encoding.GetEncoding("gbk").GetString(data).Replace("\0", "");
        /// </summary>
        @string,
        /// <summary>
        /// string.Join('\0', data.ToList().ForEach(o => o.ToString("x").PadLeft(2, '0')));
        /// </summary>
        @string_x_0,
        /// <summary>
        /// data.ToString("x2");
        /// </summary>
        string_x2,
        /// <summary>
        /// $"0x{data.ToString("x2")}";
        /// </summary>
        string_x4,
        /// <summary>
        /// UTC日期时间
        /// new DateTime(
        /// 年：NativeSocketMethod.htons(BitConverter.ToUInt16(bytes, 0)),
        /// 月：bytes[2],
        /// 日：bytes[3],
        /// 时：bytes[4],
        /// 分：bytes[5],
        /// 秒：bytes[6])
        /// </summary>
        @date,
        /// <summary>
        /// string.Join(' ', data);
        /// </summary>
        @data,
        /// <summary>
        /// 使用空格分隔
        /// </summary>
        @data_split,
        @object,
        /// <summary>
        /// Encoding.GetEncoding(jT.Encoding).GetBytes(value.ToJson())
        /// </summary>
        @json,
        @additional
    }
}
