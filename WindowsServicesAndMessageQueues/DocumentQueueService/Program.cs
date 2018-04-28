using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.IO;
using Topshelf;

namespace DocumentQueueService
{
	class Program
	{
		static void Main(string[] args)
		{
			string currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			string outDir = Path.Combine(currentDir, "out");
			string settingsFile = Path.Combine(currentDir, "settings.txt");

			LoggingConfiguration conf = new LoggingConfiguration();
			FileTarget fileTarget = new FileTarget()
			{
				Name = "Default",
				FileName = Path.Combine(currentDir, "log.txt"),
				Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
			};
			conf.AddTarget(fileTarget);
			conf.AddRuleForAllLevels(fileTarget);

			LogFactory logFactory = new LogFactory(conf);

			HostFactory.Run(
				hostConf => hostConf.Service<DocumentCollectorService>(
					s => {
						s.ConstructUsing(() => new DocumentCollectorService(outDir, settingsFile));
						s.WhenStarted(serv => serv.Start());
						s.WhenStopped(serv => serv.Stop());
					}).UseNLog(logFactory));
		}
	}
}
