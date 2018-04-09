using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.DownloadManager
{
	class FileService
	{
		public async void SaveToFileAsync(Stream content, string fileName)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
			using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
			{
				await content.CopyToAsync(fileStream);
			}
		}
	}
}
