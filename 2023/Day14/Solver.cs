using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using _2023.Day13;

using NLog;

namespace _2023.Day14
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day14\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static Platform ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day14\\input.txt");

			return new Platform(lines);

		}

		public string SolvePart1()
		{
			var platform = ReadInput();

			Console.WriteLine(platform.Render());
			Console.WriteLine();

			platform.TiltNorth();

			Console.WriteLine(platform.Render());

			var result = platform.CalculateLoad();

			return result.ToString();

		}


		public string SolvePart2()
		{
			var platform = ReadInput();

			int totalCycles = 1000000000;

			DateTime cycleStartTime = DateTime.Now;

			List<string> history = new List<string>();

			history.Add(platform.Render());

			for (int i = 0; i < totalCycles; i++)
			{
				platform.TiltNorth();
				platform.TiltWest();
				platform.TiltSouth();
				platform.TiltEast();

				string render = platform.Render();

				if (i % 10 == 0)
				{
					Console.WriteLine($"{i}/{totalCycles} | {(DateTime.Now - cycleStartTime).TotalMilliseconds}ms");
					cycleStartTime = DateTime.Now;
				}

				if (history.Contains(render))
				{
					history.Add (render);
					break;
				}

				history.Add(render);
			}

			int cycleStart = history.IndexOf(history.Last());

			int cycleLength = history.Count - cycleStart - 1;

			int indexOfLast = ((totalCycles - cycleStart) % cycleLength) + cycleStart;

			var finalPlatform = new Platform(history[indexOfLast].Split("\r\n"));

			var result = finalPlatform.CalculateLoad();

            Console.WriteLine(finalPlatform.Render());

            return result.ToString();
		}



	}

	public enum Direction
	{
		North,
		East,
		West,
		South
	}

	public class Platform
	{
		public int Height { get; }

		public int Width { get; }

		public Rock[] Rocks { get; }

		public Platform(string[] lines)
		{
			Height = lines.Length;
			Width = lines[0].Length;

			List<Rock> rocks = new List<Rock>();

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (lines[i][j] == '#')
						rocks.Add(new SquareRock(new Coordinate(i, j)));
					else if (lines[i][j] == 'O')
						rocks.Add(new RoundRock(new Coordinate(i, j)));
				}
			}

			Rocks = rocks.ToArray();
		}

		public void TiltNorth()
		{
			for (int i = 0; i < Height; i++)
			{
				var rocks = Rocks.Where(r => r.CanMove && r.Position.Row == i);

				foreach (var rock in rocks)
				{
					rock.Move(Direction.North, GetStepsNorth(rock));
				}
			}
		}

		public void TiltWest()
		{
			for (int i = 0; i < Width; i++)
			{
				var rocks = Rocks.Where(r => r.CanMove && r.Position.Column == i);

				foreach (var rock in rocks)
				{
					rock.Move(Direction.West, GetStepsWest(rock));
				}
			}
		}

		public void TiltSouth()
		{
			for (int i = Height - 1; i >= 0; i--)
			{
				var rocks = Rocks.Where(r => r.CanMove && r.Position.Row == i);

				foreach (var rock in rocks)
				{
					rock.Move(Direction.South, GetStepsSouth(rock));
				}
			}
		}

		public void TiltEast()
		{
			for (int i = Width - 1; i >= 0; i--)
			{
				var rocks = Rocks.Where(r => r.CanMove && r.Position.Column == i);

				foreach (var rock in rocks)
				{
					rock.Move(Direction.East, GetStepsEast(rock));
				}
			}
		}

		public long CalculateLoad()
		{
			var rocks = Rocks.Where(r => r.GetType() == typeof(RoundRock));

			return rocks.Sum(r => Height - r.Position.Row);
		}


		private int GetStepsNorth(Rock rock)
		{
			var blockingRocks = Rocks
				.Where(r => r.Position.Row < rock.Position.Row && r.Position.Column == rock.Position.Column);

			if (blockingRocks.Any())
				return rock.Position.Row - blockingRocks.Max(r => r.Position.Row) - 1;

			return rock.Position.Row;
		}

		private int GetStepsWest(Rock rock)
		{
			var blockingRocks = Rocks
				.Where(r => r.Position.Column < rock.Position.Column && r.Position.Row == rock.Position.Row);

			if (blockingRocks.Any())
				return rock.Position.Column - blockingRocks.Max(r => r.Position.Column) - 1;

			return rock.Position.Column;
		}

		private int GetStepsSouth(Rock rock)
		{
			var blockingRocks = Rocks
				.Where(r => r.Position.Row > rock.Position.Row && r.Position.Column == rock.Position.Column);

			if (blockingRocks.Any())
				return blockingRocks.Min(r => r.Position.Row) - rock.Position.Row - 1;

			return Height - rock.Position.Row - 1;
		}

		private int GetStepsEast(Rock rock)
		{
			var blockingRocks = Rocks
				.Where(r => r.Position.Column > rock.Position.Column && r.Position.Row == rock.Position.Row);

			if (blockingRocks.Any())
				return blockingRocks.Min(r => r.Position.Column) - rock.Position.Column - 1;

			return Width - rock.Position.Column - 1;
		}

		public string Render()
		{
			var sb = new StringBuilder();

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					var rock = Rocks.FirstOrDefault(r => r.Position.Row == i && r.Position.Column == j);

					if (rock == null)
						sb.Append('.');
					else
						sb.Append(rock.Symbol);
				}

				sb.AppendLine();
			}

			return sb.ToString().TrimEnd('\r', '\n');
		}
	}

	public abstract class Rock
	{
		public Coordinate Origin { get; }
		public Coordinate Position { get; protected set; }

		public abstract char Symbol { get; }

		public abstract bool CanMove { get; }

		protected Rock(Coordinate position)
		{
			Origin = position;
			Position = position;
		}




		public abstract void Move(Direction direction, int steps);

	}

	public class RoundRock : Rock
	{
		public override char Symbol => 'O';
		public override bool CanMove => true;

		public RoundRock(Coordinate start) : base(start)
		{

		}


		public override void Move(Direction direction, int steps)
		{
			switch (direction)
			{
				case Direction.North:
					Position = new Coordinate(Position.Row - steps, Position.Column);
					break;
				case Direction.East:
					Position = new Coordinate(Position.Row, Position.Column + steps);
					break;
				case Direction.West:
					Position = new Coordinate(Position.Row, Position.Column - steps);
					break;
				case Direction.South:
					Position = new Coordinate(Position.Row + steps, Position.Column);
					break;
				default:
					throw new InvalidOperationException($"Unkown direction: {direction}");
			}

		}

		public override string ToString()
		{
			return $"{Origin} -> {Position}";
		}
	}

	public class SquareRock : Rock
	{
		public override char Symbol => '#';
		public override bool CanMove => false;

		public SquareRock(Coordinate start) : base(start)
		{
		}

		public override void Move(Direction direction, int steps)
		{
		}

		public override string ToString()
		{
			return $"# {Origin}";
		}
	}
}
