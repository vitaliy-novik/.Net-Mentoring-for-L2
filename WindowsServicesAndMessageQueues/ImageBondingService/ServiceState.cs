using System.IO;

namespace ImageBondingService
{
	class ServiceState
	{
		public string NextImage;

		public ClientStatus Status { get; set; }

		public Stream Document { get; set; }

		public bool DocumentFinished { get; set; }
	}
}
