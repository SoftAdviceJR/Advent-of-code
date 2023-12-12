using System;
using System.Text;

namespace _2023
{
	public static class ArrayExtensions
	{
		public static string Print<T>(this T[,] matrix)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					sb.Append(matrix[i, j]);
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		public static T[,] To2DArray<T>(this List<List<T>> input)
		{
			T[,] matrix = new T[input.Count, input[0].Count];

			for (int i = 0; i < input.Count; i++)
			{
				for (int j = 0; j < input[i].Count; j++)
				{
					matrix[i, j] = input[i][j];
				}
			}

			return matrix;
		}

		public static T[,] To2DArray<T>(this T[][] input)
		{
			T[,] matrix = new T[input.Length, input[0].Length];

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[i].Length; j++)
				{
					matrix[i, j] = input[i][j];
				}
			}

			return matrix;
		}
	}
}