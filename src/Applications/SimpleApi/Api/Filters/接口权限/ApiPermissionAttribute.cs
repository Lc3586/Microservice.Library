using Business.Interface.System;
using Library.Container;
using Library.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.System;
using Model.System.Config;

namespace Api
{
    /// <summary>
    /// 接口权限校验
    /// </summary>
    public class ApiPermissionAttribute : BaseActionFilter, IActionFilter
    {
        SystemConfig Config => AutofacHelper.GetScopeService<SystemConfig>();

        IAuthoritiesBusiness AuthoritiesBusiness => AutofacHelper.GetScopeService<IAuthoritiesBusiness>();

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
            switch (Operator.AuthenticationInfo.UserType)
            {
                case UserType.系统用户:
                    if (!AuthoritiesBusiness.UserHasMenuUri(Operator.AuthenticationInfo.Id, context.HttpContext.Request.Path.Value?.ToLower()))
                        context.Result = Error("没有权限!", ErrorCode.forbidden);
                    break;
                case UserType.会员:
                    if (!AuthoritiesBusiness.MemberHasMenuUri(Operator.AuthenticationInfo.Id, context.HttpContext.Request.Path.Value?.ToLower()))
                        context.Result = Error("没有权限!", ErrorCode.forbidden);
                    break;
                default:
                    context.Result = Error("用户类型错误!", ErrorCode.forbidden);
                    break;
            }
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