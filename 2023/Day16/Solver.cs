using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day16
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day16\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static Map ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day16\\input.txt");

			return new Map(lines);

		}

		public string SolvePart1()
		{
			var map = ReadInput();

			var beams = map.Energize(new Beam(new Coordinate(0, 0), Direction.Right));

			//var energy = map.Energize();

			//Console.WriteLine(map.RenderEnergy());

			return beams.ToString();
		}


		public string SolvePart2()
		{
			var map = ReadInput();

			int energy = 0;

			for (int i = 0; i < map.Height; i++)
			{
				var energyLeft = map.Energize(new Beam(new Coordinate(i, 0), Direction.Right));
				energy = Math.Max(energy, energyLeft);
				map.Reset();

				var energyRight = map.Energize(new Beam(new Coordinate(i, map.Width - 1), Direction.Left));
				energy = Math.Max(energy, energyRight);
				map.Reset();

				Console.WriteLine($"Energy Row {i + 1}: {energyLeft} | {energyRight} ");
			}

			for (int i = 0; i < map.Width; i++)
			{
				if (i == 3)
					Debugger.Break();

				var energyTop = map.Energize(new Beam(new Coordinate(0, i), Direction.Down));
				energy = Math.Max(energy, energyTop);
				map.Reset();

				var energyBottom = map.Energize(new Beam(new Coordinate(map.Height - 1, i), Direction.Up));
				energy = Math.Max(energy, energyBottom);
				map.Reset();

				Console.WriteLine($"Energy Column {i + 1}: {energyTop} | {energyBottom} ");
			}


			return energy.ToString();
		}



	}

	public class Map
	{
		private Construction[] constructions { get; }

		public List<Coordinate> energized { get; private set; } = new List<Coordinate>();

		public int Height { get; }

		public int Width { get; }

		public Map(string[] lines)
		{
			List<Construction> _constructions = new List<Construction>();

			Height = lines.Length;
			Width = lines[0].Length;

			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					char symbol = lines[i][j];

					if (symbol == '/' || symbol == '\\')
						_constructions.Add(new Mirror(new Coordinate(i, j), symbol));

					if (symbol == '|' || symbol == '-')
						_constructions.Add(new Splitter(new Coordinate(i, j), symbol));
				}
			}

			constructions = _constructions.ToArray();
		}


		public void Reset()
		{
			energized = new List<Coordinate>();
		}

		private Dictionary<Beam, Beam[]> _cache = new Dictionary<Beam, Beam[]>();

		public int Energize(Beam beam)
		{
			return ContinuedPath(beam).Select(b => b.Position).Distinct().Count();
		}


		private Beam[] ContinuedPath(Beam beam)
		{
			if (_cache.ContainsKey(beam)) 
				return _cache[beam];

			List<Beam> history = new List<Beam>();

			var beams = new Beam[] { beam };

			while(beams.Length > 0)
			{
				var _beams = new List<Beam>();

				foreach (var beamToCalculate in beams)
				{
					if (history.Contains(beamToCalculate))
						continue;

					history.Add(beamToCalculate);

					var newBeams = Move(beamToCalculate);

					foreach (var newBeam in newBeams)
					{
						_beams.Add(newBeam);
					}
				}

				beams = _beams.ToArray();
			}


			_cache.Add(beam, history.ToArray());


			return history.ToArray();
		}


		public string RenderEnergy()
		{
			var sb = new StringBuilder();

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (energized.Any(e => e == new Coordinate(i, j)))
						sb.Append('#');
					else
						sb.Append('.');
				}

				sb.AppendLine();
			}

			return sb.ToString();
		}

		private Beam[] Move(Beam beam)
		{
			var beams = new List<Beam>();

			var construction = constructions.FirstOrDefault(c => c.Position == beam.Position);

			if (construction == null)
				beams.Add(beam.Next());
			else
			{
				beams.AddRange(construction.Move(beam));
			}


			return beams.Where(b => Contains(b.Position)).ToArray();
		}

		private bool Contains(Coordinate position)
		{
			return position.Row >= 0 && position.Column >= 0 && position.Row < Height && position.Column < Width;
		}
	}

	public abstract class Construction
	{
		public Coordinate Position { get; }
		public char Symbol { get; }

		protected Construction(Coordinate position, char symbol)
		{
			Position = position;
			Symbol = symbol;
		}

		public override string ToString()
		{
			return $"{Symbol} {Position}";
		}

		public abstract Beam[] Move(Beam beam);
	}

	public class Mirror : Construction
	{
		public Mirror(Coordinate position, char symbol) : base(position, symbol) { }

		public override Beam[] Move(Beam beam)
		{
			if (Symbol == '/')
			{
				switch (beam.Direction)
				{
					case Direction.Left:
						return new Beam[] { new Beam(new Coordinate(Position.Row + 1, Position.Column), Direction.Down) };
					case Direction.Right:
						return new Beam[] { new Beam(new Coordinate(Position.Row - 1, Position.Column), Direction.Up) };
					case Direction.Up:
						return new Beam[] { new Beam(new Coordinate(Position.Row, Position.Column + 1), Direction.Right) };
					case Direction.Down:
						return new Beam[] { new Beam(new Coordinate(Position.Row, Position.Column - 1), Direction.Left) };

				}
			}

			if (Symbol == '\\')
			{
				switch (beam.Direction)
				{
					case Direction.Left:
						return new Beam[] { new Beam(new Coordinate(Position.Row - 1, Position.Column), Direction.Up) };
					case Direction.Right:
						return new Beam[] { new Beam(new Coordinate(Position.Row + 1, Position.Column), Direction.Down) };
					case Direction.Up:
						return new Beam[] { new Beam(new Coordinate(Position.Row, Position.Column - 1), Direction.Left) };
					case Direction.Down:
						return new Beam[] { new Beam(new Coordinate(Position.Row, Position.Column + 1), Direction.Right) };

				}
			}

			throw new InvalidOperationException($"Unkown mirror configuration");
		}
	}

	public class Splitter : Construction
	{
		public Splitter(Coordinate position, char symbol) : base(position, symbol) { }

		public override Beam[] Move(Beam beam)
		{
			if (Symbol == '|')
			{
				switch (beam.Direction)
				{
					case Direction.Left:
					case Direction.Right:
						return new Beam[] {
							new Beam(new Coordinate(Position.Row + 1, Position.Column), Direction.Down),
							new Beam(new Coordinate(Position.Row - 1, Position.Column), Direction.Up) };
					case Direction.Up:
					case Direction.Down:
						return new Beam[] { beam.Next() };

				}
			}

			if (Symbol == '-')
			{
				switch (beam.Direction)
				{
					case Direction.Up:
					case Direction.Down:
						return new Beam[] {
							new Beam(new Coordinate(Position.Row, Position.Column - 1), Direction.Left),
							new Beam(new Coordinate(Position.Row, Position.Column + 1), Direction.Right) };
					case Direction.Left:
					case Direction.Right:
						return new Beam[] { beam.Next() };

				}
			}

			throw new InvalidOperationException($"Unkown mirror configuration");
		}
	}

	public enum Direction
	{
		Left,
		Right,
		Up,
		Down
	}

	public class Beam
	{
		public Coordinate Position { get; private set; }

		public Direction Direction { get; private set; }

		public Beam(Coordinate position, Direction direction)
		{
			Position = position;
			Direction = direction;
		}

		public Beam Next()
		{
			Coordinate position;

			if (Direction == Direction.Left)
			{
				position = new Coordinate(Position.Row, Position.Column - 1);
			}
			else if (Direction == Direction.Right)
			{
				position = new Coordinate(Position.Row, Position.Column + 1);
			}
			else if (Direction == Direction.Up)
			{
				position = new Coordinate(Position.Row - 1, Position.Column);
			}
			else if (Direction == Direction.Down)
			{
				position = new Coordinate(Position.Row + 1, Position.Column);
			}
			else
			{
				throw new InvalidOperationException($"Unkown Direction: {Direction}");
			}

			return new Beam(position, Direction);
		}

		public override string ToString()
		{
			return $"{Position} {Direction}";
		}

		public override bool Equals(object? obj)
		{
			if (obj?.GetType() != typeof(Beam)) return false;

			return Equals((Beam)obj);
		}

		private bool Equals(Beam other)
		{
			return this.Position == other.Position && this.Direction == other.Direction;
		}

		public override int GetHashCode()
		{
			return this.Position.GetHashCode() ^ this.Direction.GetHashCode();
		}
	}

}
