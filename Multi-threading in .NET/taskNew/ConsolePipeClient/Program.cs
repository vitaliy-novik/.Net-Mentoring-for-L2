using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePipeClient
{
	class Program
	{
		const string mainPipeName = "Main_Pipe";

		static void Main(string[] args)
		{
			ConnectToMainPipe();
		}

		private static void ConnectToMainPipe()
		{
			using (NamedPipeClientStream namedPipeClient = new NamedPipeClientStream(
				".",
				mainPipeName,
				PipeDirection.InOut))
			{
				namedPipeClient.Connect();
				namedPipeClient.ReadMode = PipeTransmissionMode.Message;
				string newConnectionName = ProcessSingleReceivedMessage(namedPipeClient);
				Console.WriteLine($"New connection {newConnectionName}");
				Reconnect(newConnectionName);
			}
		}

		private static void Reconnect(string newConnectionName)
		{
			using (NamedPipeClientStream namedPipeClient = new NamedPipeClientStream(
				".",
				newConnectionName,
				PipeDirection.InOut))
			{
				namedPipeClient.Connect();
				namedPipeClient.ReadMode = PipeTransmissionMode.Message;
				string firstMessage = ProcessSingleReceivedMessage(namedPipeClient);
				Console.WriteLine(firstMessage);
				StartSendingMessages(namedPipeClient);
				StartListening(namedPipeClient);
			}
		}

		private static void StartListening(NamedPipeClientStream namedPipeClient)
		{
			while (true)
			{
				Thread.Sleep(100 * 1000);
			}
		}

		private static void StartSendingMessages(NamedPipeClientStream namedPipeClient)
		{
			ThreadPool.QueueUserWorkItem(state =>
			{
				NamedPipeClientStream stream = (NamedPipeClientStream)state;
				Random randomizer = new Random();
				while (true)
				{
					CancellationTokenSource cts = new CancellationTokenSource();
					Task reader = Task.Run(() => ProcessSingleReceivedMessage(stream), cts.Token);
					reader.Wait(randomizer.Next(10) * 1000);
					string message = randomPhrases[randomizer.Next(19)];
					SendMessage(stream, message);
					Console.WriteLine($"Sending '{message}'");
				}
			}, namedPipeClient);
		}

		private static void SendMessage(PipeStream namedPipeServer, string message)
		{
			byte[] messageBytes = Encoding.UTF8.GetBytes(message);
			namedPipeServer.Write(messageBytes, 0, messageBytes.Length);
			namedPipeServer.Flush();
		}

		static void ConversationWithTheServer()
		{
			using (NamedPipeClientStream namedPipeClient = new NamedPipeClientStream(
				".", 
				"test-pipe", 
				PipeDirection.InOut))
			{
				namedPipeClient.Connect();
				Console.WriteLine("Client connected to the named pipe server. Waiting for server to send the first message...");
				namedPipeClient.ReadMode = PipeTransmissionMode.Message;
				string messageFromServer = ProcessSingleReceivedMessage(namedPipeClient);
				Console.WriteLine("The server is saying {0}", messageFromServer);
				Console.Write("Write a response: ");
				string response = Console.ReadLine();
				byte[] responseBytes = Encoding.UTF8.GetBytes(response);
				namedPipeClient.Write(responseBytes, 0, responseBytes.Length);
				while (response != "x")
				{
					messageFromServer = ProcessSingleReceivedMessage(namedPipeClient);
					Console.WriteLine("The server is saying {0}", messageFromServer);
					Console.Write("Write a response: ");
					response = Console.ReadLine();
					responseBytes = Encoding.UTF8.GetBytes(response);
					namedPipeClient.Write(responseBytes, 0, responseBytes.Length);
				}
			}
		}

		static string ProcessSingleReceivedMessage(NamedPipeClientStream namedPipeClient)
		{
			StringBuilder messageBuilder = new StringBuilder();
			string messageChunk = string.Empty;
			byte[] messageBuffer = new byte[5];
			do
			{
				namedPipeClient.Read(messageBuffer, 0, messageBuffer.Length);
				messageChunk = Encoding.UTF8.GetString(messageBuffer);
				messageBuilder.Append(messageChunk);
				messageBuffer = new byte[messageBuffer.Length];
			}
			while (!namedPipeClient.IsMessageComplete);
			return messageBuilder.ToString();
		}

		static List<string> randomPhrases = new List<string>
		{
			"Like Father Like Son",
			"Drawing a Blank",
			"Dropping Like Flies",
			"Roll With the Punches",
			"Hear, Hear",
			"Tough It Out",
			"Right Off the Bat",
			"High And Dry",
			"Read 'Em and Weep",
			"Keep On Truckin'",
			"Not the Sharpest Tool in the Shed",
			"Cry Over Spilt Milk",
			"Wouldn't Harm a Fly",
			"Hit Below The Belt",
			"Right Out of the Gate",
			"Jig Is Up",
			"Birds of a Feather Flock Together",
			"Money Doesn't Grow On Trees",
			"Quick On the Draw",
			"Long In The Tooth"
		};
	}
}
