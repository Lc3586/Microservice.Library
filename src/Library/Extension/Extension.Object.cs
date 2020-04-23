using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        //static Extention()
        //{
        //    JsonSerializerSettings setting = new JsonSerializerSettings();
        //    JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
        //    {
        //        //日期类型默认格式化处理
        //        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        //        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        //        return setting;
        //    });
        //}

        private static BindingFlags _bindingFlags { get; }
            = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>
        /// <returns></returns> 
        public static byte[] ToBytes(this object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj, DefaultContractResolver contractResolver = null)
        {
            return contractResolver == null ? JsonConvert.SerializeObject(obj) :
                JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver
                });
        }

        /// <summary>
        /// 实体类转json数据，速度快
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public static string EntityToJson(this object t)
        {
            if (t == null)
                return null;
            string jsonStr = "";
            jsonStr += "{";
            PropertyInfo[] infos = t.GetType().GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                jsonStr = jsonStr + "\"" + infos[i].Name + "\":\"" + infos[i].GetValue(t).ToString() + "\"";
                if (i != infos.Length - 1)
                    jsonStr += ",";
            }
            jsonStr += "}";
            return jsonStr;
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;

            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="rootNodeName">根节点名(建议设为xml)</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj, string rootNodeName)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, rootNodeName);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 是否拥有某属性
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static bool ContainsProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, _bindingFlags) != null;
        }

        /// <summary>
        /// 获取某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, _bindingFlags).GetValue(obj);
        }

        /// <summary>
        /// 设置某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, _bindingFlags).SetValue(obj, value);
        }

        /// <summary>
        /// 是否拥有某字段
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static bool ContainsField(this object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName, _bindingFlags) != null;
        }

        /// <summary>
        /// 获取某字段值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static object GetGetFieldValue(this object obj, string fieldName)
        {
            return obj.GetType().GetField(fieldName, _bindingFlags).GetValue(obj);
        }

        /// <summary>
        /// 设置某字段值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetFieldValue(this object obj, string fieldName, object value)
        {
            obj.GetType().GetField(fieldName, _bindingFlags).SetValue(obj, value);
        }

        /// <summary>
        /// 改变实体类型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType(this object obj, Type targetType)
        {
            return obj.ToJson().ToObject(targetType);
        }

        /// <summary>
        /// 改变实体类型
        /// </summary>
        /// <typeparam name="T">目标泛型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T ChangeType<T>(this object obj)
        {
            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 改变类型
        /// </summary>
        /// <param name="obj">原对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType_ByConvert(this object obj, Type targetType)
        {
            object resObj;
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                NullableConverter newNullableConverter = new NullableConverter(targetType);
                resObj = newNullableConverter.ConvertFrom(obj);
            }
            else
            {
                resObj = Convert.ChangeType(obj, targetType);
            }

            return resObj;
        }

        /// <summary>
        /// 更改数据类型
        /// <para>有错误时，使用其他策略（特殊转换，使用默认值）</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object TryChangeType(this object obj, Type targetType)
        {
            object result;
            try
            {
                result = Convert.ChangeType(obj, targetType);
            }
            catch (Exception ex)
            {
                switch (Type.GetTypeCode(targetType))
                {
                    case TypeCode.Boolean:
                        switch (obj.ToString().ToLower())
                        {
                            case "1":
                            case "yes":
                            case "true":
                                result = true;
                                break;
                            case "0":
                            case "no":
                            case "false":
                                result = false;
                                break;
                            default:
                                result = default;
                                break;
                        }
                        break;
                    case TypeCode.String:
                        result = System.Text.Encoding.Default.GetString(obj is Array ? (byte[])obj : (ObjectToBytes(obj) ?? new byte[0]));
                        break;
                    case TypeCode.Byte:
                    case TypeCode.Char:
                    case TypeCode.DateTime:
                    case TypeCode.DBNull:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Empty:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Object:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    default:
                        result = default;
                        break;
                }
            }
            return result;
        }

        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    return ms.GetBuffer();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns> 
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new BinaryFormatter(); return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="color">颜色(默认白色)</param>
        /// <param name="tag">标签</param>
        /// <param name="line">是否整行(默认true)</param>
        /// <param name="blankline">空行数</param>
        /// <returns></returns>
        public static void ConsoleWrite(this object data, ConsoleColor color = ConsoleColor.White, string tag = null, bool line = true, int blankline = 0)
        {
            ConsoleColor _color = Console.ForegroundColor;
            if (line)
                Console.Write("\n");
            if (!tag.IsNullOrEmpty())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(tag + " : ");
            }
            Console.ForegroundColor = color;
            Console.Write(data);
            Console.ForegroundColor = _color;
            while (blankline > 0)
            {
                Console.Write("\n");
                blankline--;
            }
        }

        /// <summary>
        /// 二维数组排序
        /// </summary>
        /// <param name="data">二维数据</param>
        /// <param name="sortIndex">用作排序依据的维度(可能的值：0,1)</param>
        /// <param name="asc">true:升序,false:降序</param>
        /// <returns></returns>
        public static object[,] OrderBy(this object[,] data, int sortIndex = 0, bool asc = true)
        {
            var result = data;
            if (data == null)
                goto end; ;
            if (data.Length == 0)
                goto end;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    var compare = Comparer.Default.Compare(data[i, sortIndex], data[j, sortIndex]);
                    if (compare == 0)
                        continue;
                    if ((asc && compare > 0) || (!asc && compare < 0))
                    {
                        var temp = data[i, sortIndex];
                        data[i, sortIndex] = data[j, sortIndex];
                        data[j, sortIndex] = temp;
                    }
                }
            }

            end:
            return result;
        }
    }
}
