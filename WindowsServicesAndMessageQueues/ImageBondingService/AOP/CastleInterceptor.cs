using Castle.DynamicProxy;
using System;

namespace ImageBondingService.AOP
{
	public class CastleInterceptor : IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			Console.Write("Calling: " + invocation.Method.Name + "\n");
			invocation.Proceed();
			Console.Write("Exiting: " + invocation.Method.Name + "\n");
		}

	}
}
