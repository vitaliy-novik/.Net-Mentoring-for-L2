using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ImageBondingService
{
	class FileSystemService
	{
		private string inDir;
		private string outDir;
		private int lastFileNumber = -1;
		private Regex imageRegex = new Regex(@"^image_\d+.(jpg|png)$");
		private ClientQueueService messagingService;

		public FileSystemService(string inDir, string outDir)
		{
			this.inDir = inDir;
			this.outDir = outDir;

			if (!Directory.Exists(inDir))
				Directory.CreateDirectory(inDir);

			if (!Directory.Exists(outDir))
				Directory.CreateDirectory(outDir);
		}

		public void Run(ServiceState state)
		{
			foreach (var file in Directory.EnumerateFiles(inDir).OrderBy(f => f))
			{
				string inFile = file;
				string fileName = Path.GetFileName(file);
				string outFile = Path.Combine(outDir, fileName);

				if (this.imageRegex.IsMatch(fileName) && this.TryOpen(inFile, 3))
				{
					state.DocumentFinished = this.EndDocument(fileName);
					if (File.Exists(outFile))
					{
						File.Delete(inFile);
					}
					else
					{
						File.Move(inFile, outFile);
					}

					state.NextImage = outFile;
				}
			}
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
