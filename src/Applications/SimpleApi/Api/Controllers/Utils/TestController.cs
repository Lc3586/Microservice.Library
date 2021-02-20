﻿using AutoMapper;
using IocServiceDemo;
using Library.DataMapping.Gen;
using Library.Extension;
using Library.FreeSql.Gen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Api.Controllers.Utils
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("/[controller]/[action]")]
    [CheckModel]
    [SwaggerTag("测试接口")]
    public class TestController : BaseApiController
    {
        #region DI

        public TestController(
            IDemoService demoService,
            IDemoServiceProvider demoServiceProvider,
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider)
        {
            if (demoService == null)
                DemoService = demoServiceProvider.GetService();
            else
                DemoService = demoService;

            Orm = freeSqlProvider.GetFreeSql();

            Mapper = autoMapperProvider.GetMapper();
        }

        #endregion

        readonly IDemoService DemoService;

        readonly IMapper Mapper;

        readonly IFreeSql Orm;

        /// <summary>
        /// 测试依赖注入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> DependencyInjectionTest(string value)
        {
            var result = DemoService.Change(value);

            DemoService.Quit();

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 测试数据库指定的表
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> TestDataBaseWithTable(string table, string where = null)
        {
            var dt = Orm.Ado.ExecuteDataTable($"select * from {table} where {(where.IsNullOrEmpty() ? "1=1" : where)}");

            return await Task.FromResult(dt);
        }

        /// <summary>
        /// 测试数据库指定的存储过程
        /// </summary>
        /// <param name="procedure">存储过程</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> TestDataBaseWithStoredProcedure(string procedure)
        {
            var ds = Orm.Ado.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, procedure);

            return await Task.FromResult(ds);
        }
    }
}