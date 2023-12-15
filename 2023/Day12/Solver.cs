using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NLog;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _2023.Day12
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day12\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{

		}

		private static DataEntry[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day12\\input.txt");

			return lines.Select(r => new DataEntry(r)).ToArray();

		}

		public string SolvePart1()
		{
			var entries = ReadInput();

			long result = 0;

			for (int i = 0; i < entries.Length; i++)
			{
				result += RunCalculator.CountPossibilities(entries[i]);
			}

			return result.ToString();
		}

		public string SolvePart2()
		{
			var entries = ReadInput();

			long result = 0;

			for (int i = 0; i < entries.Length; i++)
			{
				var entry = entries[i];

				entry.Expand();

				long count = RunCalculator.CountPossibilities(entry);

                Console.WriteLine($"{entry}\r\n-> {count}");

				result += count;
            }

            Console.WriteLine();

            return result.ToString();
		}



	}

	public class DataEntry
	{
		public int[] GroupSizes;

		public string Arrangement;

		public DataEntry(string line)
		{
			var parts = line.Split(' ');

			Arrangement = parts[0];

			GroupSizes = parts[1].Split(',').Select(int.Parse).ToArray();
		}

		public DataEntry(string arrangment, int[] groupSizes)
		{
			GroupSizes = groupSizes;
			Arrangement = arrangment;
		}

		public void Expand()
		{
			int factor = 5;

			string newArrangment = "";

			for (int i = 0; i < factor; i++)
			{
				newArrangment += Arrangement + "?";
			}

			Arrangement = newArrangment.Substring(0, newArrangment.Length - 1);

			var newRuns = new int[GroupSizes.Length * factor];

			for (int i = 0; i < newRuns.Length; i++)
			{
				newRuns[i] = GroupSizes[i % GroupSizes.Length];
			}

			GroupSizes = newRuns;
		}

		public override bool Equals(object? obj)
		{
			var typedObj = obj as DataEntry;

			if (typedObj == null) return false;

			return this.Arrangement == typedObj.Arrangement && Enumerable.SequenceEqual(this.GroupSizes, typedObj.GroupSizes);
		}

		public override int GetHashCode()
		{
			int hc = GroupSizes.Length;
			foreach (int val in GroupSizes)
			{
				hc = unchecked(hc * 314159 + val);
			}

			return HashCode.Combine(Arrangement.GetHashCode(), hc);
		}

		public override string ToString()
		{
			return $"{Arrangement} [{string.Join(", ", GroupSizes)}]";
		}
	}

	public static class RunCalculator
	{
		private static Dictionary<DataEntry, long> _cache = new Dictionary<DataEntry, long>();

		private static long Count(DataEntry run)
		{
			if (run.GroupSizes.Length == 0)
				return run.Arrangement.Contains('#') ? 0 : 1;

			if (string.IsNullOrWhiteSpace(run.Arrangement))
				return 0;

			var nextCharacter = run.Arrangement[0];
			var nextGroupSize = run.GroupSizes[0];

			if (nextCharacter == '.')
				return Dot();

			if (nextCharacter == '#')
				return Pound();

			if (nextCharacter == '?')
				return Dot() + Pound();

			throw new InvalidOperationException($"Unkown charachter {nextCharacter}");

			long Dot()
			{
				return CountPossibilities(new DataEntry(run.Arrangement.Substring(1), run.GroupSizes));
			}

			long Pound()
			{
				var group = run.Arrangement.Substring(0, Math.Min(run.Arrangement.Length, nextGroupSize));

				group = group.Replace("?", "#");

				if (group != new string('#', nextGroupSize))
					return 0;

				if (run.Arrangement.Length == nextGroupSize)
				{
					if (run.GroupSizes.Length == 1)
						return 1;
					return 0;
				}

				if ("?.".Contains(run.Arrangement[nextGroupSize]))
					return CountPossibilities(new DataEntry(run.Arrangement.Substring(nextGroupSize + 1), run.GroupSizes.Skip(1).ToArray()));

				return 0;
			}
		}

		public static long CountPossibilities(DataEntry entry)
		{
			if (_cache.TryGetValue(entry, out long value))
			{
                //Console.WriteLine($"Read from cache:\t {entry} => {value}");
                return value;
			}

			value = Count(entry);

			_cache.Add(entry, value);

			//Console.WriteLine($"Calculated:\t\t {entry} => {value}");

			return value;
		}
	}

	public static class Day12Extensions
	{
		public static char ConvertToChar(this char c)
		{
			if (c == '0')
				return '.';

			if (c == '1')
				return '#';

			throw new InvalidOperationException($"Cannot convert {c}");
		}
	}
}
