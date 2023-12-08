using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using _2023.Day3;

using NLog;

namespace _2023.Day6
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day6\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day6\\input.txt");

			var Times = CleanAndParse(lines[0]);
			var Records = CleanAndParse(lines[1]);

			Races = new Race[Times.Length];

			for (int i = 0; i < Times.Length; i++)
			{
				Races[i] = new Race(Times[i], Records[i]);
			}

			var Time = long.Parse(lines[0].Split(':')[1].Trim().Replace(" ", ""));
			var Record = long.Parse(lines[1].Split(':')[1].Trim().Replace(" ", ""));

			Race = new Race(Time, Record);
		}

		private static int[] CleanAndParse(string line)
		{
			var line1 = line.Split(':')[1].Trim().Replace("  ", " ");

			return line1.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
		}

		private Race[] Races { get; }

		private Race Race { get; }


		public string SolvePart1()
		{
			var options = new List<long>();


			for (int i = 0; i < Races.Length; i++)
			{
				var race = Races[i];
				
				options.Add(race.WaysToWin());
			}

			long result = 1;

			foreach (var option in options)
			{
				result *= option;
			}

			return result.ToString();
		}


		public string SolvePart2()
		{
			return Race.WaysToWin().ToString();
		}



	}

	public class Race
	{
		public long Time { get; }
		public long Record { get; }

		public Race(long time, long record)
		{
			Time = time;
			Record = record;
		}

		public long WaysToWin()
		{
			long minTime = Time;

			for (int j = 1; j < minTime; j++)
			{
				if (j * (Time - j) > Record)
				{
					minTime = j;
				}
			}

			return Time + 1 - 2 * minTime;
		}
	}
	
}
