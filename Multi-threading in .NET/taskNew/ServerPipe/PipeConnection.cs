using SharedPipeLibrary;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPipe
{
	internal class PipeConnection
	{
		private string connectionName;
		private StreamString streamString;
		private NamedPipeServerStream namedPipeServerStream;

		public PipeConnection(string connectionName)
		{
			this.connectionName = connectionName;
			this.namedPipeServerStream = new NamedPipeServerStream(
				connectionName,
				PipeDirection.InOut,
				1,
				PipeTransmissionMode.Message);
			this.streamString = new StreamString(namedPipeServerStream);
		}

		public void StartConnection()
		{
			namedPipeServerStream.WaitForConnection();
		}
	}
}
