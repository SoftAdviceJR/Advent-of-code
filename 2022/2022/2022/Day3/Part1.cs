using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day3
{
	static class Part1
	{
		public static string[][] Read()
		{
			List<string[]> results = new List<string[]>();

			var sacks = File.ReadAllLines("Day3/input.txt");

			foreach (var sack in sacks)
			{
				if (sack.Length % 2 != 0)
					throw new InvalidOperationException($"Length of {sack} is not even");

				int compSize = sack.Length / 2;

				string comp1 = sack.Substring(0, compSize);
				string comp2 = sack.Substring(compSize, compSize);

				results.Add(new string[] { comp1, comp2 });
			}


			return results.ToArray();
		}

		public static int Solve()
		{
			var input = Read();

			int[] results = new int[input.Length];

			for (int i = 0; i < input.Length; i++)
			{
				var sack = input[i];

				char duplicate = FindDuplicate(sack);

				results[i] = ParsePriority(duplicate);
			}
			

			return results.Sum();
		}

		public static char FindDuplicate(string[] sack)
		{
			foreach (char letter in sack[0])
			{
				if (sack[1].Contains(letter))
					return letter;
			}

			throw new InvalidOperationException($"Could not find a duplicate in {sack[0]} and {sack[1]}");
		}

		public static int ParsePriority(char letter)
		{
			if(char.IsLower(letter))
			{
				return (int)letter - 96;
			}
			return (int)letter - 38;
		}

	}
		
}
