namespace Model.Utils.Result
{
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
}
