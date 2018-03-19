using System.Collections.Generic;

namespace ConsolePipeServer
{
	class MessageStorage
	{
		private int maxMessagesNumber;
		private LinkedList<Message> messageList;

		public MessageStorage(int maxMessagesNumber)
		{
			messageList = new LinkedList<Message>();
		}

		public void Add(Message message)
		{
			messageList.AddLast(message);
			if (messageList.Count > maxMessagesNumber)
			{
				messageList.RemoveFirst();
			}
		}

		public IEnumerable<Message> GetMessages()
		{
			return messageList;
		}
	}
}
