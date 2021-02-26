using Castle.DynamicProxy;
using System;

namespace Microservice.Library.Container
{
    /// <summary>
    /// 拦截器基类
    /// </summary>
    public abstract class BaseFilterAttribute : Attribute, IFilter
    {
        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void OnActionExecuting(IInvocation invocation);

        /// <summary>
        /// Action执行后
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void OnActionExecuted(IInvocation invocation);
    }
}
