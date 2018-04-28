using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ImageBondingService
{
	class ImageBondingService
	{
		private MessagingService messagingService;
		private PdfService pdfService;
		private FileSystemWatcher watcher;
		private string inDir;
		private string outDir;
		private int lastFileNumber = -1;
		private Regex imageRegex = new Regex(@"^image_\d+.(jpg|png)$");

		private Thread workThread;

		private ManualResetEvent stopWorkEvent;
		private AutoResetEvent newFileEvent;
		private AutoResetEvent newSettingsEvent;

		public ImageBondingService(string inDir, string outDir)
		{
			this.pdfService = new PdfService();
			this.messagingService = new MessagingService();
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
			this.newSettingsEvent = new AutoResetEvent(false);
		}

		private void WorkProcedure(object obj)
		{
			this.messagingService.RecieveSettings();
			Stream doc;
			do
			{
				foreach (var file in Directory.EnumerateFiles(inDir).OrderBy(f => f))
				{
					if (stopWorkEvent.WaitOne(TimeSpan.Zero))
						return;

					string inFile = file;
					string fileName = Path.GetFileName(file);
					string outFile = Path.Combine(outDir, fileName);

					if (this.imageRegex.IsMatch(fileName) && this.TryOpen(inFile, 3))
					{
						if (this.EndDocument(fileName))
						{
							doc = this.pdfService.GetDocument();
							this.messagingService.SendDocument(doc);
						}
						if (File.Exists(outFile))
						{
							File.Delete(inFile);
						}
						else
						{
							File.Move(inFile, outFile);
						}
						this.pdfService.InsetImage(outFile);
					}
				}

			}
			while (WaitHandle.WaitAny(new WaitHandle[] { stopWorkEvent, newFileEvent, newSettingsEvent }, 1000) != 0);

			doc = this.pdfService.GetDocument();
			this.messagingService.SendDocument(doc);
		}

		private void Watcher_Created(object sender, FileSystemEventArgs e)
		{
			newFileEvent.Set();
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

		public bool EndDocument(string fileName)
		{
			string resultString = Regex.Match(fileName, @"\d+").Value;
			int number = Int32.Parse(resultString);
			if (this.lastFileNumber == -1 || number == this.lastFileNumber + 1)
			{
				this.lastFileNumber = number;
				return false;
			}

			return true;
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
