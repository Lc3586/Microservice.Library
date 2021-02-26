using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Microservice.Library.Xml
{
    /// <summary>
    /// Xml文件处理
    /// </summary>
    public class XmlFile
    {
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
            catch (Exception)
            {
                return IsReturnNUll ? default(T) : new T();
            }
        }
    }
}