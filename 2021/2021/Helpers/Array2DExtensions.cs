﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

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
					Console.Write(matrix[i, j] + " ");
				}
				Console.WriteLine();
			}
		}

		public static void Fill<T>(this T[,] matrix, T value)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] = value;
				}
			}
		}

		public static bool Any<T>(this T[,] matrix, Func<T, bool> predicate)
		{
			return matrix.ToList().Any(predicate);
		}

		public static TResult[,] Select<T, TResult>(this T[,] matrix, Func<T, TResult> predicate)
		{
			var result = new TResult[matrix.GetLength(0), matrix.GetLength(1)];

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					result[i, j] = predicate(matrix[i, j]);
				}
			}

			return result;
		}

		public static TResult[,] Select<T, TResult>(this T[,] matrix, Func<T, int, int, TResult> predicate)
		{
			var result = new TResult[matrix.GetLength(0), matrix.GetLength(1)];

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					result[i, j] = predicate(matrix[i, j], i, j);
				}
			}

			return result;
		}

		public static T[,] ForeachNeighbour<T>(this T[,] matrix, int row, int col, Func<T, T> action)
		{
			if (row > 0)
			{
				matrix[row - 1, col] = action(matrix[row - 1, col]);

				if (col > 0)
					matrix[row - 1, col - 1] = action(matrix[row - 1, col - 1]);
				if (col < matrix.GetLength(1) - 1)
					matrix[row - 1, col + 1] = action(matrix[row - 1, col + 1]);
			}
			if (row < matrix.GetLength(0) - 1)
			{
				matrix[row + 1, col] = action(matrix[row + 1, col]);

				if (col < matrix.GetLength(1) - 1)
					matrix[row + 1, col + 1] = action(matrix[row + 1, col + 1]);
			}
			if (col > 0)
			{
				matrix[row, col - 1] = action(matrix[row, col - 1]);

				if (row < matrix.GetLength(0) - 1)
					matrix[row + 1, col - 1] = action(matrix[row + 1, col - 1]);
			}
			if (col < matrix.GetLength(1) - 1)
			{
				matrix[row, col + 1] = action(matrix[row, col + 1]);
			}

			return matrix;
		}

		public static T[,] ExpandWith<T>(this T[,] original, T outside)
		{
			var newImage = new T[original.GetLength(0) + 2 , original.GetLength(1) + 2];
			newImage.Fill(outside);
			newImage.FillWithAt(original, 1, 1);

			return newImage;
		}

		public static void FillWithAt<T>(this T[,] target, T[,] matrix, int row, int col)
		{
			for (int i = row; i < matrix.GetLength(0) + row; i++)
			{
				for (int j = col; j < matrix.GetLength(1) + col; j++)
				{
					target[i, j] = matrix[i - row, j - col];
				}
			}
		}

		public static T[,] GetRange<T>(this T[,] matrix, int row,  int col, int rowCount, int colCount)
		{
			var result = new T[rowCount, colCount];

			for (int i = row; i < row + rowCount; i++)
			{
				for (int j = col; j < col + colCount; j++)
				{
					result[i - row, j - col] = matrix[i, j];
				}
			}

			return result;
		}
	}
}
