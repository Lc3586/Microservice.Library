using Business.Utils.Log;
using Microservice.Library.Container;
using Microservice.Library.Extension;
using Model.Utils.Config;
using Model.Utils.Log;
using Model.Utils.Result;
using System;

namespace Business.Utils
{
    /// <summary>
    /// 异常帮助类
    /// </summary>
    public static class ExceptionHelper
    {
        readonly static SystemConfig Config = AutofacHelper.GetScopeService<SystemConfig>();

        /// <summary>
        /// 处理系统异常
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="url">请求地址</param>
        /// <param name="target">目标</param>
        /// <param name="method">方法</param>
        public static void ExceptionWriteLog(Exception exception, string url = null, string target = null, string method = null)
        {
            Logger.Log(NLog.LogLevel.Error, LogType.系统异常, "请求接口时发生异常.", $"url: {url}, \r\n\tTarget: {target}, \r\n\tMethod: {method}", exception);
        }

        /// <summary>
        /// 处理系统异常
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="url">请求地址</param>
        /// <param name="target">目标</param>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public static AjaxResult HandleException(Exception exception, string url = null, string target = null, string method = null)
        {
            ExceptionWriteLog(exception, url, target, method);
            return HandleException(exception, null);
        }

        /// <summary>
        /// 处理系统异常
        /// </summary>
        /// <param name="exception">当前异常</param>
        /// <param name="base_exception">原异常</param>
        /// <returns></returns>
        public static AjaxResult HandleException(Exception exception, Exception base_exception = null)
        {
            string msg = null;
            object data = null;
            ErrorCode code = ErrorCode.none;
            AjaxResult result = null;
            if (exception.InnerException != null)
                result = HandleException(exception.InnerException, base_exception);
            if (result == null)
            {
                Type e_type = exception.GetType();
                if (e_type == typeof(ApplicationException))
                {
                    var _ex = exception as ApplicationException;
                    msg = _ex.Message;
                    code = ErrorCode.error;
                }
                else if (e_type == typeof(MessageException))
                {
                    var _ex = exception as MessageException;
                    msg = _ex.Msg;
                    code = _ex.Code;
                }
                else if (e_type == typeof(ValidationException))
                {
                    var _ex = exception as ValidationException;
                    msg = _ex.Msg;
                    data = _ex.Data;
                    code = ErrorCode.validation;
                }

                if (base_exception != null)
                    return result;

                if (Config.RunMode != RunMode.Publish)
                    result = AjaxResultFactory.Error(msg ?? "系统异常", exception.GetExceptionAllMsg(), data, code);
                else
                    result = AjaxResultFactory.Error(msg ?? "系统繁忙，请稍后重试", data, code);
            }

            return result;
        }
    }
}
