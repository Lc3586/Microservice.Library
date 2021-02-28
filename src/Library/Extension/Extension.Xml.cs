using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Microservice.Library.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// xml序列化器缓存
        /// </summary>

        static readonly ConcurrentDictionary<string, XmlSerializer> CachedSerializers = new ConcurrentDictionary<string, XmlSerializer>();

        /// <summary>
        /// 获取xml序列化器
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterNs"></param>
        /// <returns></returns>
        public static XmlSerializer GetXmlSerializer(this Type elementType, string parameterName, string parameterNs)
        {
            var key = $"{elementType}|{parameterName}|{parameterNs}";
            return CachedSerializers.GetOrAdd(key, _ => new XmlSerializer(elementType, null, new Type[0], new XmlRootAttribute(parameterName), parameterNs));
        }

        /// <summary>
        /// 获取xml元素的值
        /// </summary>
        /// <typeparam name="T">参数数据类型</typeparam>
        /// <param name="xmlReader"></param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="parameterNs">命名空间</param>
        /// <param name="modelBound">是否模型</param>
        /// <returns></returns>
        public static T GetXmlElementValue<T>(this XmlDictionaryReader xmlReader, string parameterName, string parameterNs = "", bool modelBound = false)
        {
            var type = typeof(T);

            if (modelBound)
            {
                //if (!xmlReader.IsStartElement(parameterName, parameterNs))
                //    return default;

                //xmlReader.ReadStartElement(parameterName, parameterNs);

                //var result = default(T);

                //foreach (var property in type.GetProperties())
                //{
                //    property.SetValue(result, GetXmlElementValue(xmlReader, property.PropertyType, property.Name));
                //}

                //return result;
            }

            return (T)GetXmlElementValue(xmlReader, type, parameterName, parameterNs, modelBound);
        }

        /// <summary>
        /// 获取xml元素的值
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <param name="parameterType">参数数据类型</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="parameterNs">命名空间</param>
        /// <param name="innerXml"></param>
        /// <returns></returns>
        public static object GetXmlElementValue(this XmlDictionaryReader xmlReader, Type parameterType, string parameterName, string parameterNs = "", bool innerXml = false)
        {
            if (!xmlReader.IsStartElement(parameterName, parameterNs))
                return default;

            xmlReader.MoveToStartElement(parameterName, parameterNs);

            if (!xmlReader.IsStartElement(parameterName, parameterNs))
                return default;

            var serializer = parameterType.GetXmlSerializer(parameterName, parameterNs);

            lock (serializer)
            {
                if (parameterType == typeof(Stream) || typeof(Stream).IsAssignableFrom(parameterType))
                {
                    xmlReader.Read();
                    return new MemoryStream(xmlReader.ReadContentAsBase64());
                }

                if (innerXml)
                    return $"<{parameterName}>{xmlReader.ReadInnerXml()}</{parameterName}>";

                return serializer.Deserialize(xmlReader);
            }
        }

        /// <summary>
        /// 对象序列化为Xml字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serialize(this object obj)
        {
            var type = obj.GetType();
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        /// <summary>
        /// Xml字符串转Hashtable
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static Hashtable XMLToHashtable(this string xmlData)
        {
            DataTable dt = XMLToDataTable(xmlData);
            Hashtable ht = new Hashtable();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string key = dt.Columns[i].ColumnName;
                    ht[key] = dr[key];
                }
            }
            return ht;
        }

        /// <summary>
        /// Xml字符串转DataTable
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataTable XMLToDataTable(this string xmlData)
        {
            if (!String.IsNullOrEmpty(xmlData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(xmlData));
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Xml字符串转DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet XMLToDataSet(this string xmlData)
        {
            if (!String.IsNullOrEmpty(xmlData))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(xmlData));
                return ds;
            }
            return null;
        }

        /// <summary>
        /// 将XML配置映射至VML属性
        /// LCTR 2017-11-30
        /// </summary>
        /// <typeparam name="T">指定的model类型</typeparam>
        /// <param name="NodeName">节点名称</param>
        /// <param name="Path">XML文件绝对路径</param>
        /// <param name="IsReturnNUll">当异常时，是否返回NULL</param>
        /// <returns></returns>
        public static T GetXMLConfig<T>(string NodeName, string Path, bool IsReturnNUll = false) where T : new()
        {
            try
            {
                //使用XmlDocument读取XML
                XmlDocument xdoc = new XmlDocument();
                //相对路径
                xdoc.Load(Path);
                XmlNode xn = xdoc.SelectSingleNode(NodeName);
                XmlNodeList xnl = xn.ChildNodes;
                Type type = typeof(T);
                T vml = new T();
                if (xnl == null)
                    return vml;
                if (xnl.Count == 0)
                    return vml;
                foreach (XmlNode node in xnl)
                {
                    foreach (PropertyInfo p in type.GetProperties())
                    {
                        if (node.Name == p.Name)
                            p.SetValue(vml, Convert.ChangeType(node.InnerText, p.PropertyType, null));
                    }
                }
                return vml;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
            {
                return IsReturnNUll ? default : new T();
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
