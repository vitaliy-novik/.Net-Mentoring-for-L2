using System.IO;

namespace ImageBondingService.Interfaces
{
	public interface IFileSystemService
	{
		void ReadFiles(object sender, FileSystemEventArgs e);
		void Start();
	}
}
