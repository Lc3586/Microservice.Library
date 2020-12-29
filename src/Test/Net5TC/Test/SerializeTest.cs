using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Library.OpenApi.Extention;
using Library.OpenApi.JsonSerialization;
using Library.ConsoleTool;
using System.Diagnostics;
using System.Dynamic;
using Library.OpenApi.Annotations;
using System.Reflection;

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

        [GlobalSetup]
        public void Setup()
        {
            if (UseWatch)
                Watch = new Stopwatch();

            if (UseWatch)
                Watch.Restart();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataList = TestData.GetList(total, true);

            _ = typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true);

            if (UseWatch)
            {
                Watch.Stop();
                $"预热耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        /// <summary>
        /// 基础序列化
        /// </summary>
        [Benchmark(Baseline = true, Description = "基础序列化···")]
        public void ToJsonWithoutFilter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataList = TestData.GetList(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var json = JsonConvert.SerializeObject(testDataList);

            if (UseWatch)
            {
                Watch.Stop();
                $"基础序列化{testDataList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        /// <summary>
        /// 序列化时过滤属性
        /// </summary>
        /// <remarks>最优方案</remarks>
        [Benchmark(Description = "序列化时过滤属性")]
        public void ToJsonFilter()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataList = TestData.GetList(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var json = testDataList.ToOpenApiJson();

            if (UseWatch)
            {
                Watch.Stop();
                $"序列化时过滤属性{testDataList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        #region 过滤属性后序列化

        /// <summary>
        /// 过滤属性后序列化
        /// </summary>
        [Benchmark(Description = "过滤属性后序列化")]
        public void ToJsonFilterWhenBefor()
        {
            if (UseWatch)
                Watch.Start();

            var total = 100;// Convert.ToInt32(Extension.ReadInput("输入要测试的数据量: ", true, "100000"));
            var testDataList = TestData.GetList(total, true);

            if (UseWatch)
            {
                Watch.Stop();
                $"准备{total}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }

            if (UseWatch)
                Watch.Restart();

            var propertyDic = typeof(List<DTO.DB_ADTO.List>).GetOrNullForPropertyDic(true);
            var type = typeof(List<DTO.DB_ADTO.List>);
            var expandoObject = FilterObjectPropertys(testDataList, type, propertyDic);
            var json = JsonConvert.SerializeObject(expandoObject);

            if (UseWatch)
            {
                Watch.Stop();
                $"过滤属性后序列化{testDataList?.Count}条数据耗时 {Watch.ElapsedMilliseconds} ms".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }

        private object FilterObjectPropertys(object obj, Type type, Dictionary<string, List<string>> propertyDic)
        {
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
                return Foreach(obj, type, propertyDic);
            else
            {
                var expandoObject = new ExpandoObject() as IDictionary<string, object>;
                foreach (var prop in type.GetProperties())
                {
                    if (!propertyDic[type.FullName].Contains(prop.Name))
                        continue;

                    var schemaAttribute = prop.GetCustomAttribute<OpenApiSchemaAttribute>();
                    if (schemaAttribute?.Type == OpenApiSchemaType.model)
                        expandoObject.Add(prop.Name, FilterObjectPropertys(prop.GetValue(obj), prop.PropertyType, propertyDic));
                    else
                        expandoObject.Add(prop.Name, prop.GetValue(obj));
                }

                return expandoObject;
            }
        }

        private object Foreach(object objectList, Type type, Dictionary<string, List<string>> propertyDic)
        {
            var expandoObjectList = new List<object>();

            var count = (int)TestData.EnumerableMethods["Count"].MakeGenericMethod(type)
                                            .Invoke(objectList, new object[] { objectList });

            for (int i = 0; i < count; i++)
            {
                var @object = TestData.EnumerableMethods["ElementAt"].MakeGenericMethod(type)
                                                            .Invoke(null, new object[] { objectList, i });

                expandoObjectList.Add(FilterObjectPropertys(@object, type, propertyDic));
            }

            return expandoObjectList;
        }

        #endregion
    }
}
