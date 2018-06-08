using Castle.DynamicProxy;
using System;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;

namespace ImageBondingService.AOP
{
	public class UnityExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			var strategy = new ProxyBuilderStrategy(Container);

			Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
		}

		class ProxyBuilderStrategy : BuilderStrategy
		{
			private readonly IUnityContainer unityContainer;

			public ProxyBuilderStrategy(IUnityContainer container)
			{
				this.unityContainer = container;
			}

			public override void PostBuildUp(IBuilderContext context)
			{
				var key = context.OriginalBuildKey;

				if (key.Type.IsInterface && unityContainer.IsRegistered(key.Type))
				{
					ProxyGenerator generator = new ProxyGenerator();

					context.Existing = generator.CreateInterfaceProxyWithTarget(
						key.Type, 
						context.Existing, new CastleInterceptor());
				}
			}

			private object CreateDynamicProxy(Type type)
			{
				ProxyGenerator generator = new ProxyGenerator();

				return generator.CreateClassProxy(type, new CastleInterceptor());
			}
		}
	}
}
