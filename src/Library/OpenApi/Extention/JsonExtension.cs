using Library.OpenApi.JsonSerialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.OpenApi.Extention
{
    /// <summary>
    /// Json相关拓展方法
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// // * 测试信息 *
    /// 
    /// // * 数据量：100条 *
    /// 
    /// // * 数据结构：
    /// {
    ///     "DB_B":{
    ///         "Id":"1343405244365029377",
    ///         "Name":"名称",
    ///         "CreatorId":"800728e2-5997-42d6-b1d7-dd3ce6a51562",
    ///         "CreatorName":"管理员",
    ///         "CreateTime":"2020-12-28 11:55:44",
    ///         "ModifyTime":"2020-12-28 11:55:44"
    ///     },
    ///     "DB_Cs":[   //集合内数据量 10条
    ///         {
    ///             "Id":"1343405244365029378",
    ///             "Name":"名称",
    ///             "CreatorId":"f2f0d3e0-f81b-4631-ac33-41729bf36d49",
    ///             "CreatorName":"管理员",
    ///             "CreateTime":"2020-12-28 11:55:44",
    ///             "ModifyTime":"2020-12-28 11:55:44"
    ///         }
    ///     ],
    ///     "DB_Ds":[   //集合内数据量 10条
    ///         {
    ///             "Id":"1343405244365029388",
    ///             "AId":"1343405244365029376",
    ///             "Name":"名称",
    ///             "CreatorId":"29b52d6e-15b6-4c83-b849-8169e168718c",
    ///             "CreatorName":"管理员",
    ///             "CreateTime":"2020-12-28 11:55:44",
    ///             "ModifyTime":"2020-12-28 11:55:44"
    ///         }
    ///     ],
    ///     "Id":"1343405244365029376",
    ///     "ParentId":null,
    ///     "BId":"1343405244365029377",
    ///     "Name":"名称",
    ///     "Content":"内容",
    ///     "CreatorId":"3ad97ec8-57ca-4088-a620-b36c50566748",
    ///     "CreatorName":"管理员",
    ///     "CreateTime":"2020-12-28 11:55:44",
    ///     "ModifyTime":"2020-12-28 11:55:44"
    /// } *
    /// 
    /// 
    /// 
    /// // * 序列化 *
    /// // * Summary *
    /// BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.53 (1903/May2019Update/19H1)
    /// Intel Core i5-9400 CPU 2.90GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
    /// .NET Core SDK=5.0.101
    /// [Host]        : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
    /// .NET 4.6.1    : .NET Framework 4.8 (4.8.4250.0), X64 RyuJIT
    /// .NET Core 3.1 : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
    /// .NET Core 5.0 : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
    /// |   Method |           Job |       Runtime |      Mean |     Error |    StdDev | Ratio |
    /// |--------- |-------------- |-------------- |----------:|----------:|----------:|------:|
    /// | 基础序列化··· |    .NET 4.6.1 |    .NET 4.6.1 |  7.072 ms | 0.0200 ms | 0.0187 ms |  1.00 |
    /// | 序列化时过滤属性 |    .NET 4.6.1 |    .NET 4.6.1 | 18.209 ms | 0.0720 ms | 0.0638 ms |  2.57 |
    /// | 基础序列化··· | .NET Core 3.1 | .NET Core 3.1 |  6.324 ms | 0.0263 ms | 0.0246 ms |  0.89 |                
    /// | 序列化时过滤属性 | .NET Core 3.1 | .NET Core 3.1 | 14.582 ms | 0.0422 ms | 0.0394 ms |  2.06 |
    /// | 基础序列化··· | .NET Core 5.0 | .NET Core 5.0 |  6.004 ms | 0.0199 ms | 0.0186 ms |  0.85 |
    /// | 序列化时过滤属性 | .NET Core 5.0 | .NET Core 5.0 | 14.130 ms | 0.0500 ms | 0.0467 ms |  2.00 |
    /// // * Hints *
    /// Outliers
    /// SerializeTest.序列化时过滤属性: .NET 4.6.1 -> 1 outlier  was  removed (18.40 ms)
    /// 
    /// 
    /// // * 反序列化 *
    /// // * Summary *
    /// BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.53 (1903/May2019Update/19H1)
    /// Intel Core i5-9400 CPU 2.90GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
    /// .NET Core SDK=5.0.101
    /// [Host]        : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
    /// .NET 4.6.1    : .NET Framework 4.8 (4.8.4250.0), X64 RyuJIT
    /// .NET Core 3.1 : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
    /// .NET Core 5.0 : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
    /// |     Method |           Job |       Runtime |     Mean |    Error |   StdDev | Ratio | RatioSD |
    /// |----------- |-------------- |-------------- |---------:|---------:|---------:|------:|--------:|
    /// | 基础反序列化···· |    .NET 4.6.1 |    .NET 4.6.1 | 13.67 ms | 0.096 ms | 0.090 ms |  1.00 |    0.00 |
    /// | 反序列化时过滤属性· |    .NET 4.6.1 |    .NET 4.6.1 | 26.54 ms | 0.127 ms | 0.119 ms |  1.94 |    0.02 |
    /// | 基础反序列化···· | .NET Core 3.1 | .NET Core 3.1 | 12.82 ms | 0.240 ms | 0.224 ms |  0.94 |    0.02 |
    /// | 反序列化时过滤属性· | .NET Core 3.1 | .NET Core 3.1 | 22.97 ms | 0.136 ms | 0.127 ms |  1.68 |    0.01 |
    /// | 基础反序列化···· | .NET Core 5.0 | .NET Core 5.0 | 11.53 ms | 0.111 ms | 0.104 ms |  0.84 |    0.01 |
    /// | 反序列化时过滤属性· | .NET Core 5.0 | .NET Core 5.0 | 21.35 ms | 0.187 ms | 0.175 ms |  1.56 |    0.02 |
    /// // * Legends *
    /// Mean    : Arithmetic mean of all measurements
    /// Error   : Half of 99.9% confidence interval
    /// StdDev  : Standard deviation of all measurements
    /// Ratio   : Mean of the ratio distribution ([Current]/[Baseline])
    /// RatioSD : Standard deviation of the ratio distribution ([Current]/[Baseline])
    /// 1 ms    : 1 Millisecond (0.001 sec)
    /// // ***** BenchmarkRunner: End *****
    /// // ** Remained 0 benchmark(s) to run **
    /// Run time: 00:02:35 (155.22 sec), executed benchmarks: 6
    /// Global total time: 00:02:44 (164.31 sec), executed benchmarks: 6
    /// ]]>
    /// </remarks>
    public static class JsonExtension
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJson(this object obj, Type openApiSchemaType, params string[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType(openApiSchemaType, exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnore(this object obj, Type openApiSchemaType, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType(openApiSchemaType, null, ignoreProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJson(this object obj, Type openApiSchemaType, string[] exceptionProperties, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType(openApiSchemaType, exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, exceptionProperties?.ToList() } },
                                             ignoreProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonSpecifyType(this object obj, Type openApiSchemaType, params (string, List<string>)[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType(openApiSchemaType, exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnoreSpecifyType(this object obj, Type openApiSchemaType, (string, List<string>)[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType(openApiSchemaType, null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonSpecifyType(this object obj, Type openApiSchemaType, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new OpenApiContractResolver(openApiSchemaType.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties))
            });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJson<TOpenApiSchema>(this TOpenApiSchema obj, params string[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJson<TOpenApiSchema>(this object obj, params string[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnore<TOpenApiSchema>(this TOpenApiSchema obj, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(null, new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnore<TOpenApiSchema>(this object obj, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(null, new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJson<TOpenApiSchema>(this object obj, string[] exceptionProperties, string[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } },
                                             new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonSpecifyType<TOpenApiSchema>(this object obj, params (string, List<string>)[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonIgnoreSpecifyType<TOpenApiSchema>(this object obj, (string, List<string>)[] ignoreProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJsonSpecifyType<TOpenApiSchema>(this object obj, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new OpenApiContractResolver<TOpenApiSchema>(typeof(TOpenApiSchema).GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties))
            });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static object ToOpenApiObject(this string json, Type openApiSchemaType, params string[] exceptionProperties)
        {
            return json.ToOpenApiObject(openApiSchemaType, exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, exceptionProperties?.ToList() } }, null);
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static object ToOpenApiObjectIgnore(this string json, Type openApiSchemaType, string[] ignoreProperties)
        {
            return json.ToOpenApiObject(openApiSchemaType, null, ignoreProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static object ToOpenApiObject(this string json, Type openApiSchemaType, string[] exceptionProperties, string[] ignoreProperties)
        {
            return json.ToOpenApiObject(openApiSchemaType, exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, exceptionProperties?.ToList() } },
                                            ignoreProperties == null ? null : new Dictionary<string, List<string>>() { { openApiSchemaType.FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static object ToOpenApiObjectSpecifyType(this string json, Type openApiSchemaType, params (string, List<string>)[] exceptionProperties)
        {
            return json.ToOpenApiObject(openApiSchemaType, exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static object ToOpenApiObjectIgnoreSpecifyType(this string json, Type openApiSchemaType, (string, List<string>)[] ignoreProperties)
        {
            return json.ToOpenApiObject(openApiSchemaType, null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="openApiSchemaType">指定接口架构类型</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        public static object ToOpenApiObject(this string json, Type openApiSchemaType, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties)
        {
            return JsonConvert.DeserializeObject(json, openApiSchemaType, new JsonSerializerSettings
            {
                ContractResolver = new OpenApiContractResolver(openApiSchemaType.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties))
            });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, params string[] exceptionProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
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
            return json.ToOpenApiObject<TOpenApiSchema>(null, ignoreProperties == null ? null : new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, string[] exceptionProperties, string[] ignoreProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(exceptionProperties == null ? null : new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } },
                                             ignoreProperties == null ? null : new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObjectSpecifyType<TOpenApiSchema>(this string json, params (string, List<string>)[] exceptionProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(exceptionProperties?.ToDictionary(k => k.Item1, v => v.Item2), null);
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObjectIgnoreSpecifyType<TOpenApiSchema>(this string json, (string, List<string>)[] ignoreProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(null, ignoreProperties?.ToDictionary(k => k.Item1, v => v.Item2));
        }

        /// <summary>
        /// 将Json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, Dictionary<string, List<string>> exceptionProperties, Dictionary<string, List<string>> ignoreProperties) where TOpenApiSchema : class
        {
            return JsonConvert.DeserializeObject<TOpenApiSchema>(json, new JsonSerializerSettings
            {
                ContractResolver = new OpenApiContractResolver<TOpenApiSchema>(typeof(TOpenApiSchema).GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties))
            });
        }
    }
}
