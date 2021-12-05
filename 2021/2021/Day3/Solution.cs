using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day3
{
	static class Solution
	{
		public static long Part1()
		{
			string gamma = GetGamma();
			Console.WriteLine("Gamma: " + gamma);
			string epsilon = GetEpsilon(gamma);
			Console.WriteLine("Epsilon: " + epsilon);



			return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
		}

		public static long Part2()
		{
			return GetLifeSupport();
		}


		private static string GetGamma()
		{
			var lines = File.ReadAllLines("Day3/Input.txt");

			var count = GetBitCount(lines);

			return string.Concat(count.Select(x =>
			{
				if (x >= lines.Length / 2)
					return '1';
				return '0';
			}));

		}

		private static string GetEpsilon(string gamma)
		{
			return String.Concat(gamma.Select(x =>
			{
				if (x == '1')
					return '0';
				return '1';
			}));
		}

		private static long GetLifeSupport()
		{
			var lines = File.ReadAllLines("Day3/Input.txt");

			return Convert.ToInt32(FilterO2(lines), 2) * Convert.ToInt32(FilterCO2(lines), 2);
		}

		private static string FilterO2(IEnumerable<string> lines, int pos = 0)
		{
			var count = GetBitCount(lines);

			char filterValue = count[pos] >= lines.Count() / 2f ? '1' : '0';

			var filteredLines = lines.Where(line => line[pos] == filterValue);

			if (filteredLines.Count() == 1)
				return filteredLines.First();
			return FilterO2(filteredLines, pos + 1);
		}

		private static string FilterCO2(IEnumerable<string> lines, int pos = 0)
		{
			var count = GetBitCount(lines);

			char filterValue = count[pos] < lines.Count() / 2f ? '1' : '0';

			var filteredLines = lines.Where(line => line[pos] == filterValue);

			if (filteredLines.Count() == 1)
				return filteredLines.First();

			return FilterCO2(filteredLines, pos + 1);
		}

		private static int[] GetBitCount(IEnumerable<string> lines)
		{
			int[] count = new int[lines.First().Length];


			foreach (var line in lines)
			{
				for (int i = 0; i < line.Length; i++)
				{
					if (line[i] == '1')
						count[i]++;
				}
			}

			return count;
		}
	}
}
