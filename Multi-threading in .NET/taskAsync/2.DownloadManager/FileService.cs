using System;
using System.IO;
using System.Threading.Tasks;

namespace _2.DownloadManager
{
	class FileService
	{
		public async Task SaveToFileAsync(Stream content, string fileName)
		{
			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
			using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
			{
				await content.CopyToAsync(fileStream);
			}
		}
	}
}
