using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using MathNet.Spatial.Euclidean;

using Newtonsoft.Json;

using Submarine.Day2;
using Submarine.Helpers;

namespace Submarine.Day21
{
	static class Solution
	{
		private static Player[] ReadInput()
		{
			var lines = File.ReadAllLines("Day21/Sample.txt");

			return lines.Select(line => new Player(line)).ToArray();
		}


		public static long Part1()
		{
			Player[] players = ReadInput();

			int dieRolls = 0;

			while (true)
			{
				int index = dieRolls % 2;

				var moves = GetD100Results(dieRolls);
				dieRolls++;

				players[index].Move(moves);

				if (players[index].Score >= 1000)
					break;
			}

			var loser = players.OrderBy(p => p.Score).First();

			return loser.Score * dieRolls * 3;
		}

		public static long Part2()
		{
			List<Universe> universes = new List<Universe>() { new Universe(ReadInput()) };

			long[] wins = new long[2];

			int turnCount = 0;

			var universeCounts = new Dictionary<int, int>();

			for (int i = 1; i < 4; i++)
			{
				for (int j = 1; j < 4; j++)
				{
					for (int k = 1; k < 4; k++)
					{
						if (!universeCounts.ContainsKey(i + j + k))
							universeCounts.Add(i + j + k, 1);
						else
							universeCounts[i + j + k]++;
					}
				}
			}

			while (universes.Any())
			{
				int turnPlayer = turnCount % 2;
				int otherPlayer = 1 - turnPlayer;

				var nextUniverses = new List<Universe>();

				universes.ForEach(u =>
				{
					for (int i = 3; i < 10; i++)
					{
						int count = universeCounts[i];

						var player = u.Players[turnPlayer];

						player.Move(i);

						var nextPlayers = new Player[2];
						nextPlayers[turnPlayer] = player;
						nextPlayers[otherPlayer] = u.Players[otherPlayer];

						Universe nextUniverse = new Universe()
						{
							Count = u.Count * count,
							Players = nextPlayers
						};

						if (player.Score < 21)
						{
							nextUniverses.Add(nextUniverse);
						}
						else
						{
							wins[turnPlayer] += nextUniverse.Count;
						}
					}
				});
				turnCount++;

				universes = nextUniverses;
			}

			return wins.Max();
		}

		private static int GetD100Results(int dieRolls)
		{

			int firstDie = (dieRolls * 3 + 0) % 100 + 1;
			int centerDie = (dieRolls * 3 + 1) % 100 + 1;
			int lastDie = (dieRolls * 3 + 2) % 100 + 1;

			return firstDie + centerDie + lastDie;
		}
	}
}
