using Common;
using System.Messaging;
using System;

namespace DocumentQueueService
{
	class ServerQueueService
	{
		private const string ServerQueueName = @".\private$\ServerQueue";
		private const string ClientsQueuesPrefix = @".\private$\ClientQueue";
		private MessageQueue serverQueue;
		private MessageQueue clientQueue;

		public ServerQueueService()
		{
			if (MessageQueue.Exists(ServerQueueName))
				this.serverQueue = new MessageQueue(ServerQueueName);
			else
				this.serverQueue = MessageQueue.Create(ServerQueueName);

			if (MessageQueue.Exists(ClientsQueuesPrefix))
				this.clientQueue = new MessageQueue(ClientsQueuesPrefix);
			else
				this.clientQueue = MessageQueue.Create(ClientsQueuesPrefix);

			this.serverQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Status), typeof(Document) });
			this.clientQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Status), typeof(Settings) });
		}

		public void SendSettings(Settings settings)
		{
			this.clientQueue.Send(settings);
		}

		public Settings RecieveSettings()
		{
			return this.clientQueue.Receive().Body as Settings;
		}

		public object Recieve()
		{
			Message message = this.serverQueue.Receive();

			return message.Body;
		}
	}
}
