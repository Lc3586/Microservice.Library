using Business.Interface.System;
using Castle.DynamicProxy;
using Microservice.Library.Container;
using System;

namespace Business.Filter
{
    /// <summary>
    /// 仅限管理员
    /// </summary>
    public class AdministratorOnlyAttribute : BaseFilterAttribute
    {
        readonly IOperator Operator = AutofacHelper.GetScopeService<IOperator>();

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="invocation"></param>
        public override void OnActionExecuting(IInvocation invocation)
        {
            if (Operator.IsAuthenticated && !Operator.IsAdmin)
                throw new ApplicationException("无权限");
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="invocation"></param>
        public override void OnActionExecuted(IInvocation invocation)
        {

        }
    }
}
