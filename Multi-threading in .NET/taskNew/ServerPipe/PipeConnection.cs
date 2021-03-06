﻿using SharedPipeLibrary;
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
		protected string connectionName;
		protected StreamString streamString;
		protected NamedPipeServerStream namedPipeServerStream;

		public PipeConnection(string connectionName)
		{
			this.connectionName = connectionName;
			this.namedPipeServerStream = new NamedPipeServerStream(
				connectionName,
				PipeDirection.InOut,
				1,
				PipeTransmissionMode.Message,
				PipeOptions.Asynchronous);
			this.streamString = new StreamString(namedPipeServerStream);
		}

		public void StartConnection()
		{
			namedPipeServerStream.WaitForConnection();
		}

		public void SendMessage(string message)
		{
			streamString.WriteString(message);
		}

		internal void Disconnect()
		{
			namedPipeServerStream.Disconnect();
		}

		internal string WaitMessage()
		{
			return streamString.ReadString();
		}

		internal string GetClientName()
		{
			return namedPipeServerStream.GetImpersonationUserName();
		}
	}
}
