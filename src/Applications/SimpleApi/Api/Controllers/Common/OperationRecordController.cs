using Api.Controllers.Utils;
using Business.Interface.Common;
using Microsoft.AspNetCore.Mvc;
using Model.Common.OperationRecordDTO;
using Model.Utils.Pagination;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// 操作记录接口
    /// </summary>
    [Route("/operationrecord")]
    [ApiPermission]
    [CheckModel]
    [SwaggerTag("操作记录接口")]
    public class OperationRecordController : BaseApiController
    {
        #region DI

        public OperationRecordController(IOperationRecordBusiness operationRecordBusiness)
        {
            OperationRecordBusiness = operationRecordBusiness;
        }

        readonly IOperationRecordBusiness OperationRecordBusiness;

        #endregion

        #region 基础接口

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        [HttpPost("list")]
        [SwaggerResponse((int)HttpStatusCode.OK, "列表数据", typeof(List))]
        public async Task<object> GetList([FromBody] PaginationDTO pagination)
        {
            return JsonContent(await Task.FromResult(OperationRecordBusiness.GetList(pagination)), pagination);
        }

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost("detail-data/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(Detail))]
        public async Task<object> GetDetail(string id)
        {
            return JsonContent(await Task.FromResult(OperationRecordBusiness.GetDetail(id)));
        }

        #endregion

        #region 拓展接口



        #endregion        
    }
}
