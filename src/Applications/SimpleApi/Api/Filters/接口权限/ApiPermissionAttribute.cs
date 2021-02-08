using Microsoft.AspNetCore.Mvc.Filters;
using Library.Extension;
using System;
using Library.Http;
using Library.Container;
using Library.Models;
using Model.System;
using Model.System.Config;

namespace Api
{
    /// <summary>
    /// 接口权限校验
    /// </summary>
    public class ApiPermissionAttribute : BaseActionFilter, IActionFilter
    {
        readonly SystemConfig Config = AutofacHelper.GetScopeService<SystemConfig>();

        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ContainsFilter<NoApiPermissionAttribute>())
                return;
            if (Config.RunMode == RunMode.LocalTest)
                return;
            if (Operator.IsAdmin)
                return;

            //验证权限
            //if (Operator.Property?.ServiceAuthorities.Any_Ex(o =>
            //       context.HttpContext.Request.Path.Value?.ToLower().IndexOf(o.Url) == 0) != true)
            //    context.Result = Error("没有权限!", ErrorCode.forbidden);
        }

        /// <summary>
        /// Action执行完毕之后执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}