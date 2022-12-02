using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day2
{
	static class Part1
	{
		public static int[][] Read()
		{
			List<int[]> results = new List<int[]>();

			var rounds = File.ReadAllLines("Day2/input.txt");

			foreach (var round in rounds)
			{
				int[] plays = new int[2];
				plays[0] = ParseEnemyPlay(round[0]);
				plays[1] = ParseYourPlay(round[2]);

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
				return 1;
			if (play == 'B')
				return 2;
			if (play == 'C')
				return 3;

			throw new ArgumentException($"{play} is not a valid play");
		}

		public static int ParseYourPlay(char play)
		{
			if (play == 'X')
				return 1;
			if (play == 'Y')
				return 2;
			if (play == 'Z')
				return 3;

			throw new ArgumentException($"{play} is not a valid play");
		}

		public static int Score(int[] plays)
		{
			var score = plays[1];

			if (plays[0] == plays[1])
				return score + 3;

			if (DoYouWin(plays))
				return score + 6;

			return score;
		}

		public static bool DoYouWin(int[] plays)
		{
			return plays[1] % 3 == (plays[0] + 1) % 3;
		}


	}
}
