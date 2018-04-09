using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _1.ArrayOfTasks
{
	/// <summary>
	/// 1. Write a program, which creates an array of 100 Tasks, runs them and wait all of them are not finished. 
	/// Each Task should iterate from 1 to 1000 and print into the console the following string: 
	/// “Task #0 – {iteration number}”.
	/// </summary>
	class Program
	{
		static List<int> tasksNumber = new List<int>()
		{
			100,
			500,
			1000
		};

		static void Main(string[] args)
		{
			foreach (int number in tasksNumber)
			{
				Task[] tasks = CreateArrayOfTasks(number); // < 1ms
				RunTasks(tasks);
				Task.WaitAll(tasks);
			}
			// ~8 sec.
			// ~28 sec.
			// ~60 sec.
			Console.ReadKey();
		}

		static Task[] CreateArrayOfTasks(int tasksNumber)
		{
			Task[] tasks = new Task[tasksNumber];
			for (int i = 0; i < tasksNumber; i++)
			{
				tasks[i] = new Task(Iterate, i);
			}

			return tasks;
		}

		static void RunTasks(Task[] tasks)
		{
			foreach (Task task in tasks)
			{
				task.Start();
			}
		}

		static void Iterate(object taskState)
		{
			int taskNumber = (int) taskState;
			for (int i = 1; i <= 1000; i++)
			{
				Console.WriteLine($"Task #{taskNumber} – {i}");
			}
		}
	}
}
