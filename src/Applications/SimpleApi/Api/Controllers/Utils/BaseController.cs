using Library.Extension;
using Library.OpenApi.Extention;
using Microsoft.AspNetCore.Mvc;
using Model.Utils.Pagination;
using Model.Utils.Result;
using System.Collections.Generic;
using System.Text;

namespace Api.Controllers.Utils
{
    /// <summary>
    /// 基控制器
    /// </summary>
    [JsonParamter(true)]//Json参数转模型
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <typeparam name="TOpenApiSchema">接口架构类型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ContentResult OpenApiJsonContent<TOpenApiSchema>(TOpenApiSchema obj)
        {
            return base.Content(obj.ToOpenApiJson(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <typeparam name="TOpenApiSchema">接口架构类型</typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ContentResult OpenApiJsonContent<TOpenApiSchema>(object result)
        {
            return base.Content(result.ToOpenApiJson<TOpenApiSchema>(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected ContentResult JsonContent(object result)
        {
            return base.Content(result.ToJson(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <typeparam name="TOpenApiSchema">接口架构类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="pagination"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        protected ContentResult OpenApiJsonContent<TOpenApiSchema>(List<TOpenApiSchema> obj, PaginationDTO pagination, bool success = true, string error = null)
        {
            var type = pagination.GetSchemaResultType<TOpenApiSchema>();
            return base.Content(pagination.BuildResult(obj, success, error).ToOpenApiJson(type), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <typeparam name="TOpenApiSchema">接口架构类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="pagination"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        protected ContentResult OpenApiJsonContent<TOpenApiSchema>(List<object> obj, PaginationDTO pagination, bool success = true, string error = null)
        {
            var type = pagination.GetSchemaResultType<TOpenApiSchema>();
            return base.Content(pagination.BuildResult(obj, success, error).ToOpenApiJson(type), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pagination"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        protected ContentResult JsonContent(List<object> obj, PaginationDTO pagination, bool success = true, string error = null)
        {
            return base.Content(pagination.BuildResult(obj, success, error).ToJson(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        protected ContentResult JsonContent(string jsonStr)
        {
            return base.Content(jsonStr, "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回html
        /// </summary>
        /// <param name="body">html内容</param>
        /// <returns></returns>
        protected ContentResult HtmlContent(string body)
        {
            return base.Content(body);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        protected ContentResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！",
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        protected ContentResult Success(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg,
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        protected ContentResult Success<T>(T data)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = "请求成功！",
                Data = data
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <param name="msg">返回的消息</param>
        /// <returns></returns>
        protected ContentResult Success<T>(T data, string msg)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = msg,
                Data = data
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="TOpenApiSchema">接口架构类型</typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected ContentResult SuccessOpenApiSchema<TOpenApiSchema>(TOpenApiSchema data, string msg = "请求成功")
        {
            return base.Content(new AjaxResult<object>
            {
                Success = true,
                Msg = msg,
                Data = data.ToOpenApiJson().ToObject<object>()
            }.ToJson(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        protected ContentResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "请求失败！",
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        protected ContentResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <typeparam name="TOpenApiSchema">接口架构类型</typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected ContentResult ErrorOpenApiSchema<TOpenApiSchema>(TOpenApiSchema data, string msg = "请求失败")
        {
            return base.Content(new AjaxResult<object>
            {
                Success = false,
                Msg = msg,
                Data = data.ToOpenApiJson().ToObject<object>()
            }.ToJson(), "application/json", Encoding.UTF8);
        }

        /// <summary>
        /// 返回表格数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        protected ContentResult DataTable<T>(List<T> list)
        {
            return DataTable(list, new PaginationDTO());
        }

        /// <summary>
        /// 返回表格数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">数据列表</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        protected ContentResult DataTable<T>(List<T> list, PaginationDTO pagination)
        {
            return JsonContent(pagination.BuildResult(list).ToJson());
        }
    }
}