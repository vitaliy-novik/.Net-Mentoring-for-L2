using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _3.ParallelMatricesMultiplication
{
	/// <summary>
	/// 3. Write a program, which multiplies two matrices and uses class Parallel. 
	/// </summary>
	class Program
	{
		static Random randomizer = new Random();

		static void Main(string[] args)
		{
			int[,] matA = GenerateMatrix(800, 800);
			int[,] matB = GenerateMatrix(800, 800);
			int[,] res;

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			res = MultiplySequentially(matA, matB);
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);

			stopwatch.Restart();
			res = MultiplyParallel(matA, matB);
			stopwatch.Stop();
			Console.WriteLine(stopwatch.ElapsedMilliseconds);

			Console.Read();
		}

		static int[,] MultiplyParallel(int[,] matA, int[,] matB)
		{
			int matARows = matA.GetLength(0);
			int matACols = matA.GetLength(1);
			int matBCols = matB.GetLength(1);

			int[,] resultMat = new int[matARows, matBCols];
			Parallel.For(0, matARows, i =>
			{
				for (int j = 0; j < matBCols; j++)
				{
					int sum = 0;
					for (int k = 0; k < matACols; k++)
					{
						sum += matA[i, k] * matB[k, j];
					}
					resultMat[i, j] = sum;
				}
			});

			return resultMat;
		}

		static int[,] MultiplySequentially(int[,] matA, int[,] matB)
		{
			int matARows = matA.GetLength(0);
			int matACols = matA.GetLength(1);
			int matBCols = matB.GetLength(1);

			int[,] resultMat = new int[matARows, matBCols];
			for(int i = 0; i < matARows; i++)
			{
				for (int j = 0; j < matBCols; j++)
				{
					int sum = 0;
					for (int k = 0; k < matACols; k++)
					{
						sum += matA[i, k] * matB[k, j];
					}
					resultMat[i, j] = sum;
				}
			};

			return resultMat;
		}

		static int[,] GenerateMatrix(int rows, int cols)
		{
			int[,] matrix = new int[rows, cols];
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					matrix[i, j] = randomizer.Next(100);
				}
			}
			return matrix;
		}
	}
}
