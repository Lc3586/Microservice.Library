using Autofac;
using Microsoft.AspNetCore.Http;

namespace Microservice.Library.Container
{
    /// <summary>
    /// 
    /// </summary>
    public class AutofacHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static ILifetimeScope Container { get; set; }

        /// <summary>
        /// 获取全局服务
        /// 警告：此方法使用不当会造成内存溢出,一般开发请勿使用此方法,请使用GetScopeService
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
#if DEBUG
            var type = typeof(T);
#endif
            if (!Container.IsRegistered<T>())
                return null;

            return Container.Resolve<T>();
        }

        /// <summary>
        /// 获取全局服务
        /// 警告：此方法使用不当会造成内存溢出,一般开发请勿使用此方法,请使用GetScopeService
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="service">服务</param>
        /// <returns></returns>
        public static bool TryGetService<T>(out T service) where T : class
        {
            service = null;
            try
            {
                service = GetService<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前请求为生命周期的服务
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns></returns>
        public static T GetScopeService<T>() where T : class
        {
#if DEBUG
            var type = typeof(T);
#endif
            var httpContextAccessor = GetService<IHttpContextAccessor>();
            if (httpContextAccessor?.HttpContext == null)
                return GetService<T>();
            else
                return (T)httpContextAccessor.HttpContext.RequestServices.GetService(typeof(T));
        }

        /// <summary>
        /// 获取当前请求为生命周期的服务
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="service">服务</param>
        /// <returns></returns>
        public static bool TryGetScopeService<T>(out T service) where T : class
        {
            service = null;
            try
            {
                service = GetScopeService<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}