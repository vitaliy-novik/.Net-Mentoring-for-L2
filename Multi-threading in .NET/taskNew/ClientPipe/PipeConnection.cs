using SharedPipeLibrary;
using System.IO.Pipes;

namespace ClientPipe
{
	class PipeConnection
	{
		protected string connectionName;
		protected StreamString streamString;
		protected NamedPipeClientStream namedPipeClientStream;

		public PipeConnection(string connectionName)
		{
			this.connectionName = connectionName;
			this.namedPipeClientStream = new NamedPipeClientStream(
				".",
				connectionName,
				PipeDirection.InOut,
				PipeOptions.Asynchronous);
			
			this.streamString = new StreamString(namedPipeClientStream);
		}

		public void StartConnection()
		{
			namedPipeClientStream.Connect();
			this.namedPipeClientStream.ReadMode = PipeTransmissionMode.Message;
		}

		public void SendMessage(string message)
		{
			streamString.WriteString(message);
		}

		internal void Disconnect()
		{
			namedPipeClientStream.Close();
		}

		internal string WaitMessage()
		{
			return streamString.ReadString();
		}
	}
}
