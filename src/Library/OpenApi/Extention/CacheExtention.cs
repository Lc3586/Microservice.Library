using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.OpenApi.Extention
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static class CacheExtention
    {
        /// <summary>
        /// 类型对应的架构信息
        /// </summary>
        public static Dictionary<Type, OpenApiObject> OpenApiObjectDic = new Dictionary<Type, OpenApiObject>();

        /// <summary>
        /// 枚举对应的字段说明信息
        /// </summary>
        public static Dictionary<Type, Dictionary<string, string>> EnumNameAndDescriptionDic = new Dictionary<Type, Dictionary<string, string>>();

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
