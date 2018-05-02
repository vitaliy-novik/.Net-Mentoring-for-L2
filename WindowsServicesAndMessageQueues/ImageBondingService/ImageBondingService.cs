using System;
using System.IO;
using System.Threading;

namespace ImageBondingService
{
	class ImageBondingService
	{
		private ClientQueueService messagingService;
		private FileSystemWatcher watcher;
		private PdfService pdfService;

		private Thread workThread;

		private ManualResetEvent stopWorkEvent;
		private AutoResetEvent newFileEvent;
		private AutoResetEvent newSettingsEvent;
		private FileSystemService fileSystemService;
		private string guid;

		public ImageBondingService(string inDir, string outDir)
		{
			this.fileSystemService = new FileSystemService(inDir, outDir);
			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
			this.newFileEvent = new AutoResetEvent(false);
			this.newSettingsEvent = new AutoResetEvent(false);
			this.watcher = new FileSystemWatcher(inDir);
			this.watcher.Created += Watcher_Created;
			this.guid = Guid.NewGuid().ToString();
			this.messagingService = new ClientQueueService(this.guid);
			this.pdfService = new PdfService();
		}

		private void WorkProcedure(object obj)
		{
			ServiceState state = new ServiceState();
			
			do
			{
				this.messagingService.RecieveSettings(this.Settings_Updated);
				this.fileSystemService.Run(state);
				this.pdfService.Run(state);
				this.messagingService.SendDocument(state.Document);
			}
			while (WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, newFileEvent, newSettingsEvent }, 1000) != 0);
		}

		public void Start()
		{
			this.workThread.Start();
			this.watcher.EnableRaisingEvents = true;
		}

		public void Stop()
		{
			this.watcher.EnableRaisingEvents = false;
			this.stopWorkEvent.Set();
			this.workThread.Join();
		}

		private void Watcher_Created(object sender, FileSystemEventArgs e)
		{
			this.newFileEvent.Set();
		}

		private void Settings_Updated(ClientStatus status)
		{
			this.newSettingsEvent.Set();
		}
	}
}
