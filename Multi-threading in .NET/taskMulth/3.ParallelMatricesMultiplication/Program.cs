using System;
using System.Threading.Tasks;

namespace _3.ParallelMatricesMultiplication
{
	class Program
	{
		static void Main(string[] args)
		{

		}

		static int[,] MultiplyParallel(int[,] matA, int[,] matB)
		{
			int matARows = matA.GetLength(0);
			int matACols = matA.GetLength(1);
			int matBCols = matB.GetLength(1);

			int[,] result = new int[matARows, matBCols];

			Parallel.For(0, matARows, i =>
			{
				for (int j = 0; j < matBCols; j++)
				{
					int sum = 0;
					for (int k = 0; k < matACols; k++)
					{
						sum += matA[i, k] * matB[k, j];
					}
					result[i, j] = sum;
				}
			});

			return result;
		}

		private static int[,] InitializeMatrix(int rows, int cols)
		{
			int[,] matrix = new int[rows, cols];
			Random randomizer = new Random();
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
