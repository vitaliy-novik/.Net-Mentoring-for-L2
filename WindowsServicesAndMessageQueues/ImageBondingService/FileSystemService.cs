using ImageBondingService.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Unity.Attributes;

namespace ImageBondingService
{
	public class FileSystemService : IFileSystemService
	{
		private string inDir;
		private string outDir;
		private FileSystemWatcher watcher;
		private int lastFileNumber = -1;
		private Regex imageRegex = new Regex(@"^image_\d+.(jpg|png)$");
		private PdfService pdfService;

		[Dependency]
		public ClientQueueService messagingService { get; set; }

		public FileSystemService(string inDir, string outDir, string serviceGuid)
		{
			this.inDir = inDir;
			this.outDir = outDir;

			if (!Directory.Exists(inDir))
				Directory.CreateDirectory(inDir);

			if (!Directory.Exists(outDir))
				Directory.CreateDirectory(outDir);

			this.watcher = new FileSystemWatcher(inDir);
			this.pdfService = new PdfService();
			//this.messagingService = new ClientQueueService(serviceGuid);
		}

		public void Start()
		{
			this.watcher.Created += ReadFiles;
			this.watcher.EnableRaisingEvents = true;
		}

		public void ReadFiles(object sender, FileSystemEventArgs e)
		{
			this.messagingService.SendStatus(ClientStatus.Processing);
			foreach (var file in Directory.EnumerateFiles(inDir).OrderBy(f => f))
			{
				string inFile = file;
				string fileName = Path.GetFileName(file);
				string outFile = Path.Combine(outDir, fileName);

				if (this.imageRegex.IsMatch(fileName) && this.TryOpen(inFile, 3))
				{
					if (this.EndDocument(fileName))
					{
						Stream doc = this.pdfService.GetDocument();
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

			this.messagingService.SendStatus(ClientStatus.Waiting);
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

			this.lastFileNumber = number;
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
