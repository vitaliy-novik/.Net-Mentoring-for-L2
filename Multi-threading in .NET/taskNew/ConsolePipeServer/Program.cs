using ServerPipe;

namespace ConsolePipeServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Server server = new Server();
			server.Start();
		}
	}
}
