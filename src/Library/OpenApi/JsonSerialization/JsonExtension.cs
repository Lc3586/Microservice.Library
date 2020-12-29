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
    /// | 基础序列化··· |    .NET 4.6.1 |    .NET 4.6.1 |  7.094 ms | 0.0170 ms | 0.0151 ms |  1.00 |
    /// | 序列化时过滤属性 |    .NET 4.6.1 |    .NET 4.6.1 | 17.751 ms | 0.0573 ms | 0.0508 ms |  2.50 |
    /// | 基础序列化··· | .NET Core 3.1 | .NET Core 3.1 |  6.269 ms | 0.0191 ms | 0.0169 ms |  0.88 |
    /// | 序列化时过滤属性 | .NET Core 3.1 | .NET Core 3.1 | 14.430 ms | 0.0603 ms | 0.0535 ms |  2.03 |
    /// | 基础序列化··· | .NET Core 5.0 | .NET Core 5.0 |  5.945 ms | 0.0200 ms | 0.0187 ms |  0.84 |
    /// | 序列化时过滤属性 | .NET Core 5.0 | .NET Core 5.0 | 13.958 ms | 0.0586 ms | 0.0520 ms |  1.97 |
    /// // * Hints *
    /// Outliers
    /// SerializeTest.基础序列化···: .NET 4.6.1    -> 1 outlier  was  removed (7.15 ms)
    /// SerializeTest.序列化时过滤属性: .NET 4.6.1    -> 1 outlier  was  removed (17.89 ms)
    /// SerializeTest.基础序列化···: .NET Core 3.1 -> 1 outlier  was  removed (6.38 ms)
    /// SerializeTest.序列化时过滤属性: .NET Core 3.1 -> 1 outlier  was  removed (14.55 ms)
    /// SerializeTest.序列化时过滤属性: .NET Core 5.0 -> 1 outlier  was  removed (14.11 ms)
    /// // * Legends *
    /// Mean   : Arithmetic mean of all measurements
    /// Error  : Half of 99.9% confidence interval
    /// StdDev : Standard deviation of all measurements
    /// Ratio  : Mean of the ratio distribution ([Current]/[Baseline])
    /// 1 ms   : 1 Millisecond (0.001 sec)
    /// // ***** BenchmarkRunner: End *****
    /// // ** Remained 0 benchmark(s) to run **
    /// Run time: 00:02:14 (134.19 sec), executed benchmarks: 6
    /// Global total time: 00:02:24 (144.92 sec), executed benchmarks: 6
    /// // * Artifacts cleanup *
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
    /// | 基础反序列化···· |    .NET 4.6.1 |    .NET 4.6.1 | 13.76 ms | 0.069 ms | 0.061 ms |  1.00 |    0.00 |
    /// | 反序列化时过滤属性· |    .NET 4.6.1 |    .NET 4.6.1 | 26.78 ms | 0.385 ms | 0.360 ms |  1.95 |    0.02 |
    /// | 基础反序列化···· | .NET Core 3.1 | .NET Core 3.1 | 12.71 ms | 0.135 ms | 0.126 ms |  0.92 |    0.01 |
    /// | 反序列化时过滤属性· | .NET Core 3.1 | .NET Core 3.1 | 23.81 ms | 0.300 ms | 0.280 ms |  1.73 |    0.02 |
    /// | 基础反序列化···· | .NET Core 5.0 | .NET Core 5.0 | 11.44 ms | 0.089 ms | 0.084 ms |  0.83 |    0.01 |
    /// | 反序列化时过滤属性· | .NET Core 5.0 | .NET Core 5.0 | 21.44 ms | 0.224 ms | 0.210 ms |  1.56 |    0.02 |
    /// // * Hints *
    /// Outliers
    /// DeserializeTest.基础反序列化····: .NET 4.6.1 -> 1 outlier  was  removed (14.21 ms)
    /// // * Legends *
    /// Mean    : Arithmetic mean of all measurements
    /// Error   : Half of 99.9% confidence interval
    /// StdDev  : Standard deviation of all measurements
    /// Ratio   : Mean of the ratio distribution ([Current]/[Baseline])
    /// RatioSD : Standard deviation of the ratio distribution ([Current]/[Baseline])
    /// 1 ms    : 1 Millisecond (0.001 sec)
    /// // ***** BenchmarkRunner: End *****
    /// // ** Remained 0 benchmark(s) to run **
    /// Run time: 00:02:12 (132.55 sec), executed benchmarks: 6
    /// Global total time: 00:03:32 (212.22 sec), executed benchmarks: 6
    /// // * Artifacts cleanup *
    /// ]]>
    /// </remarks>
    public static class JsonExtension
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <returns></returns>
        public static string ToOpenApiJson<TOpenApiSchema>(this TOpenApiSchema obj, params string[] exceptionProperties)
        {
            return obj.ToOpenApiJsonSpecifyType<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } }, null);
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
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
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
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">要例外输出的属性</param>
        /// <param name="ignoreProperties">忽略的属性</param>
        /// <returns></returns>
        public static TOpenApiSchema ToOpenApiObject<TOpenApiSchema>(this string json, string[] exceptionProperties, string[] ignoreProperties) where TOpenApiSchema : class
        {
            return json.ToOpenApiObject<TOpenApiSchema>(new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, exceptionProperties?.ToList() } },
                                             new Dictionary<string, List<string>>() { { typeof(TOpenApiSchema).FullName, ignoreProperties?.ToList() } });
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
