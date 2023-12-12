using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day11
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day11\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static Universe ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day11\\input.txt");



			return new Universe(lines);

		}

		public string SolvePart1()
		{
			var universe = ReadInput();

			Coordinate[] stars = universe.GetStars();

			Dictionary<string, long> results = new Dictionary<string, long>();

			for (int i = 0; i < stars.Length - 1; i++)
			{
				for (int j = i + 1; j < stars.Length; j++)
				{
					results.Add($"{i+1} -> {j+1}",universe.ExpandedDistance(stars[i], stars[j], 1));
				}
			}

			return results.Values.Sum().ToString();

		}


		public string SolvePart2()
		{
			var universe = ReadInput();

			Coordinate[] stars = universe.GetStars();

			Dictionary<string, long> results = new Dictionary<string, long>();

			for (int i = 0; i < stars.Length - 1; i++)
			{
				for (int j = i + 1; j < stars.Length; j++)
				{
					results.Add($"{i + 1} -> {j + 1}", universe.ExpandedDistance(stars[i], stars[j], 1000000 - 1));
				}
			}

			return results.Values.Sum().ToString();
		}



	}


	public class Universe
	{
		private string[] universe;

		int[] emptyRows;
		int[] emptyColumns;


		public Universe(string[] lines)
		{

			List<int> emptyRows = new List<int>();
			List<int> emptyColumns = new List<int>();

			universe = lines;

			for (int i = 0; i < universe.Length; i++)
			{
				if (universe[i].All(spot => spot == '.'))
					emptyRows.Add(i);
			}

			this.emptyRows = emptyRows.ToArray();

			for (int i = 0; i < universe[0].Length; i++)
			{
				if(universe.All(row => row[i] == '.'))
					emptyColumns.Add(i);
			}

			this.emptyColumns = emptyColumns.ToArray();
		}


		public long ExpandedDistance(Coordinate start, Coordinate end, int expansionFactor)
		{
			var rowRange = new Range(start.Row, end.Row);
			var columnRange = new Range(start.Column, end.Column);

			var crossedEmptyRows = this.emptyRows.Where(i => rowRange.Contains(i)).ToArray();
			var crossedEmptyColumns = this.emptyColumns.Where(i => columnRange.Contains(i)).ToArray();

			long result = start.ManhattanDistance(end);

			result += (crossedEmptyColumns.Length + crossedEmptyRows.Length) * expansionFactor;

			return result;
		}

		public Universe Expand()
		{
			var expandedList = new List<string>();


			for (int i = 0; i < universe.Length; i++)
			{
				var row = universe[i];

				expandedList.Add(row);

				if (row.All(spot => spot == '.'))
					expandedList.Add(new string('.', row.Length));
			}


			for (int i = 0; i < expandedList[0].Length; i++)
			{
				if (expandedList.All(r => r[i] == '.'))
				{
					foreach (var row in expandedList)
					{
						row.Insert(i, ".");
					}
					i++;
				}

			}

			return new Universe(expandedList.ToArray());
		}

		public Coordinate[] GetStars()
		{
			List<Coordinate> stars = new List<Coordinate>();

			for (int i = 0; i < universe.Length; i++)
			{
				for (int j = 0; j < universe[i].Length; j++)
				{
					if (universe[i][j] == '#')
						stars.Add(new Coordinate(i,j));
				}
			}

			return stars.ToArray();
		}

		public override string ToString()
		{
			return universe.Select(s => s.ToArray()).ToArray().To2DArray().Print();
		}
	}
}
