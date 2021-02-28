using Microservice.Library.Container;
using Microsoft.AspNetCore.Http;

namespace Microservice.Library.Http.WebApp
{
    /// <summary>
    /// Session帮助类,自定义Session,解决原Session并发问题
    /// </summary>
    public class SessionHelper
    {
        #region 私有成员

        static string CacheModuleName { get; } = "Session";

        static string _sessionId => AutofacHelper.GetService<IHttpContextAccessor>().HttpContext.Request.Cookies[SessionCookieName];

        static string BuildCacheKey(string sessionKey)
        {
            return $"{CacheModuleName}_{_sessionId}_{sessionKey}";
        }

        static ICacheStorage Storage = AutofacHelper.GetService<ICacheStorage>();

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
                    return Storage.GetCache(cacheKey);
                }
                set
                {
                    string cacheKey = BuildCacheKey(index);
                    if (value == null || value.ToString() == string.Empty)
                        Storage.RemoveCache(cacheKey);
                    else
                        Storage.SetCache(cacheKey, value);
                }
            }
        }

        #endregion
    }
}
