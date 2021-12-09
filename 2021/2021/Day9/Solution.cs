using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day9
{
	static class Solution
	{
		private static int[,] ReadInput()
		{
			var lines = File.ReadAllLines("Day9/Input.txt");

			var coords = new int[lines.Length + 2, lines[0].Length + 2];

			coords.Fill(9);

			for (int i = 1; i < coords.GetLength(0) - 1; i++)
			{
				for (int j = 1; j < coords.GetLength(1) - 1; j++)
				{
					coords[i, j] = int.Parse(lines[i - 1][j - 1].ToString());
				}
			}

			return coords;
		}

		public static decimal Part1()
		{
			var coords = ReadInput();

			List<int> riskPoints = new List<int>();

			for (int i = 1; i < coords.GetLength(0) - 1; i++)
			{
				for (int j = 1; j < coords.GetLength(1) - 1; j++)
				{
					int pos = coords[i, j];
					if (pos < coords[i - 1, j] &&
						pos < coords[i + 1, j] &&
						pos < coords[i, j - 1] &&
						pos < coords[i, j + 1])
					{
						riskPoints.Add(pos);
					}
				}
			}

			return riskPoints.Sum() + riskPoints.Count;
		}

		public static long Part2()
		{
			var coords = ReadInput();

			List<int> basins = new List<int>();

			for (int i = 1; i < coords.GetLength(0) - 1; i++)
			{
				for (int j = 1; j < coords.GetLength(1) - 1; j++)
				{
					int size = FindBasinSize(ref coords, i, j);
					if (size > 0)
						basins.Add(size);
				}
			}

			basins = basins.OrderByDescending(i => i).ToList();

			return basins[0] * basins[1] * basins[2];
		}

		public static int FindBasinSize(ref int[,] coords, int row, int col)
		{
			int size = 0;

			if (coords[row, col] == 9)
				return size;

			coords[row, col] = 9;
			size++;

			size += FindBasinSize(ref coords, row - 1, col);
			size += FindBasinSize(ref coords, row + 1, col);
			size += FindBasinSize(ref coords, row, col - 1);
			size += FindBasinSize(ref coords, row, col + 1);

			return size;

		}




	}
}
