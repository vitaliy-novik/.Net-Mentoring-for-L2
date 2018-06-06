using System;
using System.Collections.Generic;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace ImageBondingService.AOP
{
	public class EnterLogBehavior : IInterceptionBehavior
	{
		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public bool WillExecute
		{
			get { return true; }
		}

		public IMethodReturn Invoke(IMethodInvocation input,
			GetNextInterceptionBehaviorDelegate getNext)
		{
			Console.Write("Calling: " + input.MethodBase.Name + "\n");
			var methodReturn = getNext().Invoke(input, getNext);
			return methodReturn;
		}
	}

	public class ExitLogBehavior : IInterceptionBehavior
	{
		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		public bool WillExecute
		{
			get { return true; }
		}

		public IMethodReturn Invoke(IMethodInvocation input,
			GetNextInterceptionBehaviorDelegate getNext)
		{
			var methodReturn = getNext().Invoke(input, getNext);
			Console.Write("Exiting: " + input.MethodBase.Name + "\n");
			return methodReturn;
		}
	}
}
