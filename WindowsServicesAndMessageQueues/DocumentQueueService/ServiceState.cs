using Common;
using System.Collections.Generic;
using System.Threading;

namespace DocumentQueueService
{
	class ServiceState
	{
		public ServiceState()
		{
			this.Clients = new List<Client>();
			this.Documents = new List<Document>();
		}

		public List<Client> Clients { get; set; }

		public bool ClientsUpdated { get; set; }

		public List<Document> Documents { get; set; }

		public bool MessageRecieved { get; set; }

		public bool SettingsChanged { get; set; }

		public AutoResetEvent MessageRecievedEvent { get; set; }

		public AutoResetEvent SettingsChangedEvent { get; set; }
	}
}
