using System.Messaging;

namespace DocumentQueueService
{
	class Client
	{
		public ClientStatus Status { get; set; }

		public MessageQueue Queue { get; set; }
	}
}
