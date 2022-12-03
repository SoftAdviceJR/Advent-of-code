using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day3
{
	static class Part2
	{
		public static string[][] Read()
		{
			List<string[]> results = new List<string[]>();

			var sacks = File.ReadAllLines("Day3/input.txt");

			for (int i = 0; i < sacks.Length; i += 3)
			{
				results.Add(new string[] { sacks[i], sacks[i + 1], sacks[i + 2] });
			}


			return results.ToArray();
		}

		public static int Solve()
		{
			var groups = Read();

			int[] results = new int[groups.Length];

			for (int i = 0; i < groups.Length; i++)
			{
				var group = groups[i];

				char duplicate = FindDuplicate(group);

				results[i] = ParsePriority(duplicate);
			}


			return results.Sum();
		}

		public static char FindDuplicate(string[] group)
		{
			foreach (char letter in group[0])
			{
				if (group[1].Contains(letter) && group[2].Contains(letter))
					return letter;
			}

			throw new InvalidOperationException($"Could not find a duplicate in {group[0]}, {group[1]} and {group[3]}");
		}

		public static int ParsePriority(char letter)
		{
			if (char.IsLower(letter))
			{
				return (int)letter - 96;
			}
			return (int)letter - 38;
		}

	}

}
