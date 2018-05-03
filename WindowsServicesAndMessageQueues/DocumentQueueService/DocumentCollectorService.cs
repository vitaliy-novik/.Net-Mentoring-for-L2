using Common;
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

		private ServerQueueService queueService;
		private XmlService xmlService;
		private FileSystemService fileService;
		private DocumentsService documentService;

		public DocumentCollectorService(string outDir, string settingsFile, string clientsFile)
		{
			this.outDir = outDir;
			this.settingsFile = settingsFile;

			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);

			this.queueService = new ServerQueueService();
			this.xmlService = new XmlService(clientsFile);
			this.fileService = new FileSystemService(settingsFile);
			this.documentService = new DocumentsService(outDir);
		}

		private void WorkProcedure()
		{
			ServiceState state = new ServiceState();

			ThreadPool.QueueUserWorkItem(this.ProcessSettings, state);
			ThreadPool.QueueUserWorkItem(this.ListenClients, state);

			this.stopWorkEvent.WaitOne();
		}

		private void ListenClients(object state)
		{
			while (true)
			{
				object message = this.queueService.Recieve();
				if (message is Status)
				{
					this.xmlService.SaveStatus(message as Status);
				}
				else if (message is Document)
				{
					this.documentService.SaveDocument(message as Document);
				}
			}
		}

		private void ProcessSettings(object state)
		{
			this.fileService.Start();
		}

		public void Start()
		{
			workThread.Start();
		}

		public void Stop()
		{
			this.stopWorkEvent.Set();
			this.workThread.Join();
		}
	}
}
