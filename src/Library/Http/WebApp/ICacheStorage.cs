namespace Microservice.Library.Http.WebApp
{
    /// <summary>
    /// 缓存存储接口
    /// </summary>
    public interface ICacheStorage
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        void SetCache(string key, object value);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">主键</param>
        object GetCache(string key);

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">主键</param>
        void RemoveCache(string key);
    }
}
