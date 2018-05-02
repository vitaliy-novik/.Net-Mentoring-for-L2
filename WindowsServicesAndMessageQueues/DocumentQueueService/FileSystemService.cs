using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentQueueService
{
	class FileSystemService
	{
		private string outDir;
		private int lastFileNumber = 0;

		public FileSystemService(string outDir)
		{
			this.outDir = outDir;

			if (!Directory.Exists(outDir))
				Directory.CreateDirectory(outDir);
		}

		public void SaveDocument(ServiceState state)
		{
			if (!state.DocumentSaved && state.Documents != null)
			{
				string filePath = Path.Combine(this.outDir, $"{this.lastFileNumber++}.pdf");
				using (FileStream fileStream = File.Create(filePath))
				{
					state.Documents.Seek(0, SeekOrigin.Begin);
					state.Documents.CopyTo(fileStream);
				}
			}
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
