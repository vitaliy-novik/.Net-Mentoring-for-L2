using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerPipe
{
	public class Server
	{
		private PipeConnection mainPipeConnection;
		private List<PipeConnection> connections;
		private const string mainPipeName = "MainPipe";

		public Server(string connectionName)
		{
			connections = new List<PipeConnection>();
		}

		public void Start()
		{

		}

		private string GenerateConnectionName()
		{
			return $"{mainPipeName}{connections.Count + 1}";
		}
	}
}
