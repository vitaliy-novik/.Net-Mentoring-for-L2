using System.IO;
using System.Messaging;

namespace ImageBondingService
{
	class MessagingService
	{
		private MessageQueue messageQueue;
		private const string queueName = @".\private$\DocumentsQueue";

		public MessagingService()
		{
			if (MessageQueue.Exists(queueName))
				messageQueue = new MessageQueue(queueName);
			else
				messageQueue = MessageQueue.Create(queueName);
		}

		public void SendDocument(Stream document)
		{
			Message message = new Message();
			message.BodyStream = document;
			messageQueue.Send(message);
		}

	}
}
