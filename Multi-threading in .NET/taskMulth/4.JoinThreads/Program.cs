using System;
using System.Collections.Generic;
using System.Threading;

namespace _4.JoinThreads
{
	/// <summary>
	/// 4. Write a program which recursively creates 10 threads.
	/// Each thread should be with the same body and receive a state with integer number,
	/// decrement it, print and pass as a state into the newly created thread.
	/// Use Thread class for this task and Join for waiting threads.
	/// </summary>
	class Program
	{
		const int threadsNumber = 10;
		static Semaphore semaphore = new Semaphore(0, 1);
		static List<int> list = new List<int>();

		static void Main(string[] args)
		{
			RunRecusiveThreadsWithJoin(threadsNumber);

			RunRecursiveThreadsWithSemaphore(threadsNumber);

			Console.ReadKey();
		}

		static void RunRecusiveThreadsWithJoin(object threadState)
		{
			int counter = (int) threadState;
			Console.WriteLine(--counter);
			if (counter > 0)
			{
				Thread thread = new Thread(RunRecusiveThreadsWithJoin);
				thread.Start(counter);
				thread.Join();
			}
		}

		static void RunRecursiveThreadsWithSemaphore(object threadState)
		{
			int counter = (int)threadState;
			Console.WriteLine(--counter);
			if (counter > 0)
			{
				ThreadPool.QueueUserWorkItem(RunRecursiveThreadsWithSemaphore, counter);
			} else
			{
				semaphore.Release();
			}

			semaphore.WaitOne();
		}
	}
}
