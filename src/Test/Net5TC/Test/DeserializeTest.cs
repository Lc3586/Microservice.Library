using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microservice.Library.OpenApi.Annotations;
using Microservice.Library.OpenApi.Extention;
using Microservice.Library.OpenApi.JsonSerialization;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microservice.Library.ConsoleTool;
using System.Dynamic;
using System.Linq;
using Microservice.Library.FreeSql.Extention;

namespace Net5TC.Test
{
    /// <summary>
    /// 反序列化测试
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [RPlotExporter]
    public class DeserializeTest
    {
        public bool UseWatch = false;

        private Stopwatch Watch;

        [GlobalSetup]
        public void Setup()
        {
            if (UseWatch)
                Watch = new Stopwatch();

            if (UseWatch)
                Watch.Restart();

            _ = typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true);

            if (UseWatch)
            {
                Watch.Stop();
                $"预热耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        /// <summary>
        /// 基础反序列化
        /// </summary>
        [Benchmark(Baseline = true, Description = "基础反序列化····")]
        public void ToObjectWithoutFilter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var objectList = JsonConvert.DeserializeObject<List<DTO.DB_ADTO.List>>(testDataJson);

            if (UseWatch)
            {
                Watch.Stop();
                $"基础反序列化{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        /// <summary>
        /// 反序列化时过滤属性
        /// </summary>
        /// <remarks>最优方案</remarks>
        [Benchmark(Description = "反序列化时过滤属性·")]
        public void ToObjectFilter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var objectList = testDataJson.ToOpenApiObject<List<DTO.DB_ADTO.List>>();

            if (UseWatch)
            {
                Watch.Stop();
                $"反序列化时过滤属性{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        #region 反序列化后过滤属性

        /// <summary>
        /// 反序列化后过滤属性
        /// </summary>
        [Benchmark(Description = "反序列化后过滤属性·")]
        public void ToObjectFilterWhenAfter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var objectList = ToOpenApiObjectFilterWhenAfter<List<DTO.DB_ADTO.List>>(testDataJson);

            if (UseWatch)
            {
                Watch.Stop();
                $"反序列化后过滤属性{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        /// <summary>
        /// 反序列化后过滤属性
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        private TOpenApiSchema ToOpenApiObjectFilterWhenAfter<TOpenApiSchema>(string json, Dictionary<string, List<string>> exceptionProperties = null, Dictionary<string, List<string>> ignoreProperties = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            var @object = JsonConvert.DeserializeObject<TOpenApiSchema>(json);

            if (@object.Equals(default(TOpenApiSchema)))
                return default;

            var type = typeof(TOpenApiSchema);

            var propertyDic = type.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);

            FilterOpenApiObject(@object, type, propertyDic);

            return @object;
        }

        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="object"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        private void FilterOpenApiObject(object @object, Type type, Dictionary<string, List<string>> propertyDic)
        {
            if (@object == null)
                return;

            var isEnumerable = false;
            if (type.IsArray)
            {
                type = type.Assembly.GetType(type.FullName.Replace("[]", string.Empty));
                isEnumerable = true;
            }
            else if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
                isEnumerable = true;
            }

            if (isEnumerable)
                Foreach(@object, type, propertyDic);
            else
            {
                foreach (var prop in type.GetProperties())
                {
                    if (!propertyDic[type.FullName].Contains(prop.Name))
                    {
                        prop.SetValue(@object, default);
                        continue;
                    }

                    var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                    if (schemaAttribute?.Type == OpenApiSchemaType.model)
                        FilterOpenApiObject(prop.GetValue(@object), prop.PropertyType, propertyDic);
                }
            }
        }

        /// <summary>
        /// 遍历集合
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        private void Foreach(object objectList, Type type, Dictionary<string, List<string>> propertyDic)
        {
            var count = (int)TestData.EnumerableMethods["Count"].MakeGenericMethod(type)
                                                        .Invoke(objectList, new object[] { objectList });

            for (int i = 0; i < count; i++)
            {
                var @object = TestData.EnumerableMethods["ElementAt"].MakeGenericMethod(type)
                                                            .Invoke(null, new object[] { objectList, i });

                FilterOpenApiObject(@object, type, propertyDic);
            }
        }

        #endregion

        #region 过滤属性后反序列化

        /// <summary>
        /// 过滤属性后反序列化
        /// </summary>
        [Benchmark(Description = "过滤属性后反序列化·")]
        public void ToObjectFilterWhenBefor()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var objectList = ToOpenApiObjectFilterWhenBefor<List<DTO.DB_ADTO.List>>(testDataJson);

            if (UseWatch)
            {
                Watch.Stop();
                $"过滤属性后反序列化{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        /// <summary>
        /// 过滤属性后反序列化
        /// </summary>
        /// <typeparam name="TOpenApiSchema">指定接口架构类型</typeparam>
        /// <param name="json">需要反序列化的Json字符串</param>
        /// <param name="exceptionProperties">特别输出的属性</param>
        /// <param name="ignoreProperties">特别忽略的属性</param>
        /// <remarks>如果在特别输出参数和特别忽略参数中同时指定了同一个属性，那么最终不会输出该属性</remarks>
        /// <returns></returns>
        private TOpenApiSchema ToOpenApiObjectFilterWhenBefor<TOpenApiSchema>(string json, Dictionary<string, List<string>> exceptionProperties = null, Dictionary<string, List<string>> ignoreProperties = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            var type = typeof(TOpenApiSchema);

            var propertyDic = type.GetOrNullForPropertyDic(true, exceptionProperties, ignoreProperties);

            var jt = JToken.Parse(json);

            FilterOpenApiObject(jt, type, propertyDic);

            return jt.ToObject<TOpenApiSchema>();
        }

        /// <summary>
        /// 过滤属性
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        private void FilterOpenApiObject(JToken jt, Type type, Dictionary<string, List<string>> propertyDic)
        {
            if (jt == null)
                return;

            var isEnumerable = false;
            if (type.IsArray)
            {
                type = type.Assembly.GetType(type.FullName.Replace("[]", string.Empty));
                isEnumerable = true;
            }
            else if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
                isEnumerable = true;
            }

            if (isEnumerable)
            {
                Foreach(jt, type, propertyDic);
            }
            else
            {
                var jp = jt.First as JProperty;
                while (jp != null)
                {
                    var jp_next = jp.Next as JProperty;

                    if (!propertyDic[type.FullName].Contains(jp.Name))
                        jp.Remove();
                    else
                    {
                        var prop = type.GetProperty(jp.Name);
                        var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                        if (schemaAttribute?.Type == OpenApiSchemaType.model)
                            FilterOpenApiObject(jp.Value, prop.PropertyType, propertyDic);
                    }

                    jp = jp_next;
                }
            }
        }

        /// <summary>
        /// 遍历集合
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="type"></param>
        /// <param name="propertyDic"></param>
        private void Foreach(JToken jt, Type type, Dictionary<string, List<string>> propertyDic)
        {
            foreach (var item in jt)
            {
                FilterOpenApiObject(item, type, propertyDic);
            }
        }

        #endregion

        #region 转动态类型后反序列化

        /// <summary>
        /// 转动态类型后反序列化
        /// </summary>
        [Benchmark(Description = "转动态类型后反序列化")]
        public void ToObjectWhileDynamic()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var propertyDic = typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true);
            var dynamicType = GetDynamicType(typeof(List<DTO.DB_ADTO.List>), propertyDic);
            var type = dynamicType.GetType();

            foreach (var item in type.GetProperties())
            {
                Console.WriteLine(item.Name);
            }

            var @dynamic = JsonConvert.DeserializeObject(testDataJson, type, new JsonSerializerSettings
            {
                ContractResolver = new OpenApiContractResolver(typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true))
            });
            //var @dynamic = JsonConvert.DeserializeObject(testDataJson, type);
            var dynamicList = @dynamic as List<ExpandoObject>;

            var objectList = dynamicList.Select(o => SelectExtension.SelectExpandoObject<DTO.DB_ADTO.List>().Invoke(o)).ToList();


            if (UseWatch)
            {
                Watch.Stop();
                $"转动态类型后反序列化{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        private object GetDynamicType(Type type, Dictionary<string, List<string>> propertyDic)
        {
            if (type.IsArray)
            {
                type = type.Assembly.GetType(type.FullName.Replace("[]", string.Empty));
                return new ExpandoObject[]
                {
                    GetDynamicType(type, propertyDic) as ExpandoObject
                };
            }
            else if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
                return new List<ExpandoObject>
                {
                    GetDynamicType(type, propertyDic) as ExpandoObject
                };
            }

            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            foreach (var prop in type.GetProperties())
            {
                if (!propertyDic[type.FullName].Contains(prop.Name))
                    continue;

                var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                if (schemaAttribute?.Type == OpenApiSchemaType.model)
                    expandoObject.Add(prop.Name, GetDynamicType(prop.PropertyType, propertyDic));
                else
                    expandoObject.Add(prop.Name, prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null);
            }

            return expandoObject;
        }

        #endregion
    }
}
