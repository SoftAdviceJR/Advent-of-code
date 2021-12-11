using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day11
{
	static class Solution
	{
		private static int[,] ReadInput()
		{
			var lines = File.ReadAllLines("Day11/Input.txt");

			int rowCount = lines.Length;
			int colCount = lines.First().Length;

			int[,] octopi = new int[rowCount, colCount];

			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					octopi[i, j] = int.Parse(lines[i][j].ToString());
				}
			}

			return octopi;
		}

		public static decimal Part1()
		{
			var octopi = ReadInput();

			var flasher = new OctopusFlasher(octopi);

			int flashes = 0;

			for (int i = 0; i < 100; i++)
			{
				flasher.Increment();
				flashes += flasher.Flash();
			}

			return flashes;
		}

		public static long Part2()
		{
			var octopi = ReadInput();

			int octopusCount = octopi.GetLength(0) * octopi.GetLength(1);

			var flasher = new OctopusFlasher(octopi);

			int i = 0;
			while(true)
			{
				flasher.Increment();
				int flashes = flasher.Flash();
				i++;
				if (flashes == octopusCount)
					return i;

			}
		}
	}
}
