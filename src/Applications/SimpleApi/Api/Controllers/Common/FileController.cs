using Api.Controllers.Utils;
using Business.Interface.Common;
using Microservice.Library.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Common.FileDTO;
using Model.Utils.Pagination;
using Model.Utils.Result;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// 文件处理接口
    /// </summary>
    [Route("/file")]
    [Authorize]
    [ApiPermission]
    [CheckModel]
    [SwaggerTag("文件处理接口")]
    [Consumes("multipart/form-data")]
    public class FileController : BaseController
    {
        #region DI

        public FileController(IFileBusiness fileBusiness)
        {
            FileBusiness = fileBusiness;
        }

        readonly IFileBusiness FileBusiness;

        #endregion

        #region 数据接口

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        [HttpPost("list")]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.OK, "列表数据", typeof(FileInfo))]
        public async Task<object> GetList([FromBody] PaginationDTO pagination)
        {
            return await Task.FromResult(OpenApiJsonContent(FileBusiness.GetList(pagination), pagination));
        }

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost("detail-data/{id}")]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(FileInfo))]
        public async Task<object> GetDetail(string id)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(FileBusiness.GetDetail(id))));
        }

        /// <summary>
        /// 获取详情数据集合
        /// </summary>
        /// <param name="ids">id逗号拼接</param>
        /// <returns></returns>
        [HttpGet("detail-list/{ids}")]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(FileInfo))]
        public async Task<object> GetDetails(string ids)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(FileBusiness.GetDetails(ids))));
        }

        /// <summary>
        /// 获取详情数据集合
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        [HttpPost("detail-list")]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
        [Produces("application/json")]
        [SwaggerResponse((int)HttpStatusCode.OK, "详情数据", typeof(FileInfo))]
        public async Task<object> GetDetails(List<string> ids)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(FileBusiness.GetDetails(ids))));
        }

        #endregion

        #region 文件操作接口

        /// <summary>
        /// MD5校验
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        [HttpPost("check-md5")]
        [SwaggerResponse((int)HttpStatusCode.OK, "校验结果", typeof(CheckMD5Response))]
        public async Task<object> CheckMD5(string md5)
        {
            return await Task.FromResult(AjaxResultFactory.Success(OpenApiJsonContent(FileBusiness.CheckMD5(md5))));
        }

        /// <summary>
        /// 单图上传
        /// </summary>
        /// <param name="option">选项</param>
        /// <returns></returns>
        [HttpPost("upload-single-image")]
        [SwaggerResponse((int)HttpStatusCode.OK, "文件信息", typeof(FileInfo))]
        public async Task<object> UploadSingleImage(ImageUploadParams option)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(FileBusiness.SingleImage(option))));
        }

        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="option">选项</param>
        /// <returns></returns>
        [HttpPost("upload-single-file")]
        [SwaggerResponse((int)HttpStatusCode.OK, "文件信息", typeof(FileInfo))]
        public async Task<object> UploadSingleFile(FileUploadParams option)
        {
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(FileBusiness.SingleFile(option))));
        }

        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        [HttpGet("preview/{id}")]
        public void Preview(string id)
        {
            FileBusiness.Preview(id);
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        [HttpGet("browse/{id}")]
        public void Browse(string id)
        {
            FileBusiness.Browse(id);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        [HttpGet("download/{id}")]
        public void Download(string id)
        {
            FileBusiness.Download(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        [HttpPost("delete")]
        [Consumes("application/json", "application/x-www-form-urlencoded")]
        [Produces("application/json")]
        public async Task<object> Delete(IEnumerable<string> ids)
        {
            FileBusiness.Delete(ids?.ToList());
            return await Task.FromResult(Success());
        }

        #endregion        
    }
}
