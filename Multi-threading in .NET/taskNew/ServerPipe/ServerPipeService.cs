using SharedPipeLibrary;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPipe
{
	internal class ServerPipeService
	{
		public ServerPipeService(NamedPipeServerStream namedPipeServer) { }

		public void WaitForConnection()
		{
		}

	}
}
