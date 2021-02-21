

namespace Model.Utils.Result
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
        /// <see cref="Model.Utils.Result.ErrorCode"/>
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
    }
}
