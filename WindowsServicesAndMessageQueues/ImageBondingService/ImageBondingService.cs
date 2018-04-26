using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ImageBondingService
{
	class ImageBondingService
	{
		private PdfService pdfService;
		private FileSystemWatcher watcher;
		private string inDir;
		private string outDir;
		private string lastFile;

		Thread workThread;

		ManualResetEvent stopWorkEvent;
		AutoResetEvent newFileEvent;

		public ImageBondingService(string inDir, string outDir)
		{
			this.pdfService = new PdfService();
			this.inDir = inDir;
			this.outDir = outDir;

			if (!Directory.Exists(inDir))
				Directory.CreateDirectory(inDir);

			if (!Directory.Exists(outDir))
				Directory.CreateDirectory(outDir);

			this.watcher = new FileSystemWatcher(inDir);
			watcher.Created += Watcher_Created;

			this.workThread = new Thread(WorkProcedure);
			this.stopWorkEvent = new ManualResetEvent(false);
			this.newFileEvent = new AutoResetEvent(false);
		}

		private void WorkProcedure(object obj)
		{
			do
			{
				foreach (var file in Directory.EnumerateFiles(inDir))
				{
					if (stopWorkEvent.WaitOne(TimeSpan.Zero))
						return;

					var inFile = file;
					var outFile = Path.Combine(outDir, Path.GetFileName(file));

					if (this.TryOpen(inFile, 3))
					{
						this.pdfService.InsetImage(inFile);
					}
				}

			}
			while (WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, newFileEvent }, 1000) != 0);
		}

		private void Watcher_Created(object sender, FileSystemEventArgs e)
		{
			newFileEvent.Set();
		}

		public void Start()
		{
			workThread.Start();
			watcher.EnableRaisingEvents = true;
		}

		public void Stop()
		{
			watcher.EnableRaisingEvents = false;
			stopWorkEvent.Set();
			workThread.Join();
		}

		private bool TryOpen(string fileName, int tryCount)
		{
			for (int i = 0; i < tryCount; i++)
			{
				try
				{
					var file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
					file.Close();

					return true;
				}
				catch (IOException)
				{
					Thread.Sleep(5000);
				}
			}

			return false;
		}
	}
}
