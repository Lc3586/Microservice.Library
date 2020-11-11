using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Library.Xml
{
    public static class XmlReaderHelper
    {
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
        /// <returns></returns>
        public static object GetXmlElementValue(this XmlDictionaryReader xmlReader, Type parameterType, string parameterName, string parameterNs = "", bool innerXml = false)
        {
            if (!xmlReader.IsStartElement(parameterName, parameterNs))
                return default;

            xmlReader.MoveToStartElement(parameterName, parameterNs);

            if (!xmlReader.IsStartElement(parameterName, parameterNs))
                return default;

            var serializer = CachedXmlSerializer.GetXmlSerializer(parameterType, parameterName, parameterNs);

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
    }
}
