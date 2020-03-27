using Library.Models;
using System;
using System.Collections.Generic;
using Library.Extention;
using Microsoft.AspNetCore.Http;

namespace Integrate_Business.Util
{
    /// <summary>
    /// 桥接器
    /// <!--修继元-->
    /// </summary>
    public static class BridgeHelper
    {
        #region 公开方法

        /// <summary>
        /// 无参无返回桥接
        /// </summary>
        /// <param name="bridgeMethod">无参无返回代理</param>
        /// <returns></returns>
        public static AjaxResult Bridge(Action bridgeMethod)
        {
            return Bridge_Real(bridgeMethod, null);
        }

        /// <summary>
        /// 无参无返回桥接
        /// </summary>
        /// <param name="bridgeMethod">无参无返回代理</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge(Action bridgeMethod, HttpRequest request)
        {
            return Bridge_Real(bridgeMethod, request);
        }

        /// <summary>
        /// 无参有返回桥接
        /// </summary>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">无参有返回代理</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Return>(Func<Return> bridgeMethod)
        {
            return Bridge_Real(bridgeMethod, null);
        }

        /// <summary>
        /// 无参有返回桥接
        /// </summary>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">无参有返回代理</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Return>(Func<Return> bridgeMethod, HttpRequest request)
        {
            return Bridge_Real(bridgeMethod, request);
        }

        /// <summary>
        /// 有参无返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <param name="bridgeMethod">有参无返回代理</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter>(Action<Parameter> bridgeMethod, Parameter parameter)
        {
            return Bridge_Real(bridgeMethod, parameter, null, null);
        }

        /// <summary>
        /// 有参无返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <param name="bridgeMethod">有参无返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter>(Action<Parameter> bridgeMethod, Parameter parameter, HttpRequest request)
        {
            return Bridge_Real(bridgeMethod, parameter, null, request);
        }

        /// <summary>
        /// 有参无返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <param name="bridgeMethod">有参无返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter>(Action<Parameter> bridgeMethod, Parameter parameter, List<ModelErrorsInfo> modelStateErrors)
        {
            return Bridge_Real(bridgeMethod, parameter, modelStateErrors, null);
        }

        /// <summary>
        /// 有参无返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <param name="bridgeMethod">有参无返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter>(Action<Parameter> bridgeMethod, Parameter parameter, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            return Bridge_Real(bridgeMethod, parameter, modelStateErrors, request);
        }

        /// <summary>
        /// 有参有返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter, Return>(Func<Parameter, Return> bridgeMethod, Parameter parameter)
        {
            return Bridge_Real(bridgeMethod, parameter, null, null);
        }

        /// <summary>
        /// 有参有返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter, Return>(Func<Parameter, Return> bridgeMethod, Parameter parameter, HttpRequest request)
        {
            return Bridge_Real(bridgeMethod, parameter, null, request);
        }

        /// <summary>
        /// 有参有返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter, Return>(Func<Parameter, Return> bridgeMethod, Parameter parameter, List<ModelErrorsInfo> modelStateErrors)
        {
            return Bridge_Real(bridgeMethod, parameter, modelStateErrors, null);
        }

