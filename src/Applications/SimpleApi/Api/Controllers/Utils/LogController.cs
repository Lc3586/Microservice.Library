using Business.Utils.Log;
using Microsoft.AspNetCore.Mvc;
using Model.System.LogDTO;
using Model.Utils.Log.LogDTO;
using Model.Utils.Pagination;
using Model.Utils.Result;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers.Utils
{
    /// <summary>
    /// 日志接口
    /// </summary>
    [Route("/log")]
    [CheckModel]
    [SwaggerTag("日志接口")]
    public class LogController : BaseApiController
    {
        #region DI

        public LogController(
            ILogBusiness logBusiness)
        {
            LogBusiness = logBusiness;
        }

        #endregion

        readonly ILogBusiness LogBusiness;

        /// <summary>
        /// 获取默认的日志组件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("default-type")]
        public async Task<object> GetDefaultType()
        {
            return await Task.FromResult(Success<string>(LogBusiness.GetDefaultType()));
        }

        /// <summary>
        /// 获取日志文件列表
        /// </summary>
        /// <param name="start">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <returns></returns>
        [HttpGet("file-list/{start}/{end}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "文件信息", typeof(FileInfo))]
        public async Task<object> GetFileList(DateTime start, DateTime end)
        {

            return await Task.FromResult(Success(LogBusiness.GetFileList(start, end)));
        }

        /// <summary>
        /// 获取日志文件内容
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        [HttpGet("file-content/{filename}")]
        public async Task GetFileContent(string filename)
        {
            await LogBusiness.GetFileContent(filename);
        }

        /// <summary>
        /// 获取ES数据列表
        /// </summary>
        /// <param name="pagination">排序、筛选以及数据量设置</param>
        /// <returns></returns>
        [HttpPost("es-list")]
        [SwaggerResponse((int)HttpStatusCode.OK, "列表数据", typeof(List))]
        public async Task<object> GetESList([FromBody] PaginationDTO pagination)
        {
            return await Task.FromResult(OpenApiJsonContent(LogBusiness.GetESList(pagination), pagination));
        }

        /// <summary>
        /// 获取ES数据详情
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet("es-detail-data/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(Detail))]
        public async Task<object> GetESDetail(string id)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(LogBusiness.GetESDetail(id))));
        }

        /// <summary>
        /// 获取数据库数据列表
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        [HttpPost("db-list")]
        [SwaggerResponse((int)HttpStatusCode.OK, "列表数据", typeof(List))]
        public async Task<object> GetDBList([FromBody] PaginationDTO pagination)
        {
            return await Task.FromResult(OpenApiJsonContent(LogBusiness.GetDBList(pagination), pagination));
        }

        /// <summary>
        /// 获取数据库数据详情
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet("db-detail-data/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(Detail))]
        public async Task<object> GetDBDetail(string id)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(LogBusiness.GetDBDetail(id))));
        }
    }
}