using PostSharp.Aspects;
using System;

namespace ImageBondingService.AOP
{
	[Serializable]
	class PostsharpAspect : OnMethodBoundaryAspect
	{
		public override void OnEntry(MethodExecutionArgs args)
		{
			Console.Write("Calling: " + args.Method.Name + "\n");
		}

		public override void OnExit(MethodExecutionArgs args)
		{
			Console.Write("Exiting: " + args.Method.Name + "\n");
		}
	}
}
