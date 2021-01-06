using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Util;
using Entity.Example;
using Library.Extension;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.OpenApi.Annotations;
using Model;
using Model.Example;
using NUnit.Framework;
using UnitTest.Extension;
using Library.Container;
using Model.System;
using UnitTest.Config;
using Library.OpenApi.Extention;

namespace UnitTest.Testing.Database
{
    /// <summary>
    /// 数据库测试
    /// </summary>
    [TestFixture(Author = "LCTR", TestName = "测试配置的数据库", Description = "测试增删改查等基本功能")]
    [Ignore("忽略测试")]
    public class DatabaseTest
    {
        public DatabaseTest()
        {

        }

        private SystemConfig Config;

        private IFreeSqlMultipleProvider<string> Orms;

        private List<DatabaseSetting> Dbs;

        private DatabaseConfig TestConfig;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            Config = AutofacHelper.GetService<SystemConfig>();

            Assert.IsTrue(
                   Config.EnableFreeSql,
                   "FreeSql未启用.");

            Assert.NotNull(
                   Config.Databases,
                   "数据库配置为空.");

            Assert.NotZero(
                Config.Databases.Count,
                "数据库配置为0.");

            Dbs = Config.Databases.Where(db => db.Enable).ToList();

            Assert.NotZero(
                Dbs.Count,
                "数据库可用配置为0.");

            Orms = AutofacHelper.GetService<IFreeSqlMultipleProvider<string>>();

            Assert.NotNull(
                   Orms,
                   "数据库实例构造器为null.");

            TestConfig = DatabaseConfig.GetData();

            Assert.NotNull(
                TestConfig.Insert,
                "测试数据 Insert 为空.");

            Assert.NotNull(
                TestConfig.Update,
                "测试数据 Update 为空.");

            Assert.AreEqual(
                TestConfig.Insert.Count,
                TestConfig.Update.Count,
                "测试数据 Insert 和 Update 数量不一致.");

            Dbs.ForEach(db =>
            {
                Assert.IsTrue(
                    Orms.Exists(db.Name),
                    $"{db.Name} 数据库未注册.");

                Assert.NotNull(
                    Orms.GetFreeSql(db.Name),
                    $"{db.Name} 数据库实例为空.");
            });

            Console.WriteLine("数据库测试开始.");
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {

            Console.WriteLine("数据库测试结束.");
        }

        [Test(Author = "LCTR", Description = "测试新增实体")]
        [Order(1)]
        public void Insert()
        {
            var datas = TestConfig.Insert;

            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                var insertData = data.InitEntityWithoutOP();
                insertData.CreateTime = DateTime.Parse(insertData.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                TestConfig.Insert[i] = insertData;
            }

            Dbs.ForEach(db =>
            {
                var orm = Orms.GetFreeSql(db.Name);

                for (int i = 0; i < datas.Count; i++)
                {
                    var insertData = datas[i];

                    Assert.AreEqual(
                        1,
                        orm.Insert(insertData).ExecuteAffrows(),
                        $"{db.Name} 数据库写入数据失败.");

                    var findInsertList = orm.Select<Sample_DB>().Where(e => e.Id == insertData.Id);

                    Assert.AreEqual(
                        1,
                        findInsertList.Count(),
                        $"{db.Name} 数据库写入数据错误,查询到了多条[ID : {insertData.Id}]数据.\r\n{string.Join("\r\n", findInsertList.ToList().Select(o => o.ToJson()))}.");

                    var findInsertData = findInsertList.First();

                    findInsertData.ModifyTime = DateTime.MinValue;

                    Assert.NotNull(
                        findInsertData,
                        $"未查询到已写入 {db.Name} 数据库的数据[ID : {insertData.Id}].");

                    insertData.AreEqual(
                        null,
                        findInsertData);

                    Console.WriteLine($"{db.Name} 数据库新增实体功能正常.");
                }

                datas.ForEach(data =>
                {

                });
            });

            Assert.Pass("新增实体功能正常.");
        }

        [Test(Author = "LCTR", Description = "更新实体")]
        [Order(2)]
        public void Update()
        {
            var datas = TestConfig.Insert;

            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];
                var updateData = data.ModifyEntityWithoutOP();
                updateData.Name = TestConfig.Update[i].Name;
                updateData.Content = TestConfig.Update[i].Content;
                updateData.ModifyTime = DateTime.Parse(updateData.ModifyTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            Dbs.ForEach(db =>
            {
                var orm = Orms.GetFreeSql(db.Name);

                for (int i = 0; i < datas.Count; i++)
                {
                    var updateData = datas[i];

                    Assert.AreEqual(
                        1,
                        orm.Update<Sample_DB>().SetSource(updateData).ExecuteAffrows(),
                        $"{db.Name} 数据库更新数据[{updateData.ToJson()}]失败.");

                    var findUpdateData = orm.Select<Sample_DB>().Where(e => e.Id == updateData.Id).First();

                    Assert.NotNull(
                        findUpdateData,
                        $"未查询到 {db.Name} 数据库已修改的数据[ID : {updateData.Id}].");

                    updateData.AreEqual(
                        null,
                        findUpdateData);

                    Console.WriteLine($"{db.Name} 数据库更新实体功能正常.");
                }
            });

            Assert.Pass("更新实体功能正常.");
        }

        [Test(Author = "LCTR", Description = "查询列表")]
        [Order(3)]
        public void ToList()
        {
            var datas = TestConfig.Insert.Select(data => data.Id).ToList();

            Dbs.ForEach(db =>
            {
                var orm = Orms.GetFreeSql(db.Name);

                var findListData = orm.Select<Sample_DB>()
                                        .Where(o => datas.Contains(o.Id))
                                        .ToList<Sample_DB, Model.Example.DBDTO.List>(
                                            orm,
                                            new Library.Models.Pagination { PageRows = 10 },
                                            typeof(Model.Example.DBDTO.List).GetNamesWithTagAndOther(true, "_List"));

                Assert.Greater(
                    findListData.Count,
                    0,
                    $"{db.Name} 数据库未查询到任何数据.");

                Assert.AreEqual(
                    datas.Count,
                    findListData.Count,
                    $"{db.Name} 数据库查询到的数据量和新增的数据量不一致.");

                Console.WriteLine($"{db.Name} 数据库查询列表功能正常.");
            });

            Assert.Pass("查询列表功能正常.");
        }

        [Test(Author = "LCTR", Description = "删除实体")]
        [Order(4)]
        public void Delete()
        {
            var datas = TestConfig.Insert.Select(data => data.Id).ToList();

            Dbs.ForEach(db =>
            {
                var orm = Orms.GetFreeSql(db.Name);

                Assert.AreEqual(
                        datas.Count,
                        orm.Delete<Sample_DB>().Where(e => datas.Contains(e.Id)).ExecuteAffrows(),
                        $"{db} 数据库删除数据失败.");

                var findDeleteData = orm.Select<Sample_DB>().Where(e => datas.Contains(e.Id));

                Assert.AreEqual(
                    0,
                    findDeleteData.Count(),
                    $"{db} 数据库数据未被删除\r\n{string.Join("\r\n", findDeleteData.ToList().Select(o => o.ToJson()))}.");

                Console.WriteLine($"{db.Name} 数据库删除实体功能正常.");
            });

            Assert.Pass("删除实体功能正常.");
        }
    }
}
