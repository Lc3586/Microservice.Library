using Api;
using Microservice.Library.Container;
using Model.Utils.Config;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [SetUpFixture]
    public class SetUp
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            //启动API
            Console.WriteLine("启动Api.");
            Task.Run(() => Program.Main(Array.Empty<string>()));

            Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    if (AutofacHelper.Container != null)
                        return;

                    await Task.Delay(100);
                }

                Assert.Fail("Api启动超时.");
            }).Wait();

            Assert.NotNull(AutofacHelper.Container, "Autofac容器为空.");

            Console.WriteLine("Api已启动.");

            var config = AutofacHelper.GetService<SystemConfig>();

            Assert.NotNull(
                config,
                "配置为空.");
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {

        }
    }
}
