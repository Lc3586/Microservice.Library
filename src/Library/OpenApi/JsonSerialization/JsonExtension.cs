using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.OpenApi.JsonSerialization
{
    /// <summary>
    /// Json拓展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJson<TOpenApiSchema>(this object obj, params string[] exceptionProperties)
        {
            return obj.ToJson<TOpenApiSchema>(exceptionProperties, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJsonIgnore<TOpenApiSchema>(this object obj, params string[] ignoreProperties)
        {
            return obj.ToJson<TOpenApiSchema>(null, ignoreProperties);
        }


        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJson<TOpenApiSchema>(this object obj, string[] exceptionProperties, string[] ignoreProperties)
        {

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new JsonPropertyContractResolver<TOpenApiSchema>(exceptionProperties, ignoreProperties)
            });
        }
    }
}
