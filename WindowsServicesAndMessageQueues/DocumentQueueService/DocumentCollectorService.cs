using System;
using System.IO;
using System.Messaging;
using System.Threading;

namespace DocumentQueueService
{
	class DocumentCollectorService
	{
		private const string DocumentsQueueName = @".\Private$\DocumentsQueue";
		private const string SettingsQueueName = @".\private$\SettingsQueue";
		private readonly string outDir;
		private readonly string settingsFile;
		private Thread workThread;
		private ManualResetEvent stopWorkEvent;
		private AutoResetEvent settingsChangedEvent;
		private FileSystemWatcher watcher;
		private ServerQueueService queueService;
		private XmlService xmlService;
		private FileSystemService fileService;

		public DocumentCollectorService(string outDir, string settingsFile, string clientsFile)
		{
			this.outDir = outDir;
			this.settingsFile = settingsFile;

			this.watcher = new FileSystemWatcher(Path.GetDirectoryName(settingsFile));
			this.watcher.Filter = Path.GetFileName(settingsFile);
			this.watcher.Changed += Watcher_Changed;

			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
			this.settingsChangedEvent = new AutoResetEvent(false);
			this.queueService = new ServerQueueService();
			this.xmlService = new XmlService(clientsFile);
			this.fileService = new FileSystemService(outDir);
		}

		private void WorkProcedure()
		{
			ServiceState state = new ServiceState()
			{
				MessageRecievedEvent = new AutoResetEvent(false),
				SettingsChangedEvent = new AutoResetEvent(false)
			};

			do
			{
				this.queueService.WaitMessage(state);
				this.xmlService.SaveStatus(state);
				this.fileService.SaveDocument(state);
				this.queueService.RecieveStatus(state);
			}
			while (WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, settingsChangedEvent }, 1000) != 0);
		}

		private void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			this.settingsChangedEvent.Set();
		}

		public void Start()
		{
			workThread.Start();
			this.watcher.EnableRaisingEvents = true;
		}

		public void Stop()
		{
			this.watcher.EnableRaisingEvents = false;
			stopWorkEvent.Set();
			workThread.Join();
		}
	}
}
