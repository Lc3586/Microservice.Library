using Library.Extension;
using Library.SuperSocket.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.SuperSocket.JTProtocol
{
    public static class JTProtocolExtension
    {
        static JTProtocolExtension()
        {

        }

        /// <summary>
        /// 转义
        /// </summary>
        /// <param name="buffer">流数据</param>
        /// <returns></returns>
        public static byte[] Escape(this JTProtocol jT, byte[] buffer)
        {
            if (!buffer.Any_Ex())
                return buffer;

            byte[] result;

            using (MemoryStream ms = new MemoryStream())
            {
                buffer.ForEach(o =>
                {
                    var x2String = Get0xString(o);
                    if (jT.Escapes.ContainsKey(x2String))
                        ms.Write(Get0xBytes(jT.Escapes[x2String]), 0, 2);
                    else
                        ms.WriteByte(o);
                });
                result = ms.ToArray();
            }

            return result;
        }

        /// <summary>
        /// 转义还原
        /// </summary>
        /// <param name="buffer">流数据</param>
        /// <returns></returns>
        public static byte[] UnEscape(this JTProtocol jT, byte[] buffer)
        {
            if (!buffer.Any_Ex())
                return buffer;

            byte[] result;

            using (MemoryStream ms = new MemoryStream())
            {
                int i = 0;

                while (i < buffer.Length)
                {
                    if (i == buffer.Length - 1)
                    {
                        ms.WriteByte(buffer[i]);
                        break;
                    }

                    var x2Strings = Get0xString(buffer[i], buffer[i + 1]);
                    if (jT.Escapes.ContainsValue(x2Strings))
                    {
                        ms.WriteByte(Get0xByte(jT.Escapes.First(o => o.Value == x2Strings).Key));
                        i += 2;
                    }
                    else
                    {
                        ms.WriteByte(buffer[i]);
                        i += 1;
                    }
                }
                result = ms.ToArray();
            }

            return result;
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="jT">协议</param>
        /// <param name="bytes">数据</param>
        /// <param name="codeInfo">编码信息</param>
        /// <returns></returns>
        public static object Decode(this JTProtocol jT, byte[] bytes, CodeInfo codeInfo)
        {
            object value = null;
            switch (codeInfo.CodeType)
            {
                case CodeType.binary:
                    return new BitArray(bytes);//string.Join("", bytes.Select(o => Convert.ToString(o, 2).PadLeft(8, '0')).ToArray();
                case CodeType.@boolean:
                    return BitConverter.ToBoolean(bytes);
                case CodeType.@byte:
                    return bytes[0];
                case CodeType.@bytes:
                    return bytes;
                case CodeType.@uint16:
                    value = BitConverter.ToUInt16(bytes, 0);
                    return jT.BigEndian ? value : NativeSocketMethod.htons((UInt16)value);
                case CodeType.@uint32:
                    value = BitConverter.ToUInt32(bytes, 0);
                    return jT.BigEndian ? value : NativeSocketMethod.htonl((UInt32)value);
                case CodeType.@int16:
                    value = BitConverter.ToInt16(bytes, 0);
                    return jT.BigEndian ? value : NativeSocketMethod.htons((Int16)value);
                case CodeType.@int32:
                    value = BitConverter.ToInt32(bytes, 0);
                    return jT.BigEndian ? value : NativeSocketMethod.htonl((Int32)value);
                case CodeType.@date:
                    value = BitConverter.ToUInt16(bytes, 0);
                    return new DateTime(
                        jT.BigEndian ? (UInt16)value : NativeSocketMethod.htons((UInt16)value),
                        bytes[2],
                        bytes[3],
                        bytes[4],
                        bytes[5],
                        bytes[6]);
                case CodeType.@data:
                    return string.Join("", bytes);
                case CodeType.data_split:
                    return string.Join(' ', bytes);
                case CodeType.@object:
                    throw new NotImplementedException();
                case CodeType.json:
                    return Encoding.GetEncoding(jT.Encoding).GetString(bytes).Replace("\0", "").ToObject<object>();
                case CodeType.@string_x_0:
                    return string.Join("", bytes.Select(o => o.ToString("x").PadLeft(2, '0')));
                case CodeType.@string_x2:
                    return bytes[0].ToString("x2");
                case CodeType.@string_x4:
                    return $"0x{string.Join("", bytes.Select(o => o.ToString("x2")))}";
                case CodeType.@enum:
                    return Enum.Parse(
                        (codeInfo.Assembly.IsNullOrEmpty() ?
                            Assembly.GetExecutingAssembly() :
                            Assembly.Load(codeInfo.Assembly))
                        .GetType(codeInfo.TypeName),
                        Encoding.GetEncoding(jT.Encoding).GetString(bytes).Replace("\0", ""));
                case CodeType.@string:
                default:
                    return Encoding.GetEncoding(jT.Encoding).GetString(bytes).Replace("\0", "");
            }
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="jT">协议</param>
        /// <param name="value">值</param>
        /// <param name="codeInfo">编码信息</param>
        /// <returns></returns>
        public static byte[] Encode(this JTProtocol jT, object value, CodeInfo codeInfo)
        {
            switch (codeInfo.CodeType)
            {
                case CodeType.binary:
                    var value_binary = (BitArray)value;
                    var value_binary_bytes = new byte[value_binary.Length / 8];
                    value_binary.CopyTo(value_binary_bytes, 0);
                    return value_binary_bytes;
                case CodeType.@boolean:
                    return BitConverter.GetBytes((bool)value);
                case CodeType.@byte:
                    return new byte[] { (byte)value };
                case CodeType.@bytes:
                    return (byte[])value;
                case CodeType.@uint16:
                    return BitConverter.GetBytes(jT.BigEndian ? (UInt16)value : NativeSocketMethod.ntohs((UInt16)value));
                case CodeType.@int16:
                    return BitConverter.GetBytes((Int16)value);
                case CodeType.@uint32:
                    return BitConverter.GetBytes(jT.BigEndian ? (UInt32)value : NativeSocketMethod.ntohl((UInt32)value));
                case CodeType.@int32:
                    return BitConverter.GetBytes((Int32)value);
                case CodeType.@date:
                    var date = (DateTime)value;
                    var year_bytes = BitConverter.GetBytes(jT.BigEndian ? (UInt16)value : NativeSocketMethod.ntohs((UInt16)date.Year));
                    return new byte[] {
                        year_bytes[0] ,
                        year_bytes[1],
                        (byte)date.Month,
                        (byte)date.Day,
                        (byte)date.Hour,
                        (byte)date.Minute,
                        (byte)date.Second
                    };
                case CodeType.@data:
                    var value_string = ((string)value).Length / 2 == 0 ? (string)value : $"0{value}";
                    var bytes = new byte[value_string.Length / 2];
                    for (int i = 0; i < value_string.Length - 1; i += 2)
                    {
                        bytes[i / 2] = byte.Parse($"{value_string[i]}{value_string[i + 1]}");
                    }
                    return bytes;
                case CodeType.data_split:
                    return ((string)value).Split(' ').Select(o => byte.Parse(o)).ToArray();
                case CodeType.@object:
                    throw new NotImplementedException();
                case CodeType.json:
                    return Encoding.GetEncoding(jT.Encoding).GetBytes(value.ToJson());
                case CodeType.@string_x_0:
                    var string_x_0 = (string)value;
                    var string_x_0_bytes = new byte[string_x_0.Length / 2 + string_x_0.Length % 2];
                    for (int i = string_x_0.Length - 1; i >= 0; i -= 2)
                    {
                        string_x_0_bytes[i / 2] = byte.Parse($"{(i - 1 < 0 ? 0 : string_x_0[i - 1])}{string_x_0[i]}", NumberStyles.HexNumber);
                    }
                    return string_x_0_bytes;
                case CodeType.@string_x2:
                    return new byte[] { byte.Parse((string)value, NumberStyles.HexNumber) };
                case CodeType.@string_x4:
                    var value_string_x4 = ((string)value).Replace("0x", "");
                    var string_x4_bytes = new byte[value_string_x4.Length / 2];
                    for (int i = 0; i < string_x4_bytes.Length; i++)
                    {
                        string_x4_bytes[i] = byte.Parse(value_string_x4.Substring(i * 2, 2), NumberStyles.HexNumber);
                    }
                    return string_x4_bytes;
                case CodeType.@string:
                case CodeType.@enum:
                default:
                    return Encoding.GetEncoding(jT.Encoding).GetBytes(value.ToString());
            }
        }

        /// <summary>
        /// 创建Crc16Ccitt
        /// </summary>
        /// <param name="jT"></param>
        /// <returns></returns>
        private static CrcCcitt CreateCrcCcitt(this JTProtocol jT)
        {
            return new CrcCcitt(jT.CrcCcitt.CrcLength, jT.CrcCcitt.InitialCrcValue);
        }

        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static bool CheckCRCCode(this JTProtocol jT, MessagePackageInfo packageInfo, CrcCcitt crc = null)
        {
            var buffer = packageInfo.Buffer
                .Skip(jT.CrcCcitt.Skip[0])
                .Take(packageInfo.Buffer.Length - jT.CrcCcitt.Skip[1])
                .ToArray();

            var crcCode = Decode(
                jT,
                jT.CrcCcitt.Take > 0 ?
                packageInfo.Buffer.Take(jT.CrcCcitt.Take).ToArray() :
                packageInfo.Buffer.Skip(packageInfo.Buffer.Length + jT.CrcCcitt.Take).ToArray(),
                new CodeInfo() { CodeType = jT.CrcCcitt.CrcLength == CrcLength.B16 ? CodeType.uint16 : CodeType.@byte });

            packageInfo.SetPropertyValue(jT.CrcCcitt.Property, crcCode);
            return crcCode == (crc ?? jT.CreateCrcCcitt()).ComputeChecksum(buffer);
        }

        /// <summary>
        /// 设置CRC校验码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static void SetCRCCode(this JTProtocol jT, MessagePackageInfo packageInfo, CrcCcitt crc = null)
        {
            packageInfo.CRCCode = (crc ?? jT.CreateCrcCcitt()).ComputeChecksumBytes(
                packageInfo.Buffer
                .Skip(jT.CrcCcitt.Skip[0])
                .Take((int)packageInfo.MessageHeader.MsgLength - jT.CrcCcitt.Skip[1])
                .ToArray());
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="field">字段，多级使用.分隔</param>
        /// <returns></returns>
        public static object GetValueFromProperty(this object obj, string field)
        {
            try
            {
                var Index = field.IndexOf('.');
                var Field = field.Substring(0, Index > 0 ? Index : field.Length);
                var InObj = obj.GetType().GetProperty(Field).GetValue(obj);
                if (Index > 0)
                    return InObj.GetValueFromProperty(field.Substring(Index + 1));
                return InObj;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取属性值 : 错误 obj : [{obj?.GetType().FullName}] field : [{field}]", ex);
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj">实例</param>
        /// <param name="field">字段，多级使用.分隔</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetValueToProperty(this object obj, string field, object value)
        {
            try
            {
                var Index = field.IndexOf('.');
                var Field = field.Substring(0, Index > 0 ? Index : field.Length);
                var Property = obj.GetType().GetProperty(Field);
                if (Index > 0)
                    Property.GetValue(obj).SetValueToProperty(field.Substring(Index + 1), value);
                else
                    Property.SetValue(obj, value);
            }
            catch (Exception ex)
            {
                throw new Exception($"设置属性值 : 错误 obj : [{obj?.GetType().FullName}] field : [{field}]", ex);
            }
        }

        /// <summary>
        /// 右侧补位
        /// </summary>
        /// <param name="buffer">原数据</param>
        /// <param name="length">长度(为0时不做任何改动)</param>
        /// <param name="paddingByte">补位字节</param>
        /// <returns>补位后的数据</returns>
        public static byte[] PadRight(this byte[] buffer, int length, byte paddingByte = 0x00)
        {
            if (length == 0 || buffer.Length == length)
                return buffer;
            var result = new byte[length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = i < buffer.Length ? buffer[i] : paddingByte;
            }
            return result;
        }

        /// <summary>
        /// 转为16进制byte数组
        /// </summary>
        /// <param name="x2String">十六进制字符串</param>
        /// <returns></returns>
        public static byte Get0xByte(string x2String)
        {
            return (byte)Int32.Parse($@"{x2String[2]}{x2String[3]}", NumberStyles.HexNumber);
        }

        /// <summary>
        /// 转为16进制byte数组
        /// </summary>
        /// <param name="x2String">十六进制字符串</param>
        /// <returns></returns>
        public static byte[] Get0xBytes(string x2String)
        {
            return new byte[] { Get0xByte(x2String) };
        }

        /// <summary>
        /// 转为16进制byte数组
        /// </summary>
        /// <param name="x2String">十六进制字符串</param>
        /// <returns></returns>
        public static byte[] Get0xBytes(params string[] x2String)
        {
            return x2String.Select(o => Get0xByte(o)).ToArray();
        }

        /// <summary>
        /// 转为16进制字符串
        /// </summary>
        /// <param name="x2Byte">十六进制byte</param>
        /// <returns></returns>
        public static string Get0xString(byte x2Byte)
        {
            return $"{x2Byte:x2}";
        }

        /// <summary>
        /// 转为16进制字符串
        /// </summary>
        /// <param name="x2Byte">十六进制byte</param>
        /// <returns></returns>
        public static string[] Get0xString(params byte[] x2Byte)
        {
            return x2Byte.Select(o => Get0xString(o)).ToArray();
        }

        /// <summary>
        /// 转为16进制字符串
        /// </summary>
        /// <param name="x2Byte">十六进制byte</param>
        /// <returns></returns>
        public static string Get0x4String(byte x2Byte)
        {
            return $"0x{x2Byte:x2}";
        }

        /// <summary>
        /// 转为16进制字符串
        /// </summary>
        /// <param name="x2Byte">十六进制byte</param>
        /// <returns></returns>
        public static string[] Get0x4String(params byte[] x2Byte)
        {
            return x2Byte.Select(o => Get0x4String(o)).ToArray();
        }
    }
}
