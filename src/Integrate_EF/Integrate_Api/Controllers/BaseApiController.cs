using Integrate_Business;
using Library.Container;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace Integrate_Api
{
    /// <summary>
    /// Mvc对外接口基控制器
    /// </summary>
    [Authorize]//登录校验
    [Consumes("application/json", "application/x-www-form-urlencoded")]//接收数据类型
    [Produces("application/json", "text/plain", "text/json", "application/octet-stream")]//生产数据类型
    [SwaggerResponse((int)HttpStatusCode.OK, "请求结果", typeof(AjaxResult))]//指定类下所有接口的输出架构，优先使用方法上指定的输出架构
    public class BaseApiController : BaseController
    {
        public IOperator Operator { get => AutofacHelper.GetScopeService<IOperator>(); }

        #region 业务通用模型

        /// <summary>
        /// 接口删除模型
        /// </summary>
        public class DeleteModel
        {
            /// <summary>
            /// id集合
            /// </summary>
            [Required(ErrorMessage = "未指定任何数据")]
            public string[] ids { get; set; }
        }

        #endregion
    }
}