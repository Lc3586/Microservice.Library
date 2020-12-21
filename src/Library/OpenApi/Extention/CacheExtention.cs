using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.OpenApi.Extention
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static class CacheExtention
    {
        /// <summary>
        /// 类型的Api架构
        /// </summary>
        public static Dictionary<string, OpenApiObject> OpenApiObjectDic = new Dictionary<string, OpenApiObject>();

        /// <summary>
        /// 类型的命名空间
        /// </summary>
        public static Dictionary<string, string> AssemblyOfTypeDic = new Dictionary<string, string>();

        /// <summary>
        /// 类型的架构类型集合
        /// </summary>
        private static Dictionary<string, List<string>> TypesOfTypeDic = new Dictionary<string, List<string>>();

        /// <summary>
        /// 类型的架构属性集合
        /// </summary>
        private static Dictionary<string, List<string>> PropertysOfTypeDic = new Dictionary<string, List<string>>();

        /// <summary>
        /// 枚举的字段说明集合
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> EnumNameAndDescriptionDic = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获取类型的架构属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetPropertysOfTypeDic(this Type type)
        {
            var dic = new Dictionary<string, List<string>>();

            dic.GetPropertysOfTypeDic(type.FullName);

            return dic;
        }

        /// <summary>
        /// 获取类型的架构属性集合
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="typeFullName"></param>
        private static void GetPropertysOfTypeDic(this Dictionary<string, List<string>> dic, string typeFullName)
        {
            if (PropertysOfTypeDic.ContainsKey(typeFullName))
                dic.Add(typeFullName, PropertysOfTypeDic[typeFullName].ToList());

            if (TypesOfTypeDic.ContainsKey(typeFullName))
                TypesOfTypeDic[typeFullName]?.ForEach(o => dic.GetPropertysOfTypeDic(o));
        }

        /// <summary>
        /// 设置类型的架构属性集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="innerType"></param>
        public static void SetPropertysOfTypeDic(this Type type, Dictionary<string, List<string>> dic)
        {
            if (!TypesOfTypeDic.ContainsKey(type.FullName))
                TypesOfTypeDic.Add(type.FullName, dic.Keys.Where(k => k != type.FullName).ToList());

            foreach (var item in dic)
            {
                if (!PropertysOfTypeDic.ContainsKey(item.Key))
                    PropertysOfTypeDic.Add(item.Key, item.Value.ToList());
            }
        }

        ///// <summary>
        ///// 销毁
        ///// </summary>
        //public static void Dispose()
        //{
        //    OpenApiObjectDic = new Dictionary<Type, OpenApiObject>();
        //    EnumNameAndDescriptionDic = new Dictionary<Type, Dictionary<string, string>>();
        //}
    }
}
