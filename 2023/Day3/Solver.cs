using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day3
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day3\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
			ReadInput();
		}

		private static Line[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day3\\input.txt");

			var parsedLines = new Line[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				parsedLines[i] = new Line(i, lines[i]);
			}

			return parsedLines;

		}

		public string SolvePart1()
		{
			var lines = ReadInput();

			var numbersToAdd = new List<Number>();

			for (int i = 0; i < lines.Length; i++)
			{
				foreach (var symbol in lines[i].Symbols)
				{
					var adjacentNumbers = new List<Number>();

					for (int j = Math.Max(0, symbol.Position.Row - 1); j <= Math.Min(lines.Length, symbol.Position.Row + 1); j++)
					{
						var rowNumbers = lines[j].Numbers.Where(number => number.AdjacentTo(symbol));
						adjacentNumbers.AddRange(rowNumbers);
					}

					logger.Debug($"\r\nSymbol {symbol.Value} on {symbol.Position} is adjacent to: \r\n{string.Join("\r\n", adjacentNumbers.Select(n => $"{n.Value} [{n.StartPosition},{n.EndPosition}]"))}");

					numbersToAdd.AddRange(adjacentNumbers);
					numbersToAdd = numbersToAdd.Distinct().ToList();
				}
			}

            return numbersToAdd.Sum(n => n.Value).ToString();
		}


		public string SolvePart2()
		{
			var lines = ReadInput();

			var numbersToAdd = new List<int>();

			for (int i = 0; i < lines.Length; i++)
			{
				foreach (var symbol in lines[i].Symbols)
				{
					if (symbol.Value != "*")
						continue;

					var adjacentNumbers = new List<Number>();

					for (int j = Math.Max(0, symbol.Position.Row - 1); j <= Math.Min(lines.Length, symbol.Position.Row + 1); j++)
					{
						var rowNumbers = lines[j].Numbers.Where(number => number.AdjacentTo(symbol));
						adjacentNumbers.AddRange(rowNumbers);
					}

					logger.Debug($"\r\nSymbol {symbol.Value} on {symbol.Position} is adjacent to: \r\n{string.Join("\r\n", adjacentNumbers.Select(n => $"{n.Value} [{n.StartPosition},{n.EndPosition}]"))}");

					if (adjacentNumbers.Count != 2)
						continue;

					numbersToAdd.Add(adjacentNumbers[0].Value * adjacentNumbers[1].Value);
				}
			}

			return numbersToAdd.Sum().ToString();
		}



	}

	class Line
	{
		public int Index { get; set; }

		public Number[] Numbers { get; set; }

		public Symbol[] Symbols { get; set; }

		public Line(int index, string line)
		{
			Index = index;

			bool isReadingDigit = false;

			var numbers = new List<Number>();

			var symbols = new List<Symbol>();

			string number = "";
			int numberStart = 0;

			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				if (char.IsDigit(c))
				{
					if (!isReadingDigit)
						numberStart = i;

					isReadingDigit = true;
					number += c;
				}
				else
				{
					if(isReadingDigit)
					{
						numbers.Add(new Number(index, numberStart, i - 1, number));
					}

					isReadingDigit = false;
					number = "";

					if(c != '.')
					{
						symbols.Add(new Symbol(index, i, c.ToString()));
					}
				}
			}

			if(isReadingDigit)
				numbers.Add(new Number(index, numberStart, line.Length - 1, number));

			Numbers = numbers.ToArray();
			Symbols = symbols.ToArray();
		}
	}

	class Number
	{
		public int Value { get; set; }

		public Coordinate StartPosition { get; set; }
		public Coordinate EndPosition { get; set; }

		public Number(int row, int start, int end, string value)
		{
			Value = int.Parse(value);

			StartPosition = new Coordinate(row, start);
			EndPosition = new Coordinate(row, end);
		}

		public bool AdjacentTo(Symbol symbol)
		{
			bool adjacentRow = Math.Abs(symbol.Position.Row - StartPosition.Row) <= 1;
			if (!adjacentRow) return false;

			bool adjacentColumn = symbol.Position.Column >= StartPosition.Column - 1 && symbol.Position.Column <= EndPosition.Column + 1;

			return adjacentColumn;
		}
	}

	class Symbol
	{
		public string Value { get; set; }
		public Coordinate Position { get; set; }

		public Symbol(int row, int column, string value)
		{
			Value = value;
			Position = new Coordinate(row, column);
		}
	}

	class Coordinate
	{
		public int Row { get; set; }
		public int Column { get; set; }

		public Coordinate(int row, int column)
		{
			Row = row; Column = column;
		}

		public override string ToString()
		{
			return $"({Row}, {Column})";
		}
	}
}
