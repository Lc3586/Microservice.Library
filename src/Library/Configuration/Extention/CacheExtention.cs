using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Microservice.Library.Configuration.Extention
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static class CacheExtention
    {
        /// <summary>
        /// 类型对应的架构信息
        /// </summary>
        public static Dictionary<string, IConfiguration> ConfigDic = new Dictionary<string, IConfiguration>();

        ///// <summary>
        ///// 销毁
        ///// </summary>
        //public static void Dispose()
        //{
        //    ConfigDic = new Dictionary<string, IConfiguration>();
        //}
    }
}
