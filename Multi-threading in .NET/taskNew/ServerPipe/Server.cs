using System;
using System.Collections.Generic;
using System.Threading;

namespace ServerPipe
{
	public class Server
	{
		private PipeConnection mainConnection;
		private List<PipeConnection> connections;
		private const string mainPipeName = "Main_Pipe";
		private MessageStorage messageStorage;

		public Server()
		{
			connections = new List<PipeConnection>();
			mainConnection = new PipeConnection(mainPipeName);
			messageStorage = new MessageStorage(50);
		}

		public void Start()
		{
			while (true)
			{
				mainConnection.StartConnection();
				RedirectClient();
				mainConnection.Disconnect();
			}
		}

		private void RedirectClient()
		{
			string newConnectionName = GenerateConnectionName();
			PipeConnection newConnection = new PipeConnection(newConnectionName);
			connections.Add(newConnection);
			StartConnectionForClient(newConnection);
			// Send to client it's new connection name
			mainConnection.SendMessage(newConnectionName);
		}

		private string GenerateConnectionName()
		{
			return $"{mainPipeName}{connections.Count + 1}";
		}

		private void StartConnectionForClient(PipeConnection pipeConnection)
		{
			ThreadPool.QueueUserWorkItem(state =>
			{
				pipeConnection.StartConnection();
				SendHistory(pipeConnection);
				while (true)
				{
					string input = pipeConnection.WaitMessage();
					Message message = new Message
					{
						Text = input,
						ClientName = pipeConnection.GetClientName(),
						Date = DateTime.Now
					};
					this.messageStorage.Add(message);
					Console.WriteLine(message.ToString());
					BroadcastMessage(message.ToString());
				}
			});
		}

		private void SendHistory(PipeConnection connection)
		{
			foreach (var message in messageStorage.GetMessages())
			{
				connection.SendMessage(message.ToString());
			};
		}

		private void BroadcastMessage(string message)
		{
			foreach (var connection in connections)
			{
				connection.SendMessage(message);
			}
		}

	}
}
