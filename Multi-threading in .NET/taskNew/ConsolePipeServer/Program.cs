using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace ConsolePipeServer
{
	class Program
	{
		static MessageStorage storage = new MessageStorage(5);
		static List<string> connections = new List<string>();
		const string mainPipeName = "Main_Pipe";

		static void Main(string[] args)
		{
			StartServer();
		}

		private static void StartServer()
		{
			using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream(
				mainPipeName,
				PipeDirection.InOut,
				1,
				PipeTransmissionMode.Message))
			{
				while (true)
				{
					namedPipeServer.WaitForConnection();
					Console.WriteLine("Client Connected");
					string newConnectionName = GenerateConnectionName();
					connections.Add(newConnectionName);
					//string clientName = namedPipeServer.GetImpersonationUserName();
					CreateNewConnectionForClient(newConnectionName);
					SendMessage(namedPipeServer, newConnectionName);
					namedPipeServer.Disconnect();
				}
			}
		}

		private static void CreateNewConnectionForClient(string newConnectionName)
		{
			ThreadPool.QueueUserWorkItem(StartConnection, newConnectionName);
		}

		private static void StartConnection(object state)
		{
			string connectionName = (string)state;
			using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream(
				connectionName,
				PipeDirection.InOut,
				1,
				PipeTransmissionMode.Message))
			{
				namedPipeServer.WaitForConnection();
				Console.WriteLine($"Redirected to {connectionName}");
				SendMessage(namedPipeServer, "First Message");
				while (true)
				{
					string message = ProcessSingleReceivedMessage(namedPipeServer);
					Console.WriteLine(message);
					BroadcastMessage(message);
				}
				
			}
		}

		private static void BroadcastMessage(string message)
		{
			throw new NotImplementedException();
		}

		private static void SendMessage(NamedPipeServerStream namedPipeServer, string message)
		{
			byte[] messageBytes = Encoding.UTF8.GetBytes(message);
			namedPipeServer.Write(messageBytes, 0, messageBytes.Length);
			namedPipeServer.Flush();
		}

		private static string GenerateConnectionName()
		{
			return $"{mainPipeName}{connections.Count + 1}";
		}

		static void ConversationWithTheClient()
		{
			using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream(
				"test-pipe", 
				PipeDirection.InOut,
				1, 
				PipeTransmissionMode.Message))
			{
				Console.WriteLine("Server waiting for a connection...");
				namedPipeServer.WaitForConnection();
				byte[] messageBytes = Encoding.UTF8.GetBytes("Enter your name ");
				namedPipeServer.Write(messageBytes, 0, messageBytes.Length);

				//using (StreamWriter writer = new StreamWriter(namedPipeServer))
				//{
				//	writer.WriteLine("Enter your name ");
				//	writer.Flush();
				//}

				//namedPipeServer.WaitForPipeDrain();

				string clientName = ProcessSingleReceivedMessage(namedPipeServer);
				Console.WriteLine($"[{clientName}] connected");
				foreach (var item in storage.GetMessages())
				{
					string message = $"[{item.ClientName}, {item.Date.ToShortTimeString()}]: {item.Text}";
					messageBytes = Encoding.UTF8.GetBytes(message);
					namedPipeServer.Write(messageBytes, 0, messageBytes.Length);
				}


				while (clientName != "x")
				{
					Console.Write("Send a response from the server: ");
					var message = Console.ReadLine();
					messageBytes = Encoding.UTF8.GetBytes(message);
					namedPipeServer.Write(messageBytes, 0, messageBytes.Length);
					clientName = ProcessSingleReceivedMessage(namedPipeServer);
					Console.WriteLine("The client is saying {0}", clientName);
				}

				Console.WriteLine("The client has ended the conversation.");
			}
		}

		static void SendMessage(PipeStream pipe, string message)
		{
			using (StreamWriter writer = new StreamWriter(pipe))
			{
				writer.WriteLine(message);
				writer.Flush();
			}
		}

		static string RecieveMessage(PipeStream pipe)
		{
			using (StreamReader reader = new StreamReader(pipe))
			{
				return(reader.ReadLine());
			}
		}

		static string ProcessSingleReceivedMessage(NamedPipeServerStream namedPipeServer)
		{
			StringBuilder messageBuilder = new StringBuilder();
			string messageChunk = string.Empty;
			byte[] messageBuffer = new byte[5];
			do
			{
				namedPipeServer.Read(messageBuffer, 0, messageBuffer.Length);
				messageChunk = Encoding.UTF8.GetString(messageBuffer);
				messageBuilder.Append(messageChunk);
				messageBuffer = new byte[messageBuffer.Length];
			}
			while (!namedPipeServer.IsMessageComplete);
			return messageBuilder.ToString();
		}
	}
}
