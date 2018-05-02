using Common;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Linq;
using System;

namespace DocumentQueueService
{
	class ServerQueueService
	{
		private const string ServerQueueName = @".\private$\ServerQueue";
		private List<MessageQueue> clientsQueues;
		private MessageQueue serverQueue;

		public ServerQueueService()
		{
			if (!MessageQueue.Exists(ServerQueueName))
			{
				MessageQueue.Create(ServerQueueName);
			}

			this.serverQueue = new MessageQueue(ServerQueueName);
			this.serverQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(ClientStatus), typeof(Document) });
		}

		public void RecieveStatus(ServiceState state)
		{
			this.serverQueue.RecieveMessage<ClientStatus>(message =>
			{
				Client client = state.Clients.FirstOrDefault(cl => cl.Queue.FormatName.Equals(message.ResponseQueue.FormatName));
				if (client != null && message.Body is ClientStatus)
				{
					client.Status = (ClientStatus)message.Body;
				}
			});
		}

		public void RecieveMessage(ServiceState state)
		{
			if (state.MessageRecieved)
			{
				Message message = this.serverQueue.Receive();
				string client = message.ResponseQueue.FormatName;
				if (message.Body is ClientStatus)
				{

				}
				else if (message.Body is Document)
				{

				}
			}
		}

		public void WaitMessage(ServiceState state)
		{
			this.serverQueue.PeekCompleted += new PeekCompletedEventHandler((sender, eventArgs) => {
				this.serverQueue.EndPeek(eventArgs.AsyncResult);
				state.MessageRecievedEvent.Set();
			});

			this.serverQueue.BeginPeek();
		}
	}
}
