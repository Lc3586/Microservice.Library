namespace Microservice.Library.Cache.Model
{
    /// <summary>
    /// 过期类型
    /// </summary>
    public enum ExpireType
    {
        /// <summary>
        /// 绝对过期
        /// </summary>
        /// <remarks>即自创建一段时间后就过期</remarks>
        Absolute,

        /// <summary>
        /// 相对过期
        /// </summary>
        /// <remarks>
        /// <para>即该键未被访问后一段时间后过期，</para>
        /// <para>若此键一直被访问则过期时间自动延长</para>
        /// </remarks>
        Relative,
    }
}
