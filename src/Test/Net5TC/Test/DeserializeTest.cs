using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Library.OpenApi.Annotations;
using Library.OpenApi.Extention;
using Library.OpenApi.JsonSerialization;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Library.ConsoleTool;
using System.Dynamic;

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

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total);

            _ = typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true);

            if (UseWatch)
            {
                Watch.Stop();
                $"预热耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        [Benchmark(Baseline = true, Description = "反序列化······")]
        public void ToObjectWithoutFilter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total);

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
                $"反序列化{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        [Benchmark(Description = "反序列化后过滤属性·")]
        public void ToObjectFilterWhenAfter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            //var objectList = TestDataJson.ToOpenApiObjectFilterWhenAfter<List<DTO.DB_ADTO.List>>();
            var objectList = testDataJson.ToOpenApiObject<List<DTO.DB_ADTO.List>>();

            if (UseWatch)
            {
                Watch.Stop();
                $"反序列化后过滤属性{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        [Benchmark(Description = "反序列化时过滤属性·")]
        public void ToObjectFilter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var objectList = testDataJson.ToOpenApiObjectA<List<DTO.DB_ADTO.List>>(null, null);

            if (UseWatch)
            {
                Watch.Stop();
                $"反序列化时过滤属性{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        //[Benchmark(Description = "过滤属性后反序列化·")]
        //public void ToObjectFilterWhenBefor()
        //{
        //    if (UseWatch)
        //        Watch.Restart();

        //    var objectList = TestDataJson.ToOpenApiObjectFilterWhenBefor<List<DTO.DB_ADTO.List>>();

        //    if (UseWatch)
        //    {
        //        Watch.Stop();
        //        $"过滤属性后反序列化{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
        //    }
        //}

        [Benchmark(Description = "转动态类型后反序列化")]
        public void ToObjectWhileDynamic()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataJson = TestData.GetJson(total);

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

            var @object = JsonConvert.DeserializeObject(testDataJson, type);
            var objectList = @object as List<object>;

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
                return new object[]
                {
                    GetDynamicType(type, propertyDic)
                };
            }
            else if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
                return new List<object>
                {
                    GetDynamicType(type, propertyDic)
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
    }
}
