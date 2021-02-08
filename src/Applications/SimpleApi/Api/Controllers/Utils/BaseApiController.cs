using Business.Interface.System;
using Library.Container;
using Library.NLogger.Gen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.System;
using Model.System.Config;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Api.Controllers.Utils
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
        #region DI

        public BaseApiController()
        {
            Config = AutofacHelper.GetScopeService<SystemConfig>();
            Operator = AutofacHelper.GetScopeService<IOperator>();
        }

        /// <summary>
        /// 系统日志
        /// </summary>
        protected readonly SystemConfig Config;

        /// <summary>
        /// 当前登录人
        /// </summary>
        protected readonly IOperator Operator;

        #endregion
    }
}