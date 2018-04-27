using System;
using System.IO;
using System.Messaging;
using System.Threading;

namespace DocumentQueueService
{
	class DocumentCollectorService
	{
		const string ServerQueueName = @".\Private$\DocumentsQueue";
		private string outDir;
		Thread workThread;
		ManualResetEvent stopWorkEvent;

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
			using (var serverQueue = new MessageQueue(ServerQueueName))
			{
				serverQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

				while (true)
				{
					IAsyncResult asyncReceive = serverQueue.BeginPeek();

					int res = WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, asyncReceive.AsyncWaitHandle });
					if (res == 0)
						break;

					var message = serverQueue.EndPeek(asyncReceive);
					serverQueue.ReceiveById(message.Id);

					var clientQueue = message.ResponseQueue;

					if (clientQueue == null)
					{
						clientQueue.Send(new Message("") { CorrelationId = message.Id });
					}

					string text = string.Format("Received {0}\n", message.Body);
				}
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
