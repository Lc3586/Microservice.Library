using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.TypeTool
{
    /// <summary>
    /// Type帮助类
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 获取指定命名空间下都类型集合
        /// </summary>
        /// <param name="assembly">命名空间<para>支持通配符</para></param>
        /// <returns></returns>
        public static List<Type> GetTypes(params string[] assembly)
        {
            return assembly?.SelectMany(x =>
             {
                 try
                 {
                     return Assembly.Load(x).GetTypes();
                 }
                 catch (FileNotFoundException)
                 {
                     var result = Array.Empty<Type>();
                     if (x.IndexOfAny(new[] { '\\', '/', ':', '*', '"', '<', '>', '|' }) > -1)
                     {
                         var Files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{x}.dll");
                         if (Files.Length > 0)
                             result = Files.SelectMany(F => Assembly.LoadFile(F).GetTypes()).ToArray();
                     }
                     else
                     {
                         var path = $"{AppDomain.CurrentDomain.BaseDirectory}{x}.dll";
                         if (File.Exists(path))
                             result = Assembly.LoadFile(path).GetTypes();
                     }
                     return result;
                 }
             }).ToList();
        }
    }
}
