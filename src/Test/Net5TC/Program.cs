using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Net5TC.Test;

namespace Net5TC
{
    class Program
    {
        [GlobalSetup]
        static void Main(string[] args)
        {
            ManualConfig.CreateEmpty()
                .AddJob(Job.Default
                        .AsBaseline()
                        .WithRuntime(ClrRuntime.Net461)
                        .WithPlatform(Platform.AnyCpu)
                        .WithJit(Jit.LegacyJit)
                        .WithGcServer(true))
                .AddJob(Job.Default
                        .WithRuntime(CoreRuntime.Core31)
                        .WithPlatform(Platform.AnyCpu)
                        .WithJit(Jit.RyuJit)
                        .WithGcServer(true))
                .AddJob(Job.Default
                        .WithRuntime(CoreRuntime.Core50)
                        .WithPlatform(Platform.AnyCpu)
                        .WithJit(Jit.RyuJit)
                        .WithGcServer(true));

            Console.WriteLine("按下任意键开始测试");
            Console.ReadKey();

            Console.WriteLine("测试开始!");

            //var serializeTest = new SerializeTest();
            //serializeTest.Setup();
            //serializeTest.ToJsonFilter();
            //serializeTest.ToJsonWithoutFilter();

            //var deserializeTest = new DeserializeTest();
            //deserializeTest.Setup();
            //deserializeTest.ToObjectFilterWhenAfter();
            //deserializeTest.ToObjectFilterWhenBefor();

            var config = DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true);

            var serializeSummary = BenchmarkRunner.Run<SerializeTest>(config);

            var deserializeSummary = BenchmarkRunner.Run<DeserializeTest>(config);

            Console.WriteLine("测试结束!");

            Console.WriteLine("按下Esc键关闭窗口");
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {

            }
        }
    }
}
