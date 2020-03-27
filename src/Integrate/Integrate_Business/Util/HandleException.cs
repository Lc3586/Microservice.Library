using Integrate_Business.Config;
using Library.Container;
using Library.Extention;
using Library.Log;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Integrate_Business.Util
{
    /// <summary>
    /// 异常帮助类
    /// </summary>
    public class ExceptionHelper
    {
        /// <summary>
        /// 处理系统异常
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="url">请求地址</param>
        /// <param name="Target">目标</param>
        /// <param name="Method">方法</param>
        public static void ExceptionWriteLog(Exception ex, string url = null, string Target = null, string Method = null)
        {
            ILogger logger = AutofacHelper.GetScopeService<ILogger>();
            logger.Error(ex);
        }

        /// <summary>
        /// 处理系统异常
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="url">请求地址</param>
        /// <param name="Target">目标</param>
        /// <param name="Method">方法</param>
        /// <returns></returns>
        public static AjaxResult HandleException(Exception ex, string url = null, string Target = null, string Method = null)
        {
            ExceptionWriteLog(ex, url, Target, Method);
            return HandleException(ex, ex);
        }

        /// <summary>
        /// 处理系统异常
        /// </summary>
        /// <param name="ex">当前异常</param>
        /// <param name="base_ex">原异常</param>
        /// <returns></returns>
        public static AjaxResult HandleException(Exception ex, Exception base_ex)
        {
            string msg = null;
            object data = null;
            ErrorCode code = ErrorCode.none;
            AjaxResult result = null;
            if (ex.InnerException != null)
                result = HandleException(ex.InnerException, base_ex);
            if (result == null)
            {
                Type e_type = ex.GetType();
                if (e_type == typeof(MessageException))
                {
                    var _ex = ex as MessageException;
                    msg = _ex.Msg;
                    code = _ex.Code;
                }
                else if (e_type == typeof(ValidationException))
                {
                    var _ex = ex as ValidationException;
                    msg = _ex.Msg;
                    data = _ex.Data;
                    code = ErrorCode.validation;
                }

                if (SystemConfig.systemConfig.RunMode != RunMode.Publish)
                    result = AjaxResultFactory.Error(msg ?? "系统异常", base_ex.GetExceptionAllMsg(), data, code);
                else
                    result = AjaxResultFactory.Error(msg ?? "系统繁忙，请稍后重试", data, code);
            }

            return result;
        }
    }
}
