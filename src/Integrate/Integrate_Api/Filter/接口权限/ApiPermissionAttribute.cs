using Microsoft.AspNetCore.Mvc.Filters;
using Library.Extension;
using System;
using Library.Http;
using Integrate_Business.Base_Manage;
using Library.Container;
using Integrate_Business.Config;

namespace Integrate_Api
{
    /// <summary>
    /// 接口权限校验
    /// </summary>
    public class ApiPermissionAttribute : BaseActionFilter, IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">权限编号，为空时使用url判断</param>
        public ApiPermissionAttribute(string code = null)
        {
            _code = code;
        }

        public string _code { get; }

        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ContainsFilter<NoApiPermissionAttribute>())
                return;

            //IPermissionBusiness permissionBus = AutofacHelper.GetScopeService<IPermissionBusiness>();
            //var permissions = permissionBus.GetUserPermissionValues(Operator.UserId);
            //if (!permissions.Contains(_code))
            //    context.Result = Error("权限不足!");

            if (context.ContainsFilter<NoApiPermissionAttribute>())
                return;
            if (SystemConfig.systemConfig.RunMode == Library.Models.RunMode.LocalTest)
                return;
            if (Operator.IsAdmin())
                return;
            if (Operator.Property?.MenuAuthorities.Any_Ex(o => _code == null ?
                   o.Url.Contains(context.HttpContext.Request.Path.Value) :
                   o.Code == _code) != true)
                context.Result = Error("权限不足!");
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