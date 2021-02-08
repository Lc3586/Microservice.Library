using Business.Utils;
using Library.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    /// <summary>
    /// 全局错误拦截
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult
            {
                Content = ExceptionHelper.HandleException(context.Exception, context.HttpContext.Request.Path.Value).ToJson(),
                ContentType = "application/json; charset=utf-8",
            };
        }
    }
}
