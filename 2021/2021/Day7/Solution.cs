using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day7
{
	static class Solution
	{
		public static decimal Part1()
		{
			var numbers = ReadInput();

			int median = (int)Math.Ceiling(numbers.GetMedian());

			return numbers.Sum(n => Math.Abs(median - n));
		}

		public static long Part2()
		{
			var positions = ReadInput();

			int min = positions.Min();
			int max = positions.Max();

			//upper bound
			long minCost = max * (max - 1) / 2 * positions.Length;

			for (int i = min; i < max; i++)
			{
				long fuelCost = positions.Sum(n =>
					{
						int d = Math.Abs(n - i);
						return d*(d+1)/2;
					});

				minCost = Math.Min(minCost, fuelCost);
			}
			return minCost;
		}

		private static int[] ReadInput()
		{
			return File.ReadAllLines("Day7/Input.txt")[0].Split(",").Select(x => int.Parse(x)).ToArray();
		}


	}
}
