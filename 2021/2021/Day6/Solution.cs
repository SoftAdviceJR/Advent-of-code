using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day6
{
	static class Solution
	{
		public static long Part1()
		{
			var fishes = ReadInput();

			for (int i = 0; i < 80; i++)
			{
				var fishesCopy = JsonConvert.DeserializeObject<Dictionary<int, long>>(JsonConvert.SerializeObject(fishes));

				for (int j = 0; j < fishes.Count - 1; j++)
				{
					fishes[j] = fishesCopy[j + 1];
				}

				fishes[6] += fishesCopy[0];
				fishes[8] = fishesCopy[0];
			}

			return fishes.Sum(x => x.Value);
		}

		public static long Part2()
		{
			var fishes = ReadInput();

			for (int i = 0; i < 256; i++)
			{
				var fishesCopy = JsonConvert.DeserializeObject<Dictionary<int, long>>(JsonConvert.SerializeObject(fishes));

				for (int j = 0; j < fishes.Count - 1; j++)
				{
					fishes[j] = fishesCopy[j + 1];
				}

				fishes[6] += fishesCopy[0];
				fishes[8] = fishesCopy[0];
			}

			return fishes.Sum(x => x.Value);
		}

		private static Dictionary<int, long> ReadInput()
		{
			var lines = File.ReadAllLines("Day6/Input.txt");
			var numbers = lines.First().Split(",").Select(x => int.Parse(x));

			var fishes = new Dictionary<int, long>();

			for (int i = 0; i < 9; i++)
			{
				fishes.Add(i, numbers.Count(x => x == i));
			}

			return fishes;
		}
	}
}
