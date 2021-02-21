namespace Model.Utils.Result
{
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
}
