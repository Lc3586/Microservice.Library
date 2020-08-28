using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static string ToJson<TOpenApiSchema>(this TOpenApiSchema obj, params string[] exceptionProperties)
        {
            return obj.ToJsonSpecifyType<TOpenApiSchema>(new Dictionary<Type, List<string>>() { { typeof(TOpenApiSchema), exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJson<TOpenApiSchema>(this object obj, params string[] exceptionProperties)
        {
            return obj.ToJsonSpecifyType<TOpenApiSchema>(new Dictionary<Type, List<string>>() { { typeof(TOpenApiSchema), exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJsonIgnore<TOpenApiSchema>(this TOpenApiSchema obj, string[] ignoreProperties)
        {
            return obj.ToJsonSpecifyType<TOpenApiSchema>(null, new Dictionary<Type, List<string>>() { { typeof(TOpenApiSchema), ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJsonIgnore<TOpenApiSchema>(this object obj, string[] ignoreProperties)
        {
            return obj.ToJsonSpecifyType<TOpenApiSchema>(null, new Dictionary<Type, List<string>>() { { typeof(TOpenApiSchema), ignoreProperties?.ToList() } });
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
            return obj.ToJsonSpecifyType<TOpenApiSchema>(new Dictionary<Type, List<string>>() { { typeof(TOpenApiSchema), exceptionProperties?.ToList() } },
                                             new Dictionary<Type, List<string>>() { { typeof(TOpenApiSchema), ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJsonSpecifyType<TOpenApiSchema>(this object obj, params (Type, List<string>)[] exceptionProperties)
        {
            return obj.ToJsonSpecifyType<TOpenApiSchema>(exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJsonIgnoreSpecifyType<TOpenApiSchema>(this object obj, (Type, List<string>)[] ignoreProperties)
        {
            return obj.ToJsonSpecifyType<TOpenApiSchema>(null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToJsonSpecifyType<TOpenApiSchema>(this object obj, Dictionary<Type, List<string>> exceptionProperties, Dictionary<Type, List<string>> ignoreProperties)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new JsonPropertyContractResolver<TOpenApiSchema>(exceptionProperties, ignoreProperties)
            });
        }
    }
}
