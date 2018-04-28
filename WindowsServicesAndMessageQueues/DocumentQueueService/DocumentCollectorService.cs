using System;
using System.IO;
using System.Messaging;
using System.Threading;

namespace DocumentQueueService
{
	class DocumentCollectorService
	{
		private const string ServerQueueName = @".\Private$\DocumentsQueue";
		private string outDir;
		private Thread workThread;
		private ManualResetEvent stopWorkEvent;
		private int fileCounter;


		public DocumentCollectorService(string outDir)
		{
			this.outDir = outDir;

			if (!Directory.Exists(outDir))
				Directory.CreateDirectory(outDir);

			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
		}

		private void WorkProcedure()
		{
			using (MessageQueue serverQueue = new MessageQueue(ServerQueueName))
			{
				while (true)
				{
					IAsyncResult asyncReceive = serverQueue.BeginPeek();

					int res = WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, asyncReceive.AsyncWaitHandle });
					if (res == 0)
						break;

					Message message = serverQueue.EndPeek(asyncReceive);
					serverQueue.ReceiveById(message.Id);
					this.SaveFile(message.BodyStream);
				}
			}
		}

		private void SaveFile(Stream bodyStream)
		{
			string filePath = Path.Combine(this.outDir, $"{this.fileCounter++}.pdf");
			using (FileStream fileStream = File.Create(filePath))
			{
				bodyStream.Seek(0, SeekOrigin.Begin);
				bodyStream.CopyTo(fileStream);
				fileCounter++;
			}
		}

		public void Start()
		{
			workThread.Start();
		}

		public void Stop()
		{
			stopWorkEvent.Set();
			workThread.Join();
		}
	}
}
