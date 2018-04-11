using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientPipe
{
	public class Client
	{
		const string mainPipeName = "Main_Pipe";
		private PhraseGenerator phraseGenerator;
		private PipeConnection connection;

		public Client()
		{
			connection = new PipeConnection(mainPipeName);
			phraseGenerator = new PhraseGenerator();
		}

		public void Start()
		{
			connection.StartConnection();
			string newConnectionName = connection.WaitMessage();
			Reconnect(newConnectionName);
		}

		private void Reconnect(string newConnectionName)
		{
			this.connection.Disconnect();
			this.connection = new PipeConnection(newConnectionName);
			this.connection.StartConnection();
			try
			{

			}
			catch (Exception)
			{
				StartChatting();
			}
			
		}

		private void StartChatting()
		{
			ThreadPool.QueueUserWorkItem(state =>
			{
				while (true)
				{
					Console.WriteLine(this.connection.WaitMessage());
				}
			});

			while (true)
			{
				Random random = new Random();
				int interval = random.Next(10) * 1000;
				Thread.Sleep(interval);

				string message = this.phraseGenerator.GetPhrase();
				this.connection.SendMessage(message);
				Console.WriteLine($"Sending '{message}'");
			}
		}
	}
}
