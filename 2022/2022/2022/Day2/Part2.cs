using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day2
{
	static class Part2
	{
		public static int[][] Read()
		{
			List<int[]> results = new List<int[]>();

			var rounds = File.ReadAllLines("Day2/input.txt");

			Console.WriteLine($"Number of rounds {rounds.Length}");

			foreach (var round in rounds)
			{
				int[] plays = new int[2];
				plays[0] = ParseEnemyPlay(round[0]);
				plays[1] = ParseYourPlay(round[2], plays[0]);

				results.Add(plays);
			}


			return results.ToArray();
		}

		public static int Solve()
		{
			var input = Read();

			var score = input.Select(r => Score(r)).Sum();

			return score;
		}

		public static int ParseEnemyPlay(char play)
		{
			if (play == 'A')
				return 0;
			if (play == 'B')
				return 1;
			if (play == 'C')
				return 2;

			throw new ArgumentException($"{play} is not a valid play");
		}

		public static int ParseYourPlay(char play, int enemy)
		{
			if (play == 'X')
				return mod(enemy - 1, 3);
			if (play == 'Y')
				return enemy;
			if (play == 'Z')
				return mod(enemy + 1, 3);

			throw new ArgumentException($"{play} is not a valid play");
		}

		public static int Score(int[] plays)
		{
			var score = plays[1] + 1;

			if (plays[0] == plays[1])
			{
				score += 3;
				Console.WriteLine($"{plays[0]} vs {plays[1]} - Draw - {score}");
			}
			else if (DoYouWin(plays))
			{
				score += 6;
				Console.WriteLine($"{plays[0]} vs {plays[1]} - Win - {score}");
			}
			else
				Console.WriteLine($"{plays[0]} vs {plays[1]} - Loss - {score}");
			return score;
		}

		public static bool DoYouWin(int[] plays)
		{
			return mod(plays[1],3) == mod(plays[0] + 1, 3);
		}

		static int mod(int x, int m)
		{
			return (x % m + m) % m;
		}
	}
}
