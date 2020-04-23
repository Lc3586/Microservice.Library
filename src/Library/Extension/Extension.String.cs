﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 转为字节数组
        /// </summary>
        /// <param name="base64Str">base64字符串</param>
        /// <returns></returns>
        public static byte[] ToBytes_FromBase64Str(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        /// <summary>
        /// 转换为MD5加密后的字符串（默认加密为32位）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMD5String(this string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            md5.Dispose();

            return sb.ToString();
        }

        /// <summary>
        /// Base64加密
        /// 注:默认采用UTF8编码
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(this string source)
        {
            return Base64Encode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="encoding">加密采用的编码方式</param>
        /// <returns></returns>
        public static string Base64Encode(this string source, Encoding encoding)
        {
            string encode = string.Empty;
            byte[] bytes = encoding.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密
        /// 注:默认使用UTF8编码
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(this string result)
        {
            return Base64Decode(result, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="encoding">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(this string result, Encoding encoding)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encoding.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64Url编码
        /// </summary>
        /// <param name="text">待编码的文本字符串</param>
        /// <returns>编码的文本字符串</returns>
        public static string Base64UrlEncode(this string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

            return base64;
        }

        /// <summary>
        /// Base64Url解码
        /// </summary>
        /// <param name="base64UrlStr">使用Base64Url编码后的字符串</param>
        /// <returns>解码后的内容</returns>
        public static string Base64UrlDecode(this string base64UrlStr)
        {
            base64UrlStr = base64UrlStr.Replace('-', '+').Replace('_', '/');
            switch (base64UrlStr.Length % 4)
            {
                case 2:
                    base64UrlStr += "==";
                    break;
                case 3:
                    base64UrlStr += "=";
                    break;
            }
            var bytes = Convert.FromBase64String(base64UrlStr);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 计算SHA1摘要
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string str)
        {
            return str.ToSHA1Bytes(Encoding.UTF8);
        }

        /// <summary>
        /// 计算SHA1摘要
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string str, Encoding encoding)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = encoding.GetBytes(str);
            byte[] outputBytes = sha1.ComputeHash(inputBytes);

            return outputBytes;
        }

        /// <summary>
        /// 转为SHA1哈希加密字符串
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(this string str)
        {
            return str.ToSHA1String(Encoding.UTF8);
        }

        /// <summary>
        /// 转为SHA1哈希
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToSHA1String(this string str, Encoding encoding)
        {
            byte[] sha1Bytes = str.ToSHA1Bytes(encoding);
            string resStr = BitConverter.ToString(sha1Bytes);
            return resStr.Replace("-", "").ToLower();
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSHA256String(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hash = SHA256.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// HMACSHA256算法
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string ToHMACSHA256String(this string text, string secret)
        {
            secret = secret ?? "";
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(text);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage).Replace('+', '-').Replace('/', '_').TrimEnd('=');
            }
        }

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            if (string.IsNullOrEmpty(str) || str == "0")
                return false;
            if (str == "1")
                return true;
            return Convert.ToBoolean(str);
        }

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            str = str.Replace("\0", "");
            if (string.IsNullOrEmpty(str))
                return 0;
            return Convert.ToInt32(str);
        }

        /// <summary>
        /// 二进制字符串转为Int
        /// </summary>
        /// <param name="str">二进制字符串</param>
        /// <returns></returns>
        public static int ToInt_FromBinString(this string str)
        {
            return Convert.ToInt32(str, 2);
        }

        /// <summary>
        /// 将16进制字符串转为Int
        /// </summary>
        /// <param name="str">数值</param>
        /// <returns></returns>
        public static int ToInt0X(this string str)
        {
            int num = Int32.Parse(str, NumberStyles.HexNumber);
            return num;
        }

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            str = str.Replace("\0", "");
            if (string.IsNullOrEmpty(str))
                return 0;

            return Convert.ToInt64(str);
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return Convert.ToDouble(str);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="theEncoding">需要的编码</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str, Encoding theEncoding)
        {
            return theEncoding.GetBytes(str);
        }

        /// <summary>
        /// 将16进制字符串转为Byte数组
        /// </summary>
        /// <param name="str">16进制字符串(2个16进制字符表示一个Byte)</param>
        /// <returns></returns>
        public static byte[] To0XBytes(this string str)
        {
            List<byte> resBytes = new List<byte>();
            for (int i = 0; i < str.Length; i = i + 2)
            {
                string numStr = $@"{str[i]}{str[i + 1]}";
                resBytes.Add((byte)numStr.ToInt0X());
            }

            return resBytes.ToArray();
        }

        /// <summary>
        /// 将ASCII码形式的字符串转为对应字节数组
        /// 注：一个字节一个ASCII码字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToASCIIBytes(this string str)
        {
            return str.ToList().Select(x => (byte)x).ToArray();
        }

        /// <summary>
        /// 转换为日期格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return Convert.ToDateTime(str);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// 删除Json字符串中键中的@符号
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        public static string RemoveAt(this string jsonStr)
        {
            Regex reg = new Regex("\"@([^ \"]*)\"\\s*:\\s*\"(([^ \"]+\\s*)*)\"");
            string strPatten = "\"$1\":\"$2\"";
            return reg.Replace(jsonStr, strPatten);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object ToObject(this string jsonStr, Type type)
        {
            return JsonConvert.DeserializeObject(jsonStr, type);
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlStr">XML字符串</param>
        /// <returns></returns>
        public static T XmlStrToObject<T>(this string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            string jsonJsonStr = JsonConvert.SerializeXmlNode(doc);

            return JsonConvert.DeserializeObject<T>(jsonJsonStr);
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <param name="xmlStr">XML字符串</param>
        /// <returns></returns>
        public static JObject XmlStrToJObject(this string xmlStr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            string jsonJsonStr = JsonConvert.SerializeXmlNode(doc);

            return JsonConvert.DeserializeObject<JObject>(jsonJsonStr);
        }

        /// <summary>
        /// 将Json字符串转为List'T'
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string jsonStr)
        {
            return string.IsNullOrEmpty(jsonStr) ? null : JsonConvert.DeserializeObject<List<T>>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串转为Dictionary<'TK','TV'>
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static Dictionary<TK, TV> ToDictionary<TK, TV>(this string jsonStr)
        {
            return string.IsNullOrEmpty(jsonStr) ? null : JsonConvert.DeserializeObject<Dictionary<TK, TV>>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串转为DataTable
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string jsonStr)
        {
            return jsonStr == null ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串转为JObject
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static JObject ToJObject(this string jsonStr)
        {
            return jsonStr == null ? JObject.Parse("{}") : JObject.Parse(jsonStr.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// 将Json字符串转为JArray
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static JArray ToJArray(this string jsonStr)
        {
            return jsonStr == null ? JArray.Parse("[]") : JArray.Parse(jsonStr.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// json数据转实体类,仅仅应用于单个实体类，速度非常快
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T ToEntity<T>(this string json)
        {
            if (json == null || json == "")
                return default(T);

            Type type = typeof(T);
            object obj = Activator.CreateInstance(type, null);

            foreach (var item in type.GetProperties())
            {
                PropertyInfo info = obj.GetType().GetProperty(item.Name);
                string pattern;
                pattern = "\"" + item.Name + "\":\"(.*?)\"";
                foreach (Match match in Regex.Matches(json, pattern))
                {
                    switch (item.PropertyType.ToString())
                    {
                        case "System.String": info.SetValue(obj, match.Groups[1].ToString(), null); break;
                        case "System.Int32": info.SetValue(obj, match.Groups[1].ToString().ToInt(), null); ; break;
                        case "System.Int64": info.SetValue(obj, Convert.ToInt64(match.Groups[1].ToString()), null); ; break;
                        case "System.DateTime": info.SetValue(obj, Convert.ToDateTime(match.Groups[1].ToString()), null); ; break;
                    }
                }
            }
            return (T)obj;
        }

        /// <summary>
        /// 转为首字母大写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToFirstUpperStr(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// 转为首字母小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToFirstLowerStr(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        /// <summary>
        /// 转为网络终结点IPEndPoint
        /// </summary>=
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static IPEndPoint ToIPEndPoint(this string str)
        {
            IPEndPoint iPEndPoint = null;
            try
            {
                string[] strArray = str.Split(':').ToArray();
                string addr = strArray[0];
                int port = Convert.ToInt32(strArray[1]);
                iPEndPoint = new IPEndPoint(IPAddress.Parse(addr), port);
            }
            catch
            {
                iPEndPoint = null;
            }

            return iPEndPoint;
        }

        /// <summary>
        /// 将枚举类型的文本转为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumText">枚举文本</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string enumText)
        {
            var values = typeof(T).GetEnumValues().CastToList<T>();
            return values.Where(x => x.ToString() == enumText).FirstOrDefault();
        }

        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 匹配字符串，如果没有匹配的，则返回完整长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOfN2L(this string str, string value)
        {
            var i = str.IndexOf(value);
            return i == -1 ? str.Length : i;
        }

        /// <summary>
        /// 替换字符串中指定的内容
        /// 将索引后指定长度的内容替换为对应的字符，字符数量和长度相等
        /// 长度为null时会全部替换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index">索引</param>
        /// <param name="length">长度</param>
        /// <param name="value">替换的字符</param>
        /// <returns></returns>
        public static string Replace(this string str, char value, int index, int? length = null)
        {

            return Replace_core(str, index, length, value, null);
        }

        /// <summary>
        /// 替换字符串中指定的内容
        /// 将索引后指定长度的内容完整替换为对应的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index">索引</param>
        /// <param name="length">长度</param>
        /// <param name="value">替换的值</param>
        /// <returns></returns>
        public static string Replace(this string str, string value, int index, int length)
        {
            return Replace_core(str, index, length, null, value);
        }

        /// <summary>
        /// 替换字符串中指定的内容
        /// 将索引后指定长度的内容替换为对应的字符，字符数量和长度相等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="indexes">索引集合（二维数组[索引,长度]）</param>
        /// <param name="value">替换的字符</param>
        /// <returns></returns>
        public static string Replace(this string str, int[,] indexes, char value)
        {
            return Replace_core(str, indexes, value, null);
        }

        /// <summary>
        /// 替换字符串中指定的内容
        /// 将索引后指定长度的内容完整替换为对应的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="indexes">索引集合（二维数组[索引,长度]）</param>
        /// <param name="value">替换的值</param>
        /// <returns></returns>
        public static string Replace(this string str, int[,] indexes, string value)
        {
            return Replace_core(str, indexes, null, value);
        }

        /// <summary>
        /// 替换字符串中指定的内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index">索引</param>
        /// <param name="length">长度</param>
        /// <param name="value_c">替换的字符</param>
        /// <param name="value_s">替换的值</param>
        /// <returns></returns>
        private static string Replace_core(string str, int index, int? length, char? value_c, string value_s)
        {
            if (str.IsNullOrEmpty())
                return string.Empty;
            if (value_c == null && value_s == null)
                return str;
            if (length == null)
                length = str.Length;
            if (length == 0)
                return str;

            var _value = string.Empty;
            if (value_c != null)
                _value = _value.PadRight(length.Value, value_c.Value);
            else
                _value = value_s;
            str = str.Substring(0, index) + _value + str.Substring(index + length.Value);

            return str;
        }

        /// <summary>
        /// 替换字符串中指定的内容
        /// </summary>
        /// <param name="str"></param>
        /// <param name="indexes">索引集合（二维数组[索引,长度]）</param>
        /// <param name="value_c">替换的字符</param>
        /// <param name="value_s">替换的值</param>
        /// <returns></returns>
        private static string Replace_core(string str, int[,] indexes, char? value_c, string value_s)
        {
            if (str.IsNullOrEmpty())
                return string.Empty;
            if (value_c == null && value_s == null)
                return str;
            if (indexes == null || indexes.Length == 0)
                return str;

            indexes = indexes.OrderBy();

            int[,,] excess_length = null;

            for (int i = 0; i < indexes.Length; i++)
            {
                var index = indexes[i, 0];
                var length = indexes[i, 1];
                if (index > str.Length)
                    continue;
                if (index + length > str.Length)
                    length = str.Length - index;

                var _value = string.Empty;
                if (value_c != null)
                {
                    _value = _value.PadRight(length, value_c.Value);
                }
                else
                {
                    _value = value_s;
                    var difference = _value.Length - length;
                    if (difference != 0)
                        excess_length = new int[,,] { { { index, length, difference } } };
                }

                var d_index = index;
                if (excess_length != null)
                {
                    if (index + length > excess_length[0, 0, 0])
                        d_index = index <= excess_length[0, 0, 0] + excess_length[0, 0, 1] + (excess_length[0, 0, 1] >= 0 ? 0 : excess_length[0, 0, 2]) ? index : (index + excess_length[0, 0, 2]);
                    excess_length = null;
                }

                str = str.Substring(0, d_index) + _value + str.Substring(d_index + length);
            }
            return str;
        }

        /// <summary>
        /// 格式转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="type">数据类型（未指定时使用默认值的数据类型）</param>
        /// <returns></returns>
        public static T ConvertToAny<T>(this string str, T defaultValue = default, Type type = null)
        {
            T result = defaultValue;
            try
            {
                if (str.IsNullOrEmpty())
                    return result;

                object data = null;
                type = type ?? typeof(T);

                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(type);
                    type = nullableConverter.UnderlyingType;
                }

                if (type == typeof(string))
                    data = str;
                else if (type == typeof(byte))
                {
                    byte value;
                    if (byte.TryParse(str, out value))
                        data = value;
                }
                else if (type == typeof(int))
                {
                    int value;
                    if (int.TryParse(str, out value))
                        data = value;
                }
                else if (type == typeof(double))
                {
                    double value;
                    if (double.TryParse(str, out value))
                        data = value;
                }
                else if (type == typeof(decimal))
                {
                    decimal value;
                    if (decimal.TryParse(str, out value))
                        data = value;
                }
                else if (type == typeof(bool))
                {
                    bool value;
                    if (bool.TryParse(str, out value))
                        data = value;
                }
                else if (type == typeof(TimeSpan))
                {
                    TimeSpan value;
                    if (TimeSpan.TryParse(str, out value))
                        data = value;
                }
                else if (type == typeof(DateTime))
                {
                    DateTime value;
                    if (DateTime.TryParse(str, out value))
                        data = value;
                }
                if (data == null && type.IsValueType)
                    return defaultValue;

                result = (T)Convert.ChangeType(data, type, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                result = defaultValue;
            }
            return result;
        }
    }
}
