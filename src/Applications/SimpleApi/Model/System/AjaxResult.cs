

namespace Model.System
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// <see cref="System.ErrorCode"/>
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult<T> : AjaxResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// Ajax请求结果（开发模式）
    /// </summary>
    public class AjaxDevelopmentResult<T> : AjaxResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExMsg { get; set; }
    }

    /// <summary>
    /// 异常代码
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// 无
        /// </summary>
        none,
        /// <summary>
        /// 未登录
        /// </summary>
        nologin,
        /// <summary>
        /// 未授权
        /// </summary>
        unauthorized,
        /// <summary>
        /// 权限不足
        /// </summary>
        forbidden,
        /// <summary>
        /// 验证失败
        /// </summary>
        validation,
        /// <summary>
        /// 业务错误
        /// </summary>
        business,
        /// <summary>
        /// 系统错误
        /// </summary>
        error
    }

    public static class AjaxResultFactory
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static AjaxResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "成功！"
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static AjaxResult<T> Success<T>()
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = "成功！",
                Data = default
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <returns></returns>
        public static AjaxResult Success(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <returns></returns>
        public static AjaxResult<T> Success<T>(string msg)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = msg,
                Data = default
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static AjaxResult<T> Success<T>(T data)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = "成功！",
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public static AjaxResult<T> Success<T>(string msg, T data)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = msg,
                Data = data
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public static AjaxResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "失败！"
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public static AjaxResult<T> Error<T>()
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = false,
                Msg = "失败！",
                Data = default
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="errorCode">错误代码<see cref="ErrorCode"/></param>
        /// <returns></returns>
        public static AjaxResult Error(string msg, ErrorCode errorCode = ErrorCode.none)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
                ErrorCode = (int)errorCode
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="errorCode">错误代码<see cref="ErrorCode"/></param>
        /// <returns></returns>
        public static AjaxResult<T> Error<T>(string msg, ErrorCode errorCode = ErrorCode.none)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = false,
                Msg = msg,
                Data = default,
                ErrorCode = (int)errorCode
            };

            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <param name="errorCode">错误代码<see cref="ErrorCode"/></param>
        /// <returns></returns>
        public static AjaxResult<T> Error<T>(T data, ErrorCode errorCode = ErrorCode.none)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = false,
                Msg = "失败！",
                Data = data,
                ErrorCode = (int)errorCode
            };

            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的数据</param>
        /// <param name="errorCode">错误代码<see cref="ErrorCode"/></param>
        /// <returns></returns>
        public static AjaxResult<T> Error<T>(string msg, T data, ErrorCode errorCode = ErrorCode.none)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = false,
                Msg = msg,
                Data = data,
                ErrorCode = (int)errorCode
            };

            return res;
        }

        /// <summary>
        /// 返回错误（开发模式）
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="exMsg">异常信息</param>
        /// <param name="data">返回的数据</param>
        /// <param name="errorCode">错误代码<see cref="ErrorCode"/></param>
        /// <returns></returns>
        public static AjaxDevelopmentResult<T> Error<T>(string msg, string exMsg, T data, ErrorCode errorCode = ErrorCode.none)
        {
            AjaxDevelopmentResult<T> res = new AjaxDevelopmentResult<T>
            {
                Success = false,
                Msg = msg,
                Data = data,
                ExMsg = exMsg,
                ErrorCode = (int)errorCode
            };

            return res;
        }
    }
}
