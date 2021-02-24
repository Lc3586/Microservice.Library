using Library.Cache.Gen;
using Library.Cache.Services;
using Library.Container;

namespace Library.WebApp
{
    /// <summary>
    /// Session帮助类,自定义Session,解决原Session并发问题
    /// </summary>
    public class SessionHelper
    {
        #region 私有成员

        private static string CacheModuleName { get; } = "Session";
        private static string _sessionId { get => HttpContextCore.Current.Request.Cookies[SessionCookieName]; }
        private static string BuildCacheKey(string sessionKey)
        {
            return $"{CacheModuleName}_{_sessionId}_{sessionKey}";
        }

        private static ICache Cache = AutofacHelper.GetService<ICacheProvider>().GetCache();

        #endregion

        #region 外部成员

        /// <summary>
        /// 存放Session标志的Cookie名
        /// </summary>
        public static string SessionCookieName { get; } = "ASP.NETCore_Session_Id";

        /// <summary>
        /// 当前Session
        /// </summary>
        public static _Session Session { get; } = new _Session();

        /// <summary>
        /// 自定义_Session类
        /// </summary>
        public class _Session
        {
            public object this[string index]
            {
                get
                {
                    string cacheKey = BuildCacheKey(index);
                    return Cache.GetCache(cacheKey);
                }
                set
                {
                    string cacheKey = BuildCacheKey(index);
                    if (value == null || value.ToString() == string.Empty)
                        Cache.RemoveCache(cacheKey);
                    else
                        Cache.SetCache(cacheKey, value);
                }
            }
        }

        #endregion
    }
}
