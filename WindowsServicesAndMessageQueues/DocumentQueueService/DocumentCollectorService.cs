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
		private FileSystemWatcher watcher;
		private int fileCounter;

		public DocumentCollectorService(string outDir, string settingsFile)
		{
			this.outDir = outDir;
			this.settingsFile = settingsFile;

			if (!Directory.Exists(outDir))
				Directory.CreateDirectory(outDir);

			this.watcher = new FileSystemWatcher(Path.GetDirectoryName(settingsFile));
			this.watcher.Filter = Path.GetFileName(settingsFile);
			this.watcher.Changed += Watcher_Changed;

			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
		}

		private void WorkProcedure()
		{
			using (MessageQueue documentsQueue = new MessageQueue(DocumentsQueueName))
			{
				while (true)
				{
					IAsyncResult asyncReceive = documentsQueue.BeginPeek();
					int res = WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, asyncReceive.AsyncWaitHandle });
					if (res == 0)
					{
						break;
					}

					Message message = documentsQueue.EndPeek(asyncReceive);
					documentsQueue.ReceiveById(message.Id);
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

		private void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			using (MessageQueue settingsQueue = new MessageQueue("formatname:multicast=234.1.1.1:8001"))
			{
				settingsQueue.Send("Multi-pulti");
			}
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
