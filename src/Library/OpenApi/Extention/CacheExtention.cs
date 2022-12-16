using Microsoft.OpenApi.Any;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.Library.OpenApi.Extention
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static class CacheExtention
    {
        /// <summary>
        /// 类型的Api架构
        /// </summary>
        public static readonly ConcurrentDictionary<string, OpenApiObject> OpenApiObjectDic = new ConcurrentDictionary<string, OpenApiObject>();

        /// <summary>
        /// 类型的命名空间
        /// </summary>
        public static readonly ConcurrentDictionary<string, string> AssemblyOfTypeDic = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 类型的架构类型集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<string>> TypesOfTypeDic = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 类型的架构属性集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<string>> PropertysOfTypeDic = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 枚举的字段说明集合
        /// </summary>
        public static readonly ConcurrentDictionary<string, Dictionary<string, string>> EnumNameAndDescriptionDic = new ConcurrentDictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获取类型的架构属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="deepClone">深度复制</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetPropertysOfTypeDic(this Type type, bool deepClone = true)
        {
            var dic = new Dictionary<string, List<string>>();

            dic.GetPropertysOfTypeDic(type.FullName, deepClone);

            return dic;
        }

        /// <summary>
        /// 获取类型的架构属性集合
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="typeFullName"></param>
        /// <param name="deepClone">深度复制</param>
        private static void GetPropertysOfTypeDic(this Dictionary<string, List<string>> dic, string typeFullName, bool deepClone = true)
        {
            if (PropertysOfTypeDic.ContainsKey(typeFullName))
                dic.Add(typeFullName, deepClone ? PropertysOfTypeDic[typeFullName].ToList() : PropertysOfTypeDic[typeFullName]);

            if (TypesOfTypeDic.ContainsKey(typeFullName))
                TypesOfTypeDic[typeFullName]?.ForEach(o => dic.GetPropertysOfTypeDic(o));
        }

        /// <summary>
        /// 设置类型的架构属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dic"></param>
        public static void SetPropertysOfTypeDic(this Type type, Dictionary<string, List<string>> dic)
        {
            if (!TypesOfTypeDic.ContainsKey(type.FullName))
            {
                var types = dic.Keys.Where(k => k != type.FullName).ToList();
                TypesOfTypeDic.AddOrUpdate(type.FullName, types, (key, old) => types);
            }
            else
                TypesOfTypeDic[type.FullName] = TypesOfTypeDic[type.FullName].Union(dic.Keys.Where(k => k != type.FullName)).ToList();

            foreach (var item in dic)
            {
                if (!PropertysOfTypeDic.ContainsKey(item.Key))
                {
                    var propertys = item.Value.ToList();
                    PropertysOfTypeDic.AddOrUpdate(item.Key, propertys, (key, old) => propertys);
                }
            }
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public static void Dispose()
        {
            OpenApiObjectDic.Clear();
            AssemblyOfTypeDic.Clear();
            TypesOfTypeDic.Clear();
            PropertysOfTypeDic.Clear();
            EnumNameAndDescriptionDic.Clear();
        }
    }
}
