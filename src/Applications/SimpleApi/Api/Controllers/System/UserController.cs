using Api.Controllers.Utils;
using Business.Interface.System;
using Library.Extension;
using Library.SelectOption;
using Microsoft.AspNetCore.Mvc;
using Model.System.UserDTO;
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
    /// 系统用户接口
    /// </summary>
    [Route("/user")]
    [ApiPermission]
    [CheckModel]
    [SwaggerTag("系统用户接口")]
    public class UserController : BaseApiController
    {
        #region DI

        public UserController(IUserBusiness userBusiness)
        {
            UserBusiness = userBusiness;
        }

        readonly IUserBusiness UserBusiness;

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
            return await Task.FromResult(OpenApiJsonContent(UserBusiness.GetList(pagination), pagination));
        }

        /// <summary>
        /// 获取下拉列表数据
        /// </summary>
        /// <param name="condition">关键词(多个用空格分隔)</param>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        [HttpPost("dropdown-list")]
        [SwaggerResponse((int)HttpStatusCode.OK, "下拉列表选项数据", typeof(SelectOption))]
        public async Task<object> DropdownList(string condition, PaginationDTO pagination)
        {
            return await Task.FromResult(OpenApiJsonContent(UserBusiness.DropdownList(condition, pagination), pagination));
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
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(new List<Detail> { UserBusiness.GetDetail(id) })));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<object> Create([FromBody] Create data)
        {
            UserBusiness.Create(data);
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
            return await Task.FromResult(OpenApiJsonContent(AjaxResultFactory.Success(UserBusiness.GetEdit(id))));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("edit")]
        public async Task<object> Edit([FromBody] Edit data)
        {
            UserBusiness.Edit(data);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<object> Delete(IEnumerable<string> ids)
        {
            UserBusiness.Delete(ids?.ToList());
            return await Task.FromResult(Success());
        }

        #endregion

        #region 拓展接口

        /// <summary>
        /// 启用/禁用
        /// </summary>
        /// <param name="id">数据</param>
        /// <param name="enable">设置状态</param>
        /// <returns></returns>
        [HttpPost("enable/{id}/{enable}")]
        public async Task<object> Enable(string id, bool enable)
        {
            UserBusiness.Enable(id, enable);
            return await Task.FromResult(Success());
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        [HttpPost("update-password")]
        public async Task<object> UpdatePassword(UpdatePassword data)
        {
            UserBusiness.UpdatePassword(data);
            return await Task.FromResult(Success());
        }

        #endregion        
    }
}
