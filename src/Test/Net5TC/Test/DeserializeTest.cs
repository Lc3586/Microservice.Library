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

        private string TestDataJson = string.Empty;

        private readonly Dictionary<string, MethodInfo> EnumerableMethods = new Dictionary<string, MethodInfo>();

        [GlobalSetup]
        public void Setup()
        {
            if (UseWatch)
                Watch = new Stopwatch();

            foreach (var method in typeof(Enumerable).GetMethods())
            {
                switch (method.Name)
                {
                    case "Count":
                        if (method.GetParameters().Length != 1)
                            break;
                        EnumerableMethods.Add("Count", method);
                        break;
                    case "ElementAt":
                        EnumerableMethods.Add("ElementAt", method);
                        break;
                    default:
                        break;
                }
            }

            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            TestDataJson = TestData.GetJson(total);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            _ = typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true);

            if (UseWatch)
            {
                Watch.Stop();
                $"预热耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        [Benchmark(Baseline = true, Description = "反序列化")]
        public void ToObjectWithoutFilter()
        {
            if (UseWatch)
                Watch.Restart();

            var objectList = JsonConvert.DeserializeObject<List<DTO.DB_ADTO.List>>(TestDataJson);

            if (UseWatch)
            {
                Watch.Stop();
                $"反序列化{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        [Benchmark(Description = "反序列化后过滤属性")]
        public void ToObjectFilterWhenAfter()
        {
            if (UseWatch)
                Watch.Restart();

            //var objectList = TestDataJson.ToOpenApiObjectFilterWhenAfter<List<DTO.DB_ADTO.List>>();
            var objectList = TestDataJson.ToOpenApiObject<List<DTO.DB_ADTO.List>>();

            if (UseWatch)
            {
                Watch.Stop();
                $"反序列化后过滤属性{objectList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        //[Benchmark(Description = "过滤属性后反序列化")]
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
    }
}
