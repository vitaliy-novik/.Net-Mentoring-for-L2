using System.IO;
using System.Messaging;

namespace ImageBondingService
{
	class MessagingService
	{
		private MessageQueue documentsQueue;
		private MessageQueue settingsQueue;
		private const string DocumentsQueueName = @".\private$\DocumentsQueue";
		private const string SettingsQueueName = @".\private$\SettingsQueue";
		private const string MulticastAddress = "234.1.1.1:8001";

		public MessagingService()
		{
			if (MessageQueue.Exists(DocumentsQueueName))
				this.documentsQueue = new MessageQueue(DocumentsQueueName);
			else
				this.documentsQueue = MessageQueue.Create(DocumentsQueueName);

			if (MessageQueue.Exists(SettingsQueueName))
				this.settingsQueue = new MessageQueue(SettingsQueueName);
			else
				this.settingsQueue = MessageQueue.Create(SettingsQueueName, false);
			this.settingsQueue.MulticastAddress = MulticastAddress;
		}

		public void SendDocument(Stream document)
		{
			Message message = new Message();
			message.BodyStream = document;
			documentsQueue.Send(message);
		}

		public void RecieveSettings()
		{
			this.settingsQueue.PeekCompleted += new PeekCompletedEventHandler(SettingsRecieved);
			this.settingsQueue.BeginPeek();
		}

		private void SettingsRecieved(object sender, PeekCompletedEventArgs e)
		{
			Message message = this.settingsQueue.EndPeek(e.AsyncResult);

			this.settingsQueue.Receive();
			this.settingsQueue.BeginPeek();
		}

	}
}
