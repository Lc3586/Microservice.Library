using Business.Interface.System;
using Microservice.Library.Container;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Utils.Config;
using Model.Utils.Result;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Api.Controllers.Utils
{
    /// <summary>
    /// 对外接口基控制器
    /// </summary>
    [Authorize]//登录校验
    [Consumes("application/json", "application/x-www-form-urlencoded")]//接收数据类型
    [Produces("application/json", "text/plain", "text/json", "application/octet-stream")]//生产数据类型
    [SwaggerResponse((int)HttpStatusCode.OK, "请求结果", typeof(AjaxResult))]//指定类下所有接口的输出架构，优先使用方法上指定的输出架构
    public class BaseApiController : BaseController
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        protected SystemConfig Config => AutofacHelper.GetService<SystemConfig>();

        /// <summary>
        /// 当前登录人
        /// </summary>
        protected IOperator Operator => AutofacHelper.GetScopeService<IOperator>();
    }
}