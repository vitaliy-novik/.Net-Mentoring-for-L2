using Common;
using ImageBondingService.AOP;
using ImageBondingService.Interfaces;
using System;
using System.IO;
using System.Messaging;

namespace ImageBondingService
{
	public class ClientQueueService : IClientQueueService
	{
		private const string ServerQueueName = @".\private$\ServerQueue";
		private const string ClientsQueuesPrefix = @".\private$\ClientQueue";
		private MessageQueue serverQueue;
		private MessageQueue clientQueue;
		private string clientGuid;
		private const int MaxChunkSize = 3000000;

		public ClientQueueService(string clientGuid)
		{
			this.clientGuid = clientGuid;

			if (MessageQueue.Exists(ServerQueueName))
				this.serverQueue = new MessageQueue(ServerQueueName);
			else
				this.serverQueue = MessageQueue.Create(ServerQueueName);

			if (MessageQueue.Exists(ClientsQueuesPrefix))
				this.clientQueue = new MessageQueue(ClientsQueuesPrefix);
			else
				this.clientQueue = MessageQueue.Create(ClientsQueuesPrefix);

			this.clientQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Settings) });
		}

		[PostsharpAspect]
		public void SendDocument(Stream documentContent)
		{
			int countOfChunks = (int)(documentContent.Length / MaxChunkSize) + 1;
			string docId = Guid.NewGuid().ToString();

			for (int i = 0; i < countOfChunks; i++)
			{
				long bytesLeft = documentContent.Length - documentContent.Position;
				int chunkSize = bytesLeft > MaxChunkSize ? MaxChunkSize : (int)bytesLeft;
				Document doc = new Document
				{
					ClientId = this.clientGuid,
					DocumentId = docId,
					CountOfChunks = countOfChunks,
					ChunkNumber = i,
					Content = new byte[chunkSize]
				};

				documentContent.Read(doc.Content, 0, chunkSize);
				this.serverQueue.Send(doc);
			}
		}

		public Settings PeekSettings()
		{
			Message message = this.clientQueue.Peek();
			return message?.Body as Settings;
		}

		public void SendStatus(ClientStatus clientStatus)
		{
			Status status = new Status
			{
				ClientId = this.clientGuid,
				ClientStatus = clientStatus
			};

			this.serverQueue.Send(status);
		}
	}
}