        /// <summary>
        /// 有参有返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static AjaxResult Bridge<Parameter, Return>(Func<Parameter, Return> bridgeMethod, Parameter parameter, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            return Bridge_Real(bridgeMethod, parameter, modelStateErrors, request);
        }

        /// <summary>
        /// 有参有返回桥接（数据列表专用）
        /// </summary>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="pagination">参数</param>
        /// <returns></returns>
        public static object Bridge_List(Func<Pagination, object> bridgeMethod, Pagination pagination)
        {
            return Bridge_List_Real(bridgeMethod, pagination, null, null);
        }

        /// <summary>
        /// 有参有返回桥接（数据列表专用）
        /// </summary>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="pagination">参数</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static object Bridge_List(Func<Pagination, object> bridgeMethod, Pagination pagination, HttpRequest request)
        {
            return Bridge_List_Real(bridgeMethod, pagination, null, request);
        }

        /// <summary>
        /// 有参有返回桥接（数据列表专用）
        /// </summary>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="pagination">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static object Bridge_List(Func<Pagination, object> bridgeMethod, Pagination pagination, List<ModelErrorsInfo> modelStateErrors)
        {
            return Bridge_List_Real(bridgeMethod, pagination, modelStateErrors, null);
        }

        /// <summary>
        /// 有参有返回桥接（数据列表专用）
        /// </summary>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="pagination">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        public static object Bridge_List(Func<Pagination, object> bridgeMethod, Pagination pagination, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            return Bridge_List_Real(bridgeMethod, pagination, modelStateErrors, request);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 无参无返回桥接
        /// </summary>
        /// <param name="bridgeMethod">无参无返回代理</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        private static AjaxResult Bridge_Real(Action bridgeMethod, HttpRequest request)
        {
            try
            {
                bridgeMethod();
                return AjaxResultFactory.Success();
            }
            catch (System.Exception e)
            {
                return ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, bridgeMethod.Target.GetType().FullName, bridgeMethod.Method.Name);
            }
        }

        /// <summary>
        /// 无参有返回桥接
        /// </summary>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">无参有返回代理</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        private static AjaxResult Bridge_Real<Return>(Func<Return> bridgeMethod, HttpRequest request)
        {
            try
            {
                return AjaxResultFactory.Success(bridgeMethod());
            }
            catch (System.Exception e)
            {
                return ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, bridgeMethod.Target.GetType().FullName, bridgeMethod.Method.Name);
            }
        }

        /// <summary>
        /// 有参无返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <param name="bridgeMethod">有参无返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        private static AjaxResult Bridge_Real<Parameter>(Action<Parameter> bridgeMethod, Parameter parameter, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            try
            {
                if (parameter == null)
                    throw new ValidationException("参数不能为空");

                if (modelStateErrors.Any_Ex())
                    throw new ValidationException("数据验证失败", modelStateErrors);

                bridgeMethod(parameter);

                return AjaxResultFactory.Success();
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, bridgeMethod.Target.GetType().FullName, bridgeMethod.Method.Name);
            }
        }

        /// <summary>
        /// 有参有返回桥接
        /// </summary>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <typeparam name="Return">返回数据类型</typeparam>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="parameter">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        private static AjaxResult Bridge_Real<Parameter, Return>(Func<Parameter, Return> bridgeMethod, Parameter parameter, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            try
            {
                if (parameter == null)
                    throw new ValidationException("参数不能为空");

                if (modelStateErrors.Any_Ex())
                    throw new ValidationException("数据验证失败", modelStateErrors);

                return AjaxResultFactory.Success(bridgeMethod(parameter));
            }
            catch (Exception e)
            {
                return ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, bridgeMethod.Target.GetType().FullName, bridgeMethod.Method.Name);
            }
        }

        /// <summary>
        /// 有参有返回桥接（数据列表专用）
        /// </summary>
        /// <param name="bridgeMethod">有参有返回代理</param>
        /// <param name="pagination">参数</param>
        /// <param name="modelStateErrors">模型验证信息</param>
        /// <param name="request">请求信息</param>
        /// <returns></returns>
        private static object Bridge_List_Real(Func<Pagination, object> bridgeMethod, Pagination pagination, List<ModelErrorsInfo> modelStateErrors, HttpRequest request)
        {
            try
            {
                if (pagination == null)
                    throw new ValidationException("参数不能为空");

                if (modelStateErrors.Any_Ex())
                    throw new ValidationException("数据验证失败", modelStateErrors);

                return pagination.BuildResult(bridgeMethod(pagination));
            }
            catch (Exception e)
            {
                var result = ExceptionHelper.HandleException(e, request != null && request.Path.HasValue ? request.Path.Value : null, bridgeMethod.Target.GetType().FullName, bridgeMethod.Method.Name);
                return pagination.BuildResult(null, false, result.Msg);
            }
        }

        #endregion
    }
}
