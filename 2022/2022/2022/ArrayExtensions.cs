using System;
using System.Linq;

namespace _2022
{
	public static class ArrayExtensions
	{
		public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
		{
			return Enumerable.Range(0, matrix.GetLength(0))
					.Select(x => matrix[x, columnNumber])
					.ToArray();
		}

		internal static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
		{
			int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
			int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
			int numberOfRows = rowsLastIndex + 1;

			int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
			int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
			int numberOfColumns = columnsLastIndex + 1;

			T[][] jaggedArray = new T[numberOfRows][];
			for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
			{
				jaggedArray[i] = new T[numberOfColumns];

				for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
				{
					jaggedArray[i][j] = twoDimensionalArray[i, j];
				}
			}
			return jaggedArray;
		}

		public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
		{
			return Enumerable.Range(0, matrix.GetLength(1))
					.Select(x => matrix[rowNumber, x])
					.ToArray();
		}

		public static void FillArray<T>(this T[,] matrix, T value)
{
			for (int i = 0; i < matrix.GetLength(0); i++)
{
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					matrix[i, j] = value;
				}
			}
		}
	}

}
