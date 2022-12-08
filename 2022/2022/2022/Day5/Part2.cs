using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2022.Day5
{
	static class Part2
	{
		public static string[] ReadArrangement(string[] input)
		{

			int stackCount = (input[0].Length + 1) / 4;

			string[] results = new string[stackCount];

			foreach (var line in input)
			{
				if (char.IsDigit(line[1]))
					return results;

				for (int i = 0; i < stackCount; i++)
				{
					int charIndex = i * 4 + 1;

					var letter = line[charIndex];

					if (char.IsWhiteSpace(letter))
						continue;

					if (char.IsLetter(letter))
					{
						results[i] += letter;
					}
				}
			}

			throw new InvalidOperationException($"Never found the bottom of the stacks.");
		}

		public static int[,] ReadInstructions(string[] input)
		{
			string[] orders = new string[0];

			for (int i = 0; i < input.Length; i++)
			{
				if (string.IsNullOrWhiteSpace(input[i]))
					orders = input.Skip(i + 1).ToArray();
			}

			if (orders.Length == 0)
				throw new InvalidOperationException($"Could not find start of instructions.");

			var results = new int[orders.Length, 3];

			var regex = new Regex(@"move (\d*) from (\d*) to (\d*)");

			for (int i = 0; i < orders.Length; i++)
			{
				var match = regex.Match(orders[i]);

				if (match.Groups.Count != 4)
					throw new InvalidOperationException($"Unexpected instruction format: {orders[i]}");

				results[i, 0] = int.Parse(match.Groups[1].Value);
				results[i, 1] = int.Parse(match.Groups[2].Value) - 1;
				results[i, 2] = int.Parse(match.Groups[3].Value) - 1;
			}

			return results;
		}

		public static string Solve()
		{
			var input = File.ReadAllLines("Day5/input.txt");

			var arrangement = ReadArrangement(input);

			var orders = ReadInstructions(input);

			foreach (var order in orders.ToJaggedArray())
			{
				Execute(arrangement, order);
			}

			return new string(arrangement.Select(stack => stack[0]).ToArray());
		}

		private static void Execute(string[] arrangement, int[] order)
		{
			int length = order[0];
			int source = order[1];
			int target = order[2];

			string block = arrangement[source].Substring(0, length);
			arrangement[target] = block + arrangement[target];
			arrangement[source] = arrangement[source].Substring(length, arrangement[source].Length - length);

		}
	}
}
