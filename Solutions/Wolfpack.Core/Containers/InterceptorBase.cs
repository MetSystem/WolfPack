using Castle.DynamicProxy;

namespace Wolfpack.Core.Containers
{
    public abstract class InterceptorBase : IInterceptor
    {
        protected string myMethodToIntercept;

        protected InterceptorBase(string methodToIntercept)
        {
            myMethodToIntercept = methodToIntercept;
        }

        protected virtual bool InterceptThisMethod(IInvocation invocation)
        {
            return (string.CompareOrdinal(invocation.Method.Name, myMethodToIntercept) == 0);
        }

        public void Intercept(IInvocation invocation)
        {
            if (!InterceptThisMethod(invocation))
                invocation.Proceed();
            else
            {
                HandleIntercept(invocation);
            }
        }

        protected abstract void HandleIntercept(IInvocation invocation);
    }
}