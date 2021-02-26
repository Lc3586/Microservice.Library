using Microservice.Library.Container;
using Microservice.Library.Extension;
using Microservice.Library.FreeSql.Extention;
using Microservice.Library.FreeSql.Gen;
using Microservice.Library.Http;
using Model.Utils.Result;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using UnitTest.Config;
using UnitTest.Extension;

namespace Tests.Testing.Controllers
{
    /// <summary>
    /// 接口测试
    /// </summary>
    [TestFixture(Author = "LCTR", TestName = "测试SampleController", Description = "简单示例")]
    public class Sample
    {
        private ControllerConfig Config;

        private ControllerTestSetting Controller;

        private IFreeSqlMultipleProvider<string> Orms;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Config = ControllerConfig.GetData();

            Assert.NotNull(
                   Config,
                   "Controller.json配置有误.");

            Assert.NotNull(
                   Config.Controllers,
                   "Controllers配置为空.");

            Assert.NotZero(
                   Config.Controllers.Count,
                   "Controllers配置为空.");

            Assert.IsTrue(
                Config.Controllers.ContainsKey("SV1_7"),
                "Controller 配置未包含 SV1_7");

            Controller = Config.Controllers["SV1_7"];

            Assert.NotNull(
                   Controller,
                   "Controller SV1_7 配置为空.");

            Assert.IsTrue(
                   Controller.Actions.Any_Ex(),
                   "Controller Actions配置为空.");

            Orms = AutofacHelper.GetService<IFreeSqlMultipleProvider<string>>();

            Assert.NotNull(
                   Orms,
                   "数据库实例构造器为null.");

            Console.WriteLine("Controller 测试开始.");
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            Console.WriteLine("Controller 测试结束.");
        }

        /// <summary>
        /// 设置测试数据
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="cmds">查询语句</param>
        private void SetupTestData(int count, List<string> cmds)
        {
            if (!Controller.EnableDatabaseCmd)
                return;

            if (!cmds.Any_Ex())
                return;

            Assert.IsTrue(
                Orms.Exists("Oracle-MZYY01"),
                "Oracle-MZYY01 数据库未注册.");

            var Orm = Orms.GetFreeSql("Oracle-MZYY01");

            Assert.NotNull(
                Orm,
                $"Oracle-MZYY01 数据库实例为空.");

            (bool success, Exception ex) = Orm.RunTransaction(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    cmds.ForEach(c =>
                    {
                        var rows = Orm.Ado.ExecuteNonQuery(string.Format(c, i.ToString("000")));

                        Assert.GreaterOrEqual(
                            rows,
                            0,
                            $"Oracle-MZYY01 数据库设置测试数据失败,查询语句 : \r\n{c}");
                    });
                }
            });

            Assert.IsTrue(
                success,
                $"Oracle-MZYY01 数据库错误 : \r\n{ex?.GetExceptionAllMsg()}");

            Console.WriteLine("已设置测试数据");
        }

        /// <summary>
        /// 默认测试方法
        /// </summary>
        /// <param name="key">标识</param>
        public void DefaultTest(string key)
        {
            Assert.IsTrue(
                Controller.Actions.ContainsKey(key),
                $"Controllers Actions 配置未包含 {key}");

            var action = Controller.Actions[key];

            Assert.NotNull(
                   action,
                   $"Controllers Actions {key} 配置为空.");

            SetupTestData(action.TestDataCount, action.ClearTestDataCmd);

            SetupTestData(action.TestDataCount, action.SetupTestDataCmd);

            (HttpStatusCode statusCode, string responseData) = HttpHelper.RequestData(
                                    action.Method,
                                    $"{Controller.Server}{action.Path.SetParamters2Path(action.Paramters)}",
                                    action.Paramters,
                                    null,
                                    600 * 1000);

            SetupTestData(action.TestDataCount, action.ClearTestDataCmd);

            Assert.AreEqual(
                HttpStatusCode.OK,
                statusCode,
                $"{Controller.Server}{action.Path} 接口请求失败\r\n参数 : {string.Join("\r\n", action.Paramters ?? new Dictionary<string, object>())}\r\n输出 : {responseData}.");

            var response = responseData.ToObject<AjaxResult>();

            Assert.AreEqual(
                false,
                response.Success,
                $"{Controller.Server}{action.Path} 接口返回状态有误\r\n返回信息 : {response.Msg}.");

            Assert.Pass($"{Controller.Server}{action.Path} 接口调用成功\r\n返回信息 : {response.Msg}.");
        }

        [Test(Author = "LCTR", Description = "测试S201")]
        [Order(1)]
        public void S201()
        {
            DefaultTest(nameof(S201));
        }

        [Test(Author = "LCTR", Description = "测试S202")]
        [Order(2)]
        public void S202()
        {
            DefaultTest(nameof(S202));
        }

        [Test(Author = "LCTR", Description = "测试S203")]
        [Order(3)]
        public void S203()
        {
            DefaultTest(nameof(S203));
        }

        [Test(Author = "LCTR", Description = "测试S204")]
        [Order(4)]
        public void S204()
        {
            DefaultTest(nameof(S204));
        }

        [Test(Author = "LCTR", Description = "测试S205")]
        [Order(5)]
        public void S205()
        {
            DefaultTest(nameof(S205));
        }

        [Test(Author = "LCTR", Description = "测试S206")]
        [Order(6)]
        [Ignore("明州医院IT部负责人方树杰表示此功能暂不需要")]
        public void S206()
        {
            DefaultTest(nameof(S206));
        }

        [Test(Author = "LCTR", Description = "测试S207")]
        [Order(7)]
        [Ignore("明州医院IT部负责人方树杰表示此功能暂不需要")]
        public void S207()
        {
            DefaultTest(nameof(S207));
        }

        [Test(Author = "LCTR", Description = "测试S208")]
        [Order(8)]
        public void S208()
        {
            DefaultTest(nameof(S208));
        }
    }
}
