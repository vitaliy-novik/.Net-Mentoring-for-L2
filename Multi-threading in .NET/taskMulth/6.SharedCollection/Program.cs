using System;
using System.Collections.Generic;
using System.Threading;

namespace _6.SharedCollection
{
	/// <summary>
	/// 6. Write a program which creates two threads and a shared collection: 
	/// the first one should add 10 elements into the collection 
	/// and the second should print all elements in the collection after each adding. 
	/// Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
	/// </summary>
	class Program
	{
		const int iterationsNumber = 10;
		static AutoResetEvent writedEvent = new AutoResetEvent(false);
		static AutoResetEvent readedEvent = new AutoResetEvent(true);
		static List<int> collection = new List<int>();

		static void Main(string[] args)
		{
			Thread writer = new Thread(WriteToCollection);
			Thread reader = new Thread(ReadCollection);
			writer.Start();
			reader.Start();

			Console.Read();
		}

		static void WriteToCollection()
		{
			Random randomizer = new Random();
			for (int i = 0; i < iterationsNumber; i++)
			{
				readedEvent.WaitOne();
				collection.Add(randomizer.Next(10));
				writedEvent.Set();
			}
		}

		static void ReadCollection()
		{
			for (int i = 0; i < iterationsNumber; i++)
			{
				writedEvent.WaitOne();
				Console.WriteLine("~~~~~~~~~~~");
				foreach (int item in collection)
				{
					Console.WriteLine(item);
				}
				readedEvent.Set();
			}
		}
	}
}
