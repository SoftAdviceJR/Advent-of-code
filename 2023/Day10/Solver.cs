using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using _2023.Day4;

using NLog;

namespace _2023.Day10
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day10\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day10\\input.txt");

			map = new Map(lines);
		}

		private readonly Map map;


		public string SolvePart1()
		{
			var start = map.FindStart();

			var history = new List<Coordinate>() { start };
			Coordinate[] current = history.ToArray();

			int steps = 0;

			while (true)
			{
				var newCoords = new List<Coordinate>();

				for (int i = 0; i < current.Length; i++)
				{
					var next = map.FindNext(current[i]).Where(c => !history.Contains(c)).ToArray();

					newCoords.AddRange(next);
				}

				if (newCoords.Count == 0)
					break;

				history.AddRange(newCoords);

				current = newCoords.ToArray();

				steps++;
			}

			return steps.ToString();
		}


		public string SolvePart2()
		{
			var start = map.FindStart();

			var history = new List<Coordinate>() { start };
			Coordinate[] current = history.ToArray();

			int steps = 0;

			while (true)
			{

				var newCoords = new List<Coordinate>();

				for (int i = 0; i < current.Length; i++)
				{
					var next = map.FindNext(current[i]).Where(c => !history.Contains(c)).ToArray();

					newCoords.AddRange(next);
				}

				if (newCoords.Count == 0)
					break;

				steps++;

				history.AddRange(newCoords);


				current = newCoords.ToArray();
			}

			var simplified = new Map(map.Visualize(history.ToArray()));


			logger.Debug($"Step {steps}: \r\n{simplified.map.Print()}");

			return simplified.Area().ToString();
		}



	}


	public struct Coordinate
	{
		public int Row { get; }

		public int Column { get; }

		public Coordinate(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public override string ToString()
		{
			return $"({Row}, {Column})";
		}
	}

	public class Map
	{
		private readonly int rows;
		private readonly int columns;

		public readonly char[,] map;

		public Map(string[] lines)
		{
			rows = lines.Length;
			columns = lines[0].Length;

			map = new char[rows, columns];

			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					map[i, j] = lines[i][j];
				}
			}
		}

		public Map(char[,] map)
		{
			this.map = map;

			this.rows = map.GetLength(0);
			this.columns = map.GetLength(1);
		}

		public Coordinate FindStart()
		{
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					if (map[i, j] == 'S')
						return new Coordinate(i, j);
				}
			}

			throw new InvalidOperationException();
		}

		public Coordinate[] FindNext(Coordinate start)
		{
			var coordinates = new List<Coordinate>();

			var symbol = map[start.Row, start.Column];

			if (symbol.ConnectsLeft())
			{
				var leftCoord = new Coordinate(start.Row, start.Column - 1);

				if (Exists(leftCoord) && Get(leftCoord).ConnectsRight())
					coordinates.Add(leftCoord);
			}

			if (symbol.ConnectsRight())
			{
				var rightCoord = new Coordinate(start.Row, start.Column + 1);

				if (Exists(rightCoord) && Get(rightCoord).ConnectsLeft())
					coordinates.Add(rightCoord);
			}

			if (symbol.ConnectsUp())
			{
				var upCoord = new Coordinate(start.Row - 1, start.Column);

				if (Exists(upCoord) && Get(upCoord).ConnectsDown())
					coordinates.Add(upCoord);
			}

			if (symbol.ConnectsDown())
			{
				var downCoord = new Coordinate(start.Row + 1, start.Column);

				if (Exists(downCoord) && Get(downCoord).ConnectsUp())
					coordinates.Add(downCoord);
			}

			return coordinates.ToArray();
		}

		public char[,] Visualize(Coordinate[] pipes)
		{
			var visual = new char[rows, columns];

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					visual[i, j] = pipes.Contains(new Coordinate(i, j)) ? map[i, j].Translate() : '.';
				}
			}

			return visual;
		}

		public int Area()
		{
			int area = 0;

			for (int i = 1; i < rows - 1; i++)
			{
				for (int j = 0; j < columns - 1; j++)
				{
					if (i == 8 && j == 95)
						Debugger.Break();

					if (IsInsideLoop(new Coordinate(i, j)))
						area++;
				}
			}

			return area;
		}

		private bool IsInsideLoop(Coordinate coord)
		{
			int ups = 0;
			int downs = 0;

			if (Get(coord) != '.')
				return false;

			for (int i = coord.Column - 1; i >= 0; i--)
			{
				if (map[coord.Row - 1, i].ConnectsDown())
					ups++;
				if (map[coord.Row + 1, i].ConnectsUp())
					downs++;
			}

			return Math.Min(ups, downs) % 2 == 1;
		}

		public bool InsideLoop(Coordinate coord)
		{
			if (map[coord.Row, coord.Column] != '.')
				return false;

			return true;
		}

		private char Get(Coordinate coord)
		{
			return map[coord.Row, coord.Column];
		}

		private bool Exists(Coordinate coord)
		{
			return coord.Row >= 0 && coord.Row < rows && coord.Column >= 0 && coord.Column < columns;
		}


	}


	public static class CharExtensions
	{
		public static bool ConnectsLeft(this char a)
		{
			return a == 'S' || a == '-' || a == 'J' || a == '7' || a == '╝' || a == '╗';
		}

		public static bool ConnectsRight(this char a)
		{
			return a == 'S' || a == '-' || a == 'F' || a == 'L' || a == '╚' || a == '╔';
		}

		public static bool ConnectsUp(this char a)
		{
			return a == 'S' || a == '|' || a == 'J' || a == 'L' || a == '╚' || a == '╝';
		}

		public static bool ConnectsDown(this char a)
		{
			return a == 'S' || a == '|' || a == 'F' || a == '7' || a == '╔' || a == '╗';
		}

		public static char Translate(this char a)
		{
			if (a == 'S')
				return '-';

			if (a == '7')
				return '╗';

			if (a == 'F')
				return '╔';

			if (a == 'L')
				return '╚';

			if (a == 'J')
				return '╝';

			return a;
		}
	}
}
