using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Markup;

using NLog;

namespace _2023.Day7
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day7\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{

		}



		private static HandWithBid[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day7\\input.txt");

			return lines.Select(line => new HandWithBid(line)).ToArray();

		}

		public string SolvePart1()
		{
			var hands = ReadInput();

			var orderedHands = hands.OrderBy(hand => hand.Value).ToArray();

			int result = 0;

			for (int i = 0; i < orderedHands.Length; i++)
			{
				result += orderedHands[i].Bid * (i + 1);
			}

			return result.ToString();
		}


		public string SolvePart2()
		{
			var hands = ReadInput();

			var orderedHands = hands.OrderBy(hand => hand.ValueWithJoker).ToArray();

			int result = 0;

			for (int i = 0; i < orderedHands.Length; i++)
			{
				result += orderedHands[i].Bid * (i + 1);
			}

			return result.ToString();
		}



	}

	public class HandWithBid
	{
		public Hand Hand { get; }

		public int Bid { get; }

		public long Value => Hand.Value;

		public long ValueWithJoker => Hand.ValueWithJoker;

		public HandWithBid(string line)
		{
			var parts = line.Split(' ');
			Hand = new Hand(parts[0]);
			Bid = int.Parse(parts[1]);
		}

		public override string ToString()
		{
			return $"{Hand} {Hand.Value}";
		}
	}


	public class Hand
	{
		public Card[] Cards { get; }

		public long Value => CalculateValue();

		public long ValueWithJoker => CalculateValueWithJoker();

		public Hand(string hand)
		{
			Cards = hand.Select(c => new Card(c)).ToArray();
		}

		private long CalculateValue()
		{
			long value = 0;

			for (int i = 0; i < Cards.Length; i++)
			{
				value += int.Parse("1" + new string('0', (4 - i) * 2)) * Cards[i].Value;
			}


			value += GetHandType() * 100000000000;


			return value;
		}

		private long CalculateValueWithJoker()
		{
			long value = 0;

			for (int i = 0; i < Cards.Length; i++)
			{
				value += int.Parse("1" + new string('0', (4 - i) * 2)) * Cards[i].ValueWithJoker;
			}


			value += GetHandTypeWithJoker() * 100000000000;


			return value;
		}

		private int GetHandType()
		{
			var groups = Cards.GroupBy(c => c.Symbol).ToArray();

			return GetHandType(groups);
		}

		private static int GetHandType(IGrouping<char, Card>[] groups)
		{
			if (groups.Length == 1)
				return 7;

			if (groups.Any(g => g.Count() == 4))
				return 6;

			var group3 = groups.FirstOrDefault(g => g.Count() == 3);

			if (group3 != null)
			{
				if (groups.Any(g => g.Key != group3.Key && g.Count() == 2))
					return 5;

				return 4;
			}

			var pairs = groups.Where(g => g.Count() == 2);

			return 1 + pairs.Count();
		}

		private int GetHandTypeWithJoker()
		{
			var jokers = Cards.Where(c => c.Symbol == 'J').ToArray();

			if (jokers.Length == 0)
				return GetHandType();

			if (jokers.Length == 5)
				return GetHandType();

			var groups = Cards.Where(c => c.Symbol != 'J').GroupBy(c => c.Symbol).ToArray();

			var maxGroup = groups.MaxBy(g => g.Count());

			if (maxGroup == null)
				throw new InvalidOperationException();

			var sb = new StringBuilder();

			foreach ( var group in groups)
			{
				if (group.Key == maxGroup.Key || group.Key == 'J')
					continue;

				sb.Append(new string(group.Key, group.Count()));
			}

			sb.Append(new string(maxGroup.Key, maxGroup.Count() + jokers.Length));

			var hand = new Hand(sb.ToString());

			return hand.GetHandType();
		}

		public override string ToString()
		{
			return new string(Cards.Select(c => c.Symbol).ToArray());
		}
	}

	public class Card
	{
		public int Value { get; }

		public int ValueWithJoker { get; }
		public char Symbol { get; }

		public Card(char symbol)
		{
			Symbol = symbol;

			Value = SymbolMapping[symbol];
			ValueWithJoker = SymbolMappingWithJoker[symbol];
		}


		private static Dictionary<char, int> SymbolMapping = new Dictionary<char, int>()
		{
			{'2', 1 },
			{'3', 2 },
			{'4', 3 },
			{'5', 4 },
			{'6', 5 },
			{'7', 6 },
			{'8', 7 },
			{'9', 8 },
			{'T', 9 },
			{'J', 10 },
			{'Q', 11 },
			{'K', 12 },
			{'A', 13 },
		};

		private static Dictionary<char, int> SymbolMappingWithJoker = new Dictionary<char, int>()
		{
			{'J', 0 },
			{'2', 1 },
			{'3', 2 },
			{'4', 3 },
			{'5', 4 },
			{'6', 5 },
			{'7', 6 },
			{'8', 7 },
			{'9', 8 },
			{'T', 9 },
			//{'J', 10 },
			{'Q', 11 },
			{'K', 12 },
			{'A', 13 },
		};

		public override string ToString()
		{
			return Symbol.ToString();
		}
	}



}
