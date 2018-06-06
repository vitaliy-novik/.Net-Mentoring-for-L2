using ImageBondingService.Interfaces;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.IO;
using Topshelf;
using Unity;
using Unity.Resolution;

namespace ImageBondingService
{
	class Program
	{
		static void Main(string[] args)
		{
			var currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			var inDir = Path.Combine(currentDir, "in");
			var outDir = Path.Combine(currentDir, "out");

			var conf = new LoggingConfiguration();
			var fileTarget = new FileTarget()
			{
				Name = "Default",
				FileName = Path.Combine(currentDir, "log.txt"),
				Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
			};
			conf.AddTarget(fileTarget);
			conf.AddRuleForAllLevels(fileTarget);

			var logFactory = new LogFactory(conf);


			IUnityContainer unityContainer = new UnityContainer();
			unityContainer.RegisterType<IClientQueueService, ClientQueueService>();
			ImageBondingService service = unityContainer.Resolve<ImageBondingService>(
				new ResolverOverride[]
				{
					new ParameterOverride("inDir", inDir),
					new ParameterOverride("outDir", outDir)
				});

			HostFactory.Run(
				hostConf => hostConf.Service<ImageBondingService>(
					s => {
						s.ConstructUsing(() => service);
						s.WhenStarted(serv => serv.Start());
						s.WhenStopped(serv => serv.Stop());
					}).UseNLog(logFactory));
		}
	}
}
