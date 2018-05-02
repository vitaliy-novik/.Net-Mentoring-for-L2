using Common;
using System;
using System.Messaging;

namespace ImageBondingService
{
	class ClientQueueService
	{
		private const string ServerQueueName = @".\private$\ServerQueue";
		private const string ClientsQueuesPrefix = @".\private$\ClientQueue";
		private MessagingService messagingService;
		private bool statusUpdated = false;

		public ClientQueueService(string guid)
		{
			this.messagingService = new MessagingService(ClientsQueuesPrefix + guid);
		}

		public void SendDocument(ServiceState state)
		{
			if (state.DocumentFinished)
			{
				Message message = new Message();
				message.BodyStream = state.Document;
				this.messagingService.SendMessage(message, ServerQueueName);
				state.DocumentFinished = false;
			}
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
