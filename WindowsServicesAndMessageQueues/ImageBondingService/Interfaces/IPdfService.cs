using System.IO;

namespace ImageBondingService.Interfaces
{
	public interface IPdfService
	{
		void InsetImage(string filePath);

		Stream GetDocument();
	}
}
