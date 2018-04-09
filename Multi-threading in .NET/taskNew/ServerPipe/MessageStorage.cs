using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ServerPipe
{
	class MessageStorage
	{
		private int capacity;
		private ConcurrentQueue<Message> messageQueue;

		public MessageStorage(int maxMessagesNumber)
		{
			messageQueue = new ConcurrentQueue<Message>();
			this.capacity = maxMessagesNumber;
		}

		public void Add(Message message)
		{
			messageQueue.Enqueue(message);
			if (messageQueue.Count > capacity)
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
