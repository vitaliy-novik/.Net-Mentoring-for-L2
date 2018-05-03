﻿using Common;
using System;
using System.IO;
using System.Threading;

namespace ImageBondingService
{
	class ImageBondingService
	{
		private Thread workThread;

		private ManualResetEvent stopWorkEvent;
		private FileSystemService fileSystemService;
		private ClientQueueService queueService;
		private string guid;

		public ImageBondingService(string inDir, string outDir)
		{
			this.guid = Guid.NewGuid().ToString();
			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
			this.fileSystemService = new FileSystemService(inDir, outDir, this.guid);
			this.queueService = new ClientQueueService(this.guid);
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
				Settings settings = this.queueService.PeekSettings();
				if (settings != null)
				{
					Thread.Sleep(settings.Timeout);
				}
			}
		}

		private void ProcessImages(object state)
		{
			this.fileSystemService.Start();
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
