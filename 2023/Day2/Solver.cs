using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day2
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day2\\{DateTime.Now:yyMMdd}.log");
				//builder.ForLogger().FilterMinLevel(LogLevel.Trace).WriteToConsole();
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static Game[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day2\\input.txt");

			var games = new Game[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				games[i] = Game.Parse(lines[i]);
			}

			return games;
		}

		public string SolvePart1()
		{
			var games = ReadInput();

			Dictionary<string, int> maxCubes = new Dictionary<string, int>
			{
				{ "red", 12 },
				{ "green", 13 },
				{ "blue", 14}
			};

			var possibleGames = games.Where(game => game.IsPossible(maxCubes));

			return possibleGames.Sum(game => game.ID).ToString();
		}


		public string SolvePart2()
		{
			var games = ReadInput();

			var minCubesPerGame = games.Select(g => g.MinimumCubes(new string[] { "red", "green", "blue" }));

			var result = 0;

			foreach (var minCubes in minCubesPerGame)
			{
				int power = 1;

				foreach (var value in minCubes.Values)
				{
					power *= value;
				}

				result += power;
			}

			return result.ToString();
		}



	}


	class Game
	{
		public int ID { get; private set; }

		public Dictionary<string, int>[] CubeSets { get; private set; } = new Dictionary<string, int>[0];

		public static Game Parse(string line)
		{
			var game = new Game();

			var parts = line.Split(':');

			{
				string idString = parts[0].Substring(5);

				game.ID = int.Parse(idString);
			}

			{
				string[] rounds = parts[1].Split(";");

				game.CubeSets = new Dictionary<string, int>[rounds.Length];

				for (int i = 0; i < rounds.Length; i++)
				{
					var colorSets = rounds[i].Split(',');

					game.CubeSets[i] = new Dictionary<string, int>();

					foreach (var colorSet in colorSets)
					{
						var info = colorSet.Trim().Split(' ');

						game.CubeSets[i].Add(info[1], int.Parse(info[0]));
					}
				}
			}

			return game;
		}

		private Game()
		{

		}

		public bool IsPossible(Dictionary<string, int> maxCubes)
		{
			return CubeSets.All(cubeSet =>
			{
				return cubeSet.All(c =>
				{
					return maxCubes[c.Key] >= c.Value;
				});
			});
		}
		
		public Dictionary<string, int> MinimumCubes(string[] colors)
		{
			var minCubes = new Dictionary<string, int>();

			foreach (var color in colors)
			{
				var maxDiceOfColor = CubeSets.Max(set =>
				{
					if (set.ContainsKey(color))
						return set[color];
					return 0;
				});

				minCubes.Add(color, maxDiceOfColor);
			}

			return minCubes;
		}
	}

}
