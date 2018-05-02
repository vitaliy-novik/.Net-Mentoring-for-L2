using Common;
using System;
using System.IO;
using System.Messaging;

namespace ImageBondingService
{
	class ClientQueueService
	{
		private const string ServerQueueName = @".\private$\ServerQueue";
		private const string ClientsQueuesPrefix = @".\private$\ClientsQueues\";
		private MessagingService messagingService;
		private bool statusUpdated = false;

		public ClientQueueService(string guid)
		{
			this.messagingService = new MessagingService(ServerQueueName, ClientsQueuesPrefix + guid);
		}

		public void SendDocument(Stream document)
		{
			Message message = new Message();
			message.BodyStream = document;
			this.messagingService.SendMessage(document);
		}

		public void RecieveSettings(Action<ClientStatus> callback)
		{
			if (statusUpdated)
			{
				this.messagingService.RecieveMessage<ClientStatus>(message =>
				{
					this.statusUpdated = true;
					callback((ClientStatus)message.Body);
				});
			}
		}

	}
}
