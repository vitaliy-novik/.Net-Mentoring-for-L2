using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace ConsolePipeServer
{
	class Program
	{
		static MessageStorage storage = new MessageStorage(5);
		static void Main(string[] args)
		{
			ConversationWithTheClient();
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
