using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day1
{
	static class Part2
	{
		public static int[][] Read()
		{
			List<int[]> results = new List<int[]>();

			var input = File.ReadAllLines("Day1/input.txt");

			List<int> items = new List<int>();

			foreach (var line in input)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					results.Add(items.ToArray());
					items.RemoveAll(d => true);
					continue;
				}
				items.Add(int.Parse(line));
			}


			return results.ToArray();
		}

		public static int Solve()
		{
			var input = Read();


			var sums = input.Select(items => items.Sum());

			var ordered = sums.OrderByDescending(s => s);
			var top3 = ordered.Take(3);

			return top3.Sum();
		}

		
	}
}
