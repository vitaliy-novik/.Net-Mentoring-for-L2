using Castle.DynamicProxy;
using ImageBondingService.Logging;
using System;

namespace ImageBondingService.AOP
{
	public class CastleInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			Logger.LogCall(invocation.Method, invocation.Arguments);
			invocation.Proceed();
			Logger.LogRet(invocation.Method, invocation.ReturnValue);
		}

	}
}
