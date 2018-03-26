using System;
using System.Threading;
using System.Threading.Tasks;

namespace _1.AsyncSum
{
	class Program
	{
		static void Main(string[] args)
		{
			int n;
			string input = string.Empty;
			CancellationTokenSource cts = null;
			while (input != "ex")
			{
				input = Console.ReadLine();
				if (cts != null)
				{
					cts.Cancel();
				}

				cts = new CancellationTokenSource();
				int.TryParse(input, out n);
				Task.Run(() => Sum(n, cts.Token), cts.Token);
			}
		}

		static void Sum(int n, CancellationToken token)
		{
			long sum = 0;
			for (int i = 1; i <= n; i++)
			{
				if (token.IsCancellationRequested)
				{
					return;
				}

				sum += i;
			}

			Console.WriteLine($"Sum({n}) = {sum}");
		}
	}
}
