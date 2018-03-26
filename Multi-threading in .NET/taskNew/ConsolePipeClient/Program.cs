using ClientPipe;

namespace ConsolePipeClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Client client = new Client();
			client.Start();
		}
	}
}
