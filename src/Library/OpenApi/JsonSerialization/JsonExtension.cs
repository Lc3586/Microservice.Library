using Library.OpenApi.Annotations;
using Library.OpenApi.Extention;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static string ToOpenApiJson<TOpenApiSchema>(this TOpenApiSchema obj, params string[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJson<TOpenApiSchema>(this object obj, params string[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnore<TOpenApiSchema>(this TOpenApiSchema obj, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(null, new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnore<TOpenApiSchema>(this object obj, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(null, new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJson<TOpenApiSchema>(this object obj, string[] exceptionProperties, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } },
                                             new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJsonSpecifyType<TOpenApiSchema>(this object obj, params (string, List<string>)[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnoreSpecifyType<TOpenApiSchema>(this object obj, (string, List<string>)[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static string ToOpenApiJsonSpecifyType<TOpenApiSchema>(this object obj, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new JsonPropertyContractResolver<TOpenApiSchema>(exceptionProperties, ignoreProperties)
            });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, params string[] exceptionProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObjectIgnore<TOpenApiSchema>(this string json, string[] ignoreProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(null, new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, string[] exceptionProperties, string[] ignoreProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } },
                                             new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObjectSpecifyType<TOpenApiSchema>(this string json, params (string, List<string>)[] exceptionProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObjectIgnoreSpecifyType<TOpenApiSchema>(this string json, (string, List<string>)[] ignoreProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties) where TOpenApiSchema : class
        {
            var result = JsonConvert.DeserializeObject<TOpenApiSchema>(json);
            if (result != null)
            {
                var type = typeof(TOpenApiSchema);
                var propertyDic = type.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);
                type.FilterOpenApiObject(result, propertyDic);
            }
            return result;
        }

        /// <summary>
        /// 过滤数据
        /// </summary>
        /// <typeparam name="TOpenApiSchema"></typeparam>
        /// <param name="type"></param>
        /// <param name="object"></param>
        /// <param name="propertyDic"></param>
        private static void FilterOpenApiObject<TOpenApiSchema>(this Type type, TOpenApiSchema @object, Dictionary<string, List<string>> propertyDic)
        {
            if (@object == null)
                return;

            foreach (var prop in type.GetProperties())
            {
                if (!propertyDic.ContainsKey(prop.DeclaringType.FullName))
                {
                    prop.SetValue(@object, default);
                    continue;
                }

                var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                if (schemaAttribute?.Type == OpenApiSchemaType.model)
                    prop.PropertyType.FilterOpenApiObject(prop.GetValue(@object), propertyDic);
            }
        }
    }
}
