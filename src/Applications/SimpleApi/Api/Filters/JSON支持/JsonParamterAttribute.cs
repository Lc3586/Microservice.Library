using Library.Extension;
using Library.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Api
{
    /// <summary>
    /// Json参数支持
    /// </summary>
    public class JsonParamterAttribute : BaseActionFilter, IActionFilter
    {
        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ContainsFilter<NoJsonParamterAttribute>())
                return;

            //参数映射：支持application/json
            string contentType = context.HttpContext.Request.ContentType;
            if (!contentType.IsNullOrEmpty() && contentType.Contains("application/json"))
            {
                var actionParameters = context.ActionDescriptor.Parameters;
                var allParamters = HttpHelper.GetAllRequestParams(context.HttpContext);
                var actionArguments = context.ActionArguments;
                actionParameters.ForEach(aParamter =>
                {
                    string key = aParamter.Name;
                    if (allParamters.ContainsKey(key))
                    {
                        if (allParamters[key].GetType() == typeof(JArray))
                            actionArguments[key] = allParamters[key]?.ToJson().ToObject(aParamter.ParameterType);
                        else
                            actionArguments[key] = allParamters[key]?.ToString()?.ChangeType_ByConvert(aParamter.ParameterType);
                    }
                    else
                    {
                        try
                        {
                            actionArguments[key] = allParamters.ToJson().ToObject(aParamter.ParameterType);
                        }
                        catch
                        {

                        }
                    }
                });
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