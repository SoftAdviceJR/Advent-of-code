using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

using NLog;

using static System.Formats.Asn1.AsnWriter;

namespace _2023.Day13
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day13\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static Sketch[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day13\\input.txt");

			List<string[]> groups = new List<string[]>();

			List<string> group = new List<string>();

			foreach (var line in lines)
			{
				if (!string.IsNullOrWhiteSpace(line))
					group.Add(line);
				else
				{
					groups.Add(group.ToArray());
					group = new List<string>();
				}
			}

			if (group.Any())
				groups.Add(group.ToArray());

			return groups.Select(g => new Sketch(g)).ToArray();

		}

		public string SolvePart1()
		{
			var sketches = ReadInput();

			return sketches.Sum(s => s.Score()).ToString();
		}


		public string SolvePart2()
		{
			var sketches = ReadInput();

			int score = 0;

			for (int i = 0; i < sketches.Length; i++)
			{
				int sketchScore = sketches[i].ScoreWithSmudge();

				score += sketchScore;

                Console.WriteLine($"{i+1}/{sketches.Length}");
                Console.WriteLine(sketches[i].Print());
                Console.WriteLine("->" + sketchScore);
                Console.WriteLine();
            }

			return score.ToString();
		}



	}

	public class Sketch
	{
		private readonly char[,] points;

		public Sketch(string[] lines)
		{
			points = lines.To2DArray();
		}

		public string Print()
		{
			return points.Print();
		}

		public override string ToString()
		{
			string firstLine = "";

			for (int i = 0; i < points.GetLength(1); i++)
			{
				firstLine += points[0, i];
			}

			return $"{firstLine} [{points.GetLength(0)},{points.GetLength(1)}]" ?? "(emtpy)";
		}

		public int Score()
		{
			return ScoreDimension(1) + 100 * ScoreDimension(0);
		}

		private string[] GetRows()
		{
			List<string> rows = new List<string>();

			for (int i = 0; i < points.GetLength(0); i++)
			{
				rows.Add(new string(points.GetRow(i)));
			}

			return rows.ToArray();
		}

		private string[] GetColumns()
		{
			List<string> rows = new List<string>();

			for (int i = 0; i < points.GetLength(1); i++)
			{
				rows.Add(new string(points.GetColumn(i)));
			}

			return rows.ToArray();
		}

		private int ScoreDimension(int dim)
		{
			int score = 0;

			var rows = dim == 0 ? GetRows() : GetColumns();

			for (int i = 0; i < rows.Length - 1; i++)
			{
				if (rows[i] != rows[i + 1])
					continue;

				int j = 1;

				bool mirrors = true;

				while (i - j >= 0 && i + 1 + j < rows.Length)
				{
					if (rows[i - j] != rows[i + 1 + j])
					{
						mirrors = false;
						break;
					}
					j++;
				}

				if (!mirrors)
					continue;

				score += (i + 1);
			}

			return score;
		}

		public int ScoreWithSmudge()
		{
			int score = 0;

			score += ScoreWithSmudge(GetRows()) * 100;

			score += ScoreWithSmudge(GetColumns());

			return score;

		}

		private int ScoreWithSmudge(string[] rows)
		{
			for (int i = 0; i < rows.Length - 1; i++)
			{
				var diff = 0;

				for (int j = 0; i - j >= 0 && i + 1 + j < rows.Length; j++)
				{
					diff += rows[i - j].CountDifferences(rows[i + 1 + j]);

					if (diff > 1)
						break;
				}

				if (diff == 1)
					return i + 1;
			}

			return 0;
		}
	}

	public static class Day13Extensions
	{
		public static int CountDifferences(this string myString, string other)
		{
			if (myString == other)
				return 0;

			if (myString.Length != other.Length)
				throw new InvalidOperationException($"strings are not of the same size");

			int count = 0;

			for (int i = 0; i < myString.Length; i++)
			{
				if (myString[i] != other[i])
					count++;
			}

			return count;
		}
	}

}
