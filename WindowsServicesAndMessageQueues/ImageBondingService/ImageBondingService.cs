using Common;
using ImageBondingService.Interfaces;
using System;
using System.Threading;
using Unity.Attributes;

namespace ImageBondingService
{
	public class ImageBondingService
	{
		private Thread workThread;
		private ManualResetEvent stopWorkEvent;
		private string guid;

		[Dependency]
		public IFileSystemService FileSystemService { get; set; }

		[Dependency]
		public IClientQueueService QueueService { get; set; }

		public ImageBondingService(string inDir, string outDir, string clientGuid)
		{
			this.guid = clientGuid;
			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
			//this.fileSystemService = new FileSystemService(inDir, outDir, this.guid);
			//this.queueService = new ClientQueueService(this.guid);
		}

		private void WorkProcedure(object obj)
		{
			ServiceState state = new ServiceState()
			{
				ClientGuid = Guid.NewGuid().ToString()
			};

			ThreadPool.QueueUserWorkItem(this.ProcessImages, state);
			ThreadPool.QueueUserWorkItem(this.ListenServer, state);

			this.stopWorkEvent.WaitOne();
		}

		private void ListenServer(object state)
		{
			while (true)
			{
				Settings settings = this.QueueService.PeekSettings();
				if (settings != null)
				{
					Thread.Sleep(settings.Timeout);
				}
			}
		}

		private void ProcessImages(object state)
		{
			this.FileSystemService.Start();
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
	}
}
