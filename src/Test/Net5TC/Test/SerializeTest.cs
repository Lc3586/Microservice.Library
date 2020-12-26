using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Coldairarrow.Util;
using Library.OpenApi.Extention;
using Library.OpenApi.JsonSerialization;
using Library.ConsoleTool;
using System.Diagnostics;

namespace Net5TC.Test
{
    /// <summary>
    /// 序列化测试
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net461, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [RPlotExporter]
    public class SerializeTest
    {
        public bool UseWatch = false;

        private Stopwatch Watch;

        private List<DTO.DB_ADTO.List> TestDataList;

        [GlobalSetup]
        public void Setup()
        {
            if (UseWatch)
                Watch = new Stopwatch();

            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            TestDataList = TestData.GetList(total);

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

        [Benchmark(Baseline = true, Description = "序列化")]
        public void ToJsonWithoutFilter()
        {
            if (UseWatch)
                Watch.Restart();

            var json = JsonConvert.SerializeObject(TestDataList);

            if (UseWatch)
            {
                Watch.Stop();
                $"序列化{TestDataList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        [Benchmark(Description = "过滤属性的序列化")]
        public void ToJsonFilter()
        {
            if (UseWatch)
                Watch.Restart();

            var json = TestDataList.ToOpenApiJson();

            if (UseWatch)
            {
                Watch.Stop();
                $"过滤属性的序列化{TestDataList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }
    }
}
