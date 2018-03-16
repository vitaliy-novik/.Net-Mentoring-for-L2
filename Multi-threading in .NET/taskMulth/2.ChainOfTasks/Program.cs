using System;
using System.Linq;
using System.Threading.Tasks;

namespace _2.ChainOfTasks
{
	/// <summary>
	/// 2. Write a program, which creates a chain of four Tasks. 
	/// First Task – creates an array of 10 random integer. 
	/// Second Task – multiplies this array with another random integer. 
	/// Third Task – sorts this array by ascending. 
	/// Fourth Task – calculates the average value. 
	/// All this tasks should print the values to console
	/// </summary>
	class Program
	{
		static readonly Random randomizer = new Random();

		static void Main(string[] args)
		{
			Task.Run((Func<int[]>)CreateRandomArray)
				.ContinueWith(task => MultiplyArray(task.Result))
				.ContinueWith(task => SortArrayByAscending(task.Result))
				.ContinueWith(task => CalculateAverage(task.Result));

			Console.ReadKey();
		}

		static int[] CreateRandomArray()
		{
			int[] array = new int[10];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = randomizer.Next(10);
			}

			Console.WriteLine("Generated array");
			PrintArray(array);
			return array;
		}

		static int[] MultiplyArray(int[] array)
		{
			int multiplier = randomizer.Next(10);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] *= multiplier;
			}

			Console.WriteLine($"Array multiplied to {multiplier}");
			PrintArray(array);
			return array;
		}

		static int[] SortArrayByAscending(int[] array)
		{
			Array.Sort(array);
			Console.WriteLine("Sorted array");
			PrintArray(array);

			return array;
		}

		static double CalculateAverage(int[] array)
		{
			double average = array.Average();
			Console.WriteLine($"Average = {average}");

			return average;
		}

		static void PrintArray(int[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Console.WriteLine($"{i} - {array[i]}");
			}
		}
	}
}
