using Api.Controllers.Utils;
using Business.Interface.Example;
using Library.Extension;
using Microsoft.AspNetCore.Mvc;
using Model.Example.DBDTO;
using Model.Utils.Pagination;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// 示例接口
    /// </summary>
    [Route("/sample")]//路由模板
    [ApiPermission]//接口权限校验
    [SwaggerTag("示例接口，包含列表、增、删、改、查等接口")]//swagger标签
    public class SampleController : BaseApiController//继承接口基本控制器
    {
        #region DI

        /// <summary>
        /// 业务类
        /// </summary>
        ISampleBusiness sampleBusiness;

        /// <summary>
        /// 在构造函数中注入DI系统中注册的依赖
        /// </summary>
        /// <param name="_exampleBusiness"></param>
        public SampleController(ISampleBusiness _exampleBusiness)
        {
            sampleBusiness = _exampleBusiness;
        }

        #endregion

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <remarks>
        /// ## 示例 1     一般查询
        /// 
        /// #查询第一页，每页10条数据，按修改时间倒序排序。
        /// 
        ///     POST /config/list
        ///     {
        ///         "PageIndex": 1,
        ///         "PageRows": 10,
        ///         "SortField": "ModifyTime",
        ///         "SortType": "desc"
        ///     }
        /// 
        /// ## 示例 2     高级排序
        /// 
        /// #查询第一页，每页10条数据，按修改时间倒序排序之后再按创建时间正序排列。
        ///
        ///     POST /config/list
        ///     {
        ///         "PageIndex": 1,
        ///         "PageRows": 10,
        ///         "AdvancedSort": [
        ///             {
        ///                 "field": "ModifyTime",
        ///                 "type": "desc"
        ///             },
        ///             {
        ///                 "field": "CreateTime",
        ///                 "type": "asc"
        ///             }
        ///         ]
        ///     }
        /// 
        /// ## 示例 3     高级搜索 1
        /// 
        /// #查询第一页，每页10条数据，筛选应用名称中包含“应用”，以及创建者为“管理员A”的数据。
        /// 
        ///     POST /config/list
        ///     {
        ///         "PageIndex": 1,
        ///         "PageRows": 10,
        ///         "Filter": [
        ///             {
        ///                 "field": "AppName",
        ///                 "value": "应用",
        ///                 "compare": "in"
        ///             },
        ///             {
        ///                 "field": "CreatorName",
        ///                 "value": "管理员A",
        ///                 "compare": "eq"
        ///             }
        ///         ]
        ///     }
        /// 
        /// ## 示例 4     高级搜索 2
        /// 
        /// #查询第一页，每页10条数据，筛选应用名称中包含“应用”，并且创建者为“管理员A”，又或者创建时间大于“2020-03-10”的数据。
        /// 
        ///     POST /config/list
        ///     {
        ///         "PageIndex": 1,
        ///         "PageRows": 10,
        ///         "Filter": [
        ///             {
        ///                 "group": "start",
        ///                 "field": "AppName",
        ///                 "value": "应用",
        ///                 "compare": "in"
        ///             },
        ///             {
        ///                 "group": "end",
        ///                 "relation": "and"
        ///                 "field": "CreatorName",
        ///                 "value": "管理员A",
        ///                 "compare": "eq"
        ///             },
        ///             {
        ///                 "relation": "or"
        ///                 "field": "CreateTime",
        ///                 "value": "2020-03-10",
        ///                 "compare": "gt"
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        [HttpPost("list")]//请求方法以及模板名称
        [SwaggerResponse((int)HttpStatusCode.OK, "列表数据", typeof(List))]//指定输出架构
        public async Task<object> GetList([FromBody]/*指定参数来自请求正文*/PaginationDTO pagination)
        {
            return JsonContent(await Task.FromResult(sampleBusiness.GetList(pagination)), pagination);
        }

        /// <summary>
        /// 详情数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost("detail-data/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(Detail))]
        public async Task<object> GetDetail(string id)
        {
            return JsonContent(await Task.FromResult(sampleBusiness.GetDetail(id)));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data">表单数据</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<object> Create([FromBody]/*指定参数来自请求正文*/Create data)
        {
            sampleBusiness.Create(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost("edit-data/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "编辑数据", typeof(Edit))]
        public async Task<object> GetEdit(string id)
        {
            return JsonContent(await Task.FromResult(sampleBusiness.GetEdit(id)));
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="data">表单数据</param>
        /// <returns></returns>
        [HttpPost("edit")]
        [CheckModel]//表单模型校验
        public async Task<object> Edit([FromBody] Edit data)
        {
            sampleBusiness.Edit(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<object> Delete(IEnumerable<string> ids)
        {
            sampleBusiness.Delete(ids?.ToList());
            return await Task.FromResult(Success());
        }
    }
}
