using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

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

		public void SendDocument()
		{
			using (messageQueue)
			{
				messageQueue.Send("Simple Message");
			}
		}

	}
}
