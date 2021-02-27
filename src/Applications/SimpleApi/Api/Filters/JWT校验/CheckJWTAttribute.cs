﻿using Microservice.Library.Container;
using Microservice.Library.Http;
using Microservice.Library.Http.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Model.Utils.Result;
using Model.Utils.Config;
using System;

namespace Api
{
    /// <summary>
    /// JWT校验
    /// </summary>
    public class CheckJWTAttribute : BaseActionFilter, IActionFilter
    {
        readonly SystemConfig Config = AutofacHelper.GetScopeService<SystemConfig>();

        static readonly ErrorCode _errorCode = ErrorCode.validation;

        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.ContainsFilter<NoCheckJWTAttribute>() || Config.RunMode == RunMode.LocalTest)
                    return;

                var req = context.HttpContext.Request;

                string token = req.GetToken();
                if (!JWTHelper.CheckToken(token, Config.JWTSecret))
                {
                    context.Result = Error("token校验失败!", _errorCode);
                    return;
                }

                var payload = JWTHelper.GetPayload<JWTPayload>(token);
                if (payload.Expire < DateTime.Now)
                {
                    context.Result = Error("token过期!", _errorCode);
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Result = Error(ex.Message, _errorCode);
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