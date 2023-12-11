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
	}
}