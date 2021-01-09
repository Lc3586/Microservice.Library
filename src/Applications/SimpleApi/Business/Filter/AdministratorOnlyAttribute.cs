using Business.Interface.System;
using Castle.DynamicProxy;
using Library.Container;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (!Operator.IsAdmin)
                invocation.ReturnValue = AjaxResultFactory.Error("无权限");
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
