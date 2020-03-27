using Library.Models;
using System;
using System.Collections.Generic;
using Library.Extention;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Integrate_Business.Util
{
    /// <summary>
    /// 通过反射调用方法
    /// LCTR 2019-06-19
    /// </summary>
    public static class ReflectionHelper
    {

        #region 公开方法

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static AjaxResult Invoke<Class>(Class obj, string Method, object[] parameters)
        {
            return Invoke_Real(obj, Method, parameters, null, null, false);
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <returns></returns>
        public static AjaxResult Invoke<Class>(Class obj, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors)
        {
            return Invoke_Real(obj, Method, parameters, modelStateErrors, null, false);
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Invoke<Class>(Class obj, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            return Invoke_Real(obj, Method, parameters, modelStateErrors, request, false);
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Invoke<Class>(Class obj, string Method, object[] parameters, HttpRequest request)
        {
            return Invoke_Real(obj, Method, parameters, null, request, false);
        }

        /// <summary>
        /// 调用异步方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static AjaxResult InvokeAsync<Class>(Class obj, string Method, object[] parameters)
        {
            return Invoke_Real(obj, Method, parameters, null, null, true);
        }

        /// <summary>
        /// 调用异步方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <returns></returns>
        public static AjaxResult InvokeAsync<Class>(Class obj, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors)
        {
            return Invoke_Real(obj, Method, parameters, modelStateErrors, null, true);
        }

        /// <summary>
        /// 调用异步方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult InvokeAsync<Class>(Class obj, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            return Invoke_Real(obj, Method, parameters, modelStateErrors, request, true);
        }

        /// <summary>
        /// 调用异步方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult InvokeAsync<Class>(Class obj, string Method, object[] parameters, HttpRequest request)
        {
            return Invoke_Real(obj, Method, parameters, null, request, true);
        }

        /// <summary>
        /// 调用静态方法
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static AjaxResult InvokeStatic(Type type, string Method, object[] parameters)
        {
            return InvokeStatic_Real(type, Method, parameters, null, null);
        }

        /// <summary>
        /// 调用静态方法
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <returns></returns>
        public static AjaxResult InvokeStatic(Type type, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors)
        {
            return InvokeStatic_Real(type, Method, parameters, modelStateErrors, null);
        }

        /// <summary>
        /// 调用静态方法
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult InvokeStatic(Type type, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            return InvokeStatic_Real(type, Method, parameters, modelStateErrors, request);
        }

        /// <summary>
        /// 调用静态方法
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult InvokeStatic(Type type, string Method, object[] parameters, HttpRequest request)
        {
            return InvokeStatic_Real(type, Method, parameters, null, request);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <typeparam name="Class">目标类型</typeparam>
        /// <param name="obj">目标</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <param name="async">异步方法</param>
        /// <returns></returns>
        private static AjaxResult Invoke_Real<Class>(Class obj, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors, HttpRequest request, bool async)
        {
            Type type = obj.GetType();
            try
            {
                if (modelStateErrors.Any_Ex(o => o.Errors.Count > 0))
                    throw new ValidationException("数据验证失败", modelStateErrors);
                var method = type.GetMethod(Method);
                bool hasResult = method.ReturnType.FullName != "System.Void";
                if (async)
                {
                    var task = method.Invoke(obj, parameters) as Task;
                    task.Wait();
                    if (hasResult)
                        return AjaxResultFactory.Success(task.GetType().GetProperty("Result").GetValue(task, null));
                }
                else
                {
                    if (hasResult)
                        return AjaxResultFactory.Success(method.Invoke(obj, parameters));
                }
                return AjaxResultFactory.Success();
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, type.FullName, Method);
            }
        }

        /// <summary>
        /// 调用静态方法
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="Method">方法</param>
        /// <param name="parameters">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        private static AjaxResult InvokeStatic_Real(Type type, string Method, object[] parameters, List<ModelErrorsInfo> modelStateErrors = null, HttpRequest request = null)
        {
            try
            {
                if (modelStateErrors.Any_Ex(o => o.Errors.Count > 0))
                    throw new ValidationException("数据验证失败", modelStateErrors);
                var method = type.GetMethod(Method);
                if (method.ReturnType.FullName != "System.Void")
                    return AjaxResultFactory.Success(method.Invoke(null, parameters));
                else
                    return AjaxResultFactory.Success();
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, type.FullName, Method);
            }
        }

        #endregion
    }
}
