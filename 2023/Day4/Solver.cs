using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day4
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day4\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
			ReadInput();
		}

		private static Card[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day4\\input.txt");

			var cards = new Card[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				cards[i] = Card.Parse(lines[i]);
			}

			return cards;

		}

		public string SolvePart1()
		{
			var cards = ReadInput();

			var score = cards.Select(c => c.Score()).Sum();

			return score.ToString();
		}


		public string SolvePart2()
		{
			var cards = ReadInput();

			var winnings = new Dictionary<int, List<Card>>();

			foreach (var card in cards)
			{
				winnings.Add(card.ID, new List<Card>());
			}


			for (int i = 0; i < cards.Length; i++)
			{
				var card = cards[i];

				int id = cards[i].ID;

				winnings[id].Add(cards[i]);

				var copies = card.CountMatches();

				for (int j = 1; j <= copies; j++)
				{
					var wonCard = cards[i + j];

					for (int k = 0; k < winnings[id].Count; k++)
						winnings[wonCard.ID].Add(wonCard);
				}
			}

			return winnings.SelectMany(k => k.Value).Count().ToString();
		}



	}


	public class Card
	{
		public int ID { get; }

		public int[] WinningNumbers { get; }

		public int[] MyNumbers { get; set; }

		public static Card Parse(string line)
		{
			return new Card(line);
		}

		private Card(string line)
		{
			var split1 = line.Split(':');

			ID = int.Parse(split1[0].Substring(5));

			var split2 = split1[1].Split('|');

			string[] winningNumbers = split2[0].Trim().Split(' ').Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();
			string[] myNumbers = split2[1].Trim().Split(' ').Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();

			WinningNumbers = winningNumbers.Select(n => int.Parse(n)).ToArray();
			MyNumbers = myNumbers.Select(n => int.Parse(n)).ToArray();
		}

		public int Score()
		{
			int matches = CountMatches();

			if (matches == 0)
				return 0;

			return (int)Math.Pow(2, matches - 1);
		}

		public int CountMatches()
		{
			return MyNumbers.Count(IsWinnigNumber);
		}

		public bool IsWinnigNumber(int number)
		{
			return WinningNumbers.Contains(number);
		}
	}
}
