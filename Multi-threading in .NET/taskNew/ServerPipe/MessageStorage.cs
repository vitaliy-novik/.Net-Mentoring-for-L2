using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ServerPipe
{
	class MessageStorage
	{
		private int maxMessagesNumber;
		private ConcurrentQueue<Message> messageQueue;

		public MessageStorage(int maxMessagesNumber)
		{
			messageQueue = new ConcurrentQueue<Message>();
			this.maxMessagesNumber = maxMessagesNumber;
		}

		public void Add(Message message)
		{
			messageQueue.Enqueue(message);
			if (messageQueue.Count > maxMessagesNumber)
			{
				Message removal;
				messageQueue.TryDequeue(out removal);
			}
		}

		public IEnumerable<Message> GetMessages()
		{
			return messageQueue;
		}
	}
}
