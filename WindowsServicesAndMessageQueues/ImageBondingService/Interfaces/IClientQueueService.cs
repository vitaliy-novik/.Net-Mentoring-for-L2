using Common;
using System.IO;

namespace ImageBondingService.Interfaces
{
	interface IClientQueueService
	{
		void SendDocument(Stream documentContent);

		Settings PeekSettings();

		void SendStatus(ClientStatus clientStatus);
	}
}
