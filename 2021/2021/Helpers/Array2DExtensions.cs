using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Helpers
{
	static class Array2DExtensions
	{
		public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
		{
			return Enumerable.Range(0, matrix.GetLength(0))
					.Select(x => matrix[x, columnNumber])
					.ToArray();
		}

		public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
		{
			return Enumerable.Range(0, matrix.GetLength(1))
					.Select(x => matrix[rowNumber, x])
					.ToArray();
		}

		public static List<T> ToList<T>(this T[,] matrix)
		{
			var list = new List<T>();

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					list.Add(matrix[i, j]);
				}
			}

			return list;
		}

		public static void Print2DArray<T>(this T[,] matrix)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					Console.Write(matrix[i, j] + "\t");
				}
				Console.WriteLine();
			}
		}
	}
}
