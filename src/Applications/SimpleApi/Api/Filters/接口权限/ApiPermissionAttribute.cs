using Business.Interface.System;
using Microservice.Library.Container;
using Microservice.Library.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.System;
using Model.Utils.Config;
using Model.Utils.Result;
using System.Linq;

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
            if (context.ActionDescriptor.EndpointMetadata.Any(o =>
                    o.GetType() == typeof(NoApiPermissionAttribute)
                    || o.GetType() == typeof(AllowAnonymousAttribute)))
                return;

            if (Config.RunMode == RunMode.LocalTest)
                return;

            if (!Operator.IsAuthenticated)
            {
                context.Result = Error("未登录!", ErrorCode.forbidden);
                return;
            }

            if (Operator.IsSuperAdmin)
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