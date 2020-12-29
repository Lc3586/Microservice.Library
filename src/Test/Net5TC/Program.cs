using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Net5TC.Test;
using Library.ConsoleTool;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Reports;

namespace Net5TC
{
    class Program
    {
        [GlobalSetup]
        static void Main(string[] args)
        {
            "按下任意键开始测试".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                switch (Extension.ReadInput("选择模式: \r\n\t1:简单模式\r\n\t2:BenchmarkRunner\r\n", true, "1", 1))
                {
                    case "1":
                    default:
                        "测试开始".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);

                        Environment.Version.ConsoleWrite(ConsoleColor.Cyan, "Version", true, 1);
                        RuntimeInformation.FrameworkDescription.ConsoleWrite(ConsoleColor.Cyan, "FrameworkDescription", true, 1);

                        var serializeTest = new SerializeTest
                        {
                            UseWatch = true
                        };
                        serializeTest.Setup();
                        serializeTest.ToJsonWithoutFilter();
                        serializeTest.ToJsonFilter();//最优方案
                        //serializeTest.ToJsonFilterWhenBefor();//性能差

                        var deserializeTest = new DeserializeTest
                        {
                            UseWatch = true
                        };
                        deserializeTest.Setup();
                        deserializeTest.ToObjectWithoutFilter();
                        deserializeTest.ToObjectFilter();//最优方案
                        //deserializeTest.ToObjectFilterWhenAfter();//性能差
                        //deserializeTest.ToObjectFilterWhenBefor();//性能差
                        //deserializeTest.ToObjectWhileDynamic();//暂未实现
                        break;
                    case "2":
                        "测试开始".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);

                        ManualConfig.CreateEmpty()
                            .AddJob(Job.Default
                                    .AsBaseline()
                                    .WithRuntime(ClrRuntime.Net461)
                                    .WithPlatform(Platform.AnyCpu)
                                    .WithJit(Jit.LegacyJit)
                                    .WithGcServer(false))
                            .AddJob(Job.Default
                                    .WithRuntime(CoreRuntime.Core31)
                                    .WithPlatform(Platform.AnyCpu)
                                    .WithJit(Jit.RyuJit)
                                    .WithGcServer(false))
                            .AddJob(Job.Default
                                    .WithRuntime(CoreRuntime.Core50)
                                    .WithPlatform(Platform.AnyCpu)
                                    .WithJit(Jit.RyuJit)
                                    .WithGcServer(false));

                        var config = DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true);
                        var summaryDic = new Dictionary<string, Summary>();

                        var testDic = new Dictionary<string, List<string>>
                        {
                            {
                                "Net5TC.Test.SerializeTest",
                                new List<string>
                                {
                                    "ToJsonWithoutFilter",
                                    "ToJsonFilter",
                                    //"ToJsonFilterWhenBefor"
                                }
                            },
                            {
                                "Net5TC.Test.DeserializeTest",
                                new List<string>
                                {
                                    "ToObjectWithoutFilter",
                                    "ToObjectFilter",
                                    //"ToObjectFilterWhenAfter",
                                    //"ToObjectFilterWhenBefor",
                                    //"ToObjectWhileDynamic"
                                }
                            }
                        };
                        foreach (var test in testDic)
                        {
                            var type = Type.GetType(test.Key);

                            var serializeSummary = BenchmarkRunner.Run(type, type.GetMethods().Where(m => test.Value.Contains(m.Name)).ToArray(), config);
                            summaryDic.Add(test.Key, serializeSummary);
                        }
                        break;
                }

                "测试结束!".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);

                "按下Esc键可退出测试，按下其他键重新测试".ConsoleWrite(ConsoleColor.Cyan, null, true, 1);
            }
        }
    }
}
