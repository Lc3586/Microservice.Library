using Castle.DynamicProxy;

namespace Microservice.Library.Container
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="invocation">执行信息</param>
        void OnActionExecuting(IInvocation invocation);

        /// <summary>
        /// Action执行后
        /// </summary>
        /// <param name="invocation">执行信息</param>
        void OnActionExecuted(IInvocation invocation);
    }
}
