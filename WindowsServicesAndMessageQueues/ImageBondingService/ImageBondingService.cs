using System;
using System.IO;
using System.Threading;

namespace ImageBondingService
{
	class ImageBondingService
	{
		private ClientQueueService messagingService;

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
			this.guid = Guid.NewGuid().ToString("N");
			this.messagingService = new ClientQueueService(this.guid);
			
		}

		private void WorkProcedure(object obj)
		{
			ServiceState state = new ServiceState();

			//do
			//{
			//	this.messagingService.RecieveSettings(this.Settings_Updated);
			//	this.fileSystemService.Run(state);
			//	this.pdfService.Run(state);
			//	this.messagingService.SendDocument(state);
			//}
			//while (WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, newFileEvent, newSettingsEvent }, 1000) != 0);

			ThreadPool.QueueUserWorkItem(this.ProcessImages, state);
			ThreadPool.QueueUserWorkItem(this.ListenServer, state);

			this.stopWorkEvent.WaitOne();
		}

		private void ListenServer(object state)
		{
			
		}

		private void ProcessImages(object state)
		{

		}

		public void Start()
		{
			this.workThread.Start();
		}

		public void Stop()
		{
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
