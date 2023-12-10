using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day9
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day9\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static Sequence[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day9\\input.txt");

			return lines.Select(l => new Sequence(l)).ToArray();

		}

		public string SolvePart1()
		{
			var sequences = ReadInput();

			int result = 0;

			foreach ( var sequence in sequences )
			{
				result += sequence.Predict();
			}

			return result.ToString();

		}


		public string SolvePart2()
		{
			var sequences = ReadInput();

			int result = 0;

			foreach (var sequence in sequences)
			{
				result += sequence.PredictPast();
			}

			return result.ToString();
		}



	}


	public class Sequence
	{
		public int[] Numbers { get; } = new int[0];

		public Sequence(string line)
		{
			Numbers = line.Split(' ').Select(int.Parse).ToArray();
		}

		public Sequence(int[] numbers)
		{
			Numbers = numbers;
		}

		public int Predict()
		{
			if (AllZeroes)
				return 0;

			return Numbers.Last() + Differences().Predict();
		}

		public int PredictPast()
		{
			string name = ToString();

			if (AllZeroes)
				return 0;

			return Numbers.First() - Differences().PredictPast();
		}

		private Sequence Differences()
		{
			var dif = new int[Numbers.Length-1];

			for (int i = 0; i < dif.Length; i++)
			{
				dif[i] = Numbers[i + 1] - Numbers[i];
			}

			return new Sequence(dif);
		}

		private bool AllZeroes => Numbers.All(x => x == 0);

		public override string ToString()
		{
			return $"[{string.Join(", ", Numbers)}]";
		}
	}
}
