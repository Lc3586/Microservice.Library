using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Library.Extention;
using Integrate_Business.Util;

namespace Integrate_Api
{
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
