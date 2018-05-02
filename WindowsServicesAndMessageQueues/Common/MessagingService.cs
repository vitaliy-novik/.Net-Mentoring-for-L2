using System;
using System.Messaging;

namespace Common
{
	public class MessagingService
	{
		private MessageQueue sendQueue;
		private MessageQueue responseQueue;
		private readonly string replayTo;
		private readonly string path;

		public MessagingService(string sendPath, string replayTo)
		{
			if (!MessageQueue.Exists(path))
			{
				MessageQueue.Create(path);
			}
			if (!MessageQueue.Exists(replayTo))
			{
				MessageQueue.Create(replayTo);
			}

			this.replayTo = replayTo;
			this.replayTo = replayTo;
		}

		public void SendMessage<T>(T messageBody)
		{
			using (this.sendQueue = new MessageQueue(this.path))
			{
				using (this.responseQueue = new MessageQueue(this.replayTo))
				{
					Message message = new Message(messageBody)
					{
						ResponseQueue = this.responseQueue
					};
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

		private void MessageRecieved(object sender, PeekCompletedEventArgs e)
		{
			
		}
	}
}
