using System;

namespace ServerPipe
{
	class Message
	{
		public string ClientName { get; set; }
		public DateTime Date { get; set; }
		public string Text { get; set; }

		public override string ToString()
		{
			return $"[{ClientName}, {Date.ToLongTimeString()}]: {Text}";
		}
	}
}
