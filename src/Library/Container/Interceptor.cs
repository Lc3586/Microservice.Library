﻿using Castle.DynamicProxy;
using System.Linq;
using System.Reflection;

namespace Microservice.Library.Container
{
    public class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var allFilers = invocation.MethodInvocationTarget.GetCustomAttributes<BaseFilterAttribute>(true)
                .Concat(invocation.InvocationTarget.GetType().GetCustomAttributes<BaseFilterAttribute>(true))
                .Where(x => x is IFilter)
                .Select(x => (IFilter)x)
                .ToList();

            //执行前
            foreach (var aFiler in allFilers)
            {
                aFiler.OnActionExecuting(invocation);

                if (invocation.ReturnValue != null && !string.IsNullOrEmpty(invocation.ReturnValue.ToString()))
                    return;
            }

            //执行
            invocation.Proceed();

            //执行后
            allFilers.ForEach(aFiler =>
            {
                aFiler.OnActionExecuted(invocation);
            });
        }
    }
}
