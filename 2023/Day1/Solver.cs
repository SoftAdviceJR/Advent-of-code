using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day1
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day1\\{DateTime.Now:yyMMdd}.log");
				//builder.ForLogger().FilterMinLevel(LogLevel.Trace).WriteToConsole();
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static string[] ReadInput()
		{
			return File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day1\\input.txt");
		}

		public string SolvePart1()
		{
			var input = ReadInput();

			int result = 0;

			foreach (var line in input)
			{
				result += CreateTwoDigitNumber(line);
			}

			return result.ToString();
		}

		private static int CreateTwoDigitNumber(string line)
		{
			if (line.Length == 0)
				return 0;

			char? first = null;
			char? last = null;

			foreach (var c in line)
			{
				if (char.IsDigit(c))
				{
					if (first == null)
						first = c;
					last = c;
				}
			}

			if (!first.HasValue || !last.HasValue)
				return 0;

			string stringValue = new string(new char[] { first.Value, last.Value });

			return int.Parse(stringValue);
		}

		public string SolvePart2()
		{
			var input = ReadInput();

			int result = 0;

			foreach (var line in input)
			{
				int lineNumber = CreateTwoDigitNumberWithWrittenDigits(line);

				result += lineNumber;

				logger.Debug($"{lineNumber}");
			}

			return result.ToString();
		}

		private static int CreateTwoDigitNumberWithWrittenDigits(string line)
		{
			if (line.Length == 0)
				return 0;

			int firstDigit = 0;
			int lastDigit = 0;

			Dictionary<int, string[]> digitStrings = new Dictionary<int, string[]>()
			{
				{1, new[]{ "1", "one" } },
				{2, new[]{ "2", "two" } },
				{3, new[]{ "3", "three" } },
				{4, new[]{ "4", "four" } },
				{5, new[]{ "5", "five" } },
				{6, new[]{ "6", "six" } },
				{7, new[]{ "7", "seven" } },
				{8, new[]{ "8", "eight" } },
				{9, new[]{ "9", "nine" } },
			};

			Dictionary<int, int[]> digitIndices = new Dictionary<int, int[]>();

			foreach (var digit in digitStrings)
			{
				int firstIndex = digit.Value
					.Select(subString =>
					{
						var index = line.IndexOf(subString);
						return index >= 0 ? index : line.Length + 1;
					}
					).Min();

				int lastIndex = digit.Value
					.Select(subString =>
					{
						return line.LastIndexOf(subString);
					}
					).Max();

				digitIndices.Add(digit.Key, new[] { firstIndex, lastIndex });
			}

			firstDigit = digitIndices.MinBy(d => d.Value[0]).Key;
			lastDigit = digitIndices.MaxBy(d => d.Value[1]).Key;

			return 10 * firstDigit + lastDigit;
		}

	}

	static class Day1Extensions
	{

	}
}
