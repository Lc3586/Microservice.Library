using Microservice.Library.Container;
using Microservice.Library.Soap.Gen;
using Model.Example.SoapDTO;
using NUnit.Framework;
using SoapServices.Example;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnitTest.Extension;
using Object = Model.Example.SoapDTO.Object;

namespace UnitTest.Testing.Soap
{
    /// <summary>
    /// Soap测试
    /// </summary>
    [TestFixture(Author = "LCTR", TestName = "测试SampleService", Description = "测试 SampleService")]
    [Ignore("忽略测试")]
    public class Sample
    {
        public Sample()
        {

        }

        private ISampleService Service;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Service = AutofacHelper.GetService<ISoapClientProvider>().GetClient<ISampleService>();

            Console.WriteLine("SampleService 测试开始.");
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {

            Console.WriteLine("SampleService 测试结束.");
        }

        [Test(Author = "LCTR", Description = "SampleService Ping")]
        public void Ping()
        {
            var request = "test";

            var response = Service.Ping(request);

            Assert.AreEqual(
                request,
                response,
                "Ping 方法输入与输出不一致.");

            Assert.Pass("SampleService Ping 方法正常.");
        }

        [Test(Author = "LCTR", Description = "SampleService PingComplexModel")]
        public void PingComplexModel()
        {
            var request = new Input
            {
                StringProperty = "测试内容.",
                IntProperty = 666,
                ListProperty = new List<string> { "测试0.", "测试1." },
                DateTimeOffsetProperty = DateTimeOffset.Now,
                ComplexListProperty = new List<Object>
                {
                     new Object
                     {
                          StringProperty = "测试内容.",
                          IntProperty = 666
                     }
                }
            };

            var response = Service.PingComplexModel(request);

            request.AreEqual(
                null,
                response);

            Assert.Pass("SampleService PingComplexModel 方法正常.");
        }

        [Test(Author = "LCTR", Description = "SampleService VoidMethod")]
        public void VoidMethod()
        {
            Service.VoidMethod(out string response);

            Assert.NotNull(
                response,
                "VoidMethod 方法未返回任何内容.");

            Assert.Pass("SampleService VoidMethod 方法正常.");
        }

        [Test(Author = "LCTR", Description = "SampleService AsyncMethod")]
        public void AsyncMethod()
        {
            var response = Service.AsyncMethod().Result;
            Assert.Greater(
                response,
                0,
                "AsyncMethod 方法返回值有误.");

            Assert.Pass("SampleService AsyncMethod 方法正常.");
        }

        [Test(Author = "LCTR", Description = "SampleService NullableMethod")]
        public void NullableMethod()
        {
            var response = Service.NullableMethod(null);
            Assert.Null(
                response,
                "NullableMethod 方法返回值有误.");

            Assert.Pass("SampleService NullableMethod 方法正常.");
        }

        [Test(Author = "LCTR", Description = "SampleService XmlMethod")]
        public void XmlMethod()
        {
            var request = XElement.Parse("<text>测试</text>");

            Service.XmlMethod(request);

            Assert.Pass("SampleService XmlMethod 方法正常.");
        }
    }
}
