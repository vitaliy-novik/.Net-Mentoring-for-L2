using System;
using System.Messaging;

namespace Common
{
	public class MessagingService
	{
		private MessageQueue responseQueue;
		private readonly string replayTo;

		public MessagingService(string replayTo)
		{
			if (!MessageQueue.Exists(replayTo))
			{
				MessageQueue.Create(replayTo);
			}

			this.replayTo = replayTo;
		}

		public void SendMessage(Message message, MessageQueue targetQueue)
		{
			using (this.responseQueue = new MessageQueue(this.replayTo))
			{
				message.ResponseQueue = this.responseQueue;

				targetQueue.Send(message);
			}
		}

		public void SendMessage(Message message, string targetQueuePath)
		{
			if (!MessageQueue.Exists(replayTo))
			{
				MessageQueue.Create(replayTo);
			}

			using (MessageQueue targetQueue = new MessageQueue(targetQueuePath))
			{
				using (this.responseQueue = new MessageQueue(this.replayTo))
				{
					message.ResponseQueue = this.responseQueue;

					targetQueue.Send(message);
				}
			}
		}

		public void RecieveMessage<T>(Action<Message> callback)
		{
			using (this.responseQueue = new MessageQueue(this.replayTo))
			{
				this.responseQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(T) });
				this.responseQueue.PeekCompleted += new PeekCompletedEventHandler((sender, eventArgs) => {
					Message message = this.responseQueue.EndPeek(eventArgs.AsyncResult);
					this.responseQueue.Receive();
					this.responseQueue.BeginPeek();
					callback(message);
				});
				this.responseQueue.BeginPeek();
			}
		}
	}
}
