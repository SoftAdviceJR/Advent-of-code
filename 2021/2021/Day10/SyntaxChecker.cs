using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day10
{
	class SyntaxChecker
	{
		private static readonly Dictionary<char, int> corruptScores = new Dictionary<char, int>()
		{
			{')', 3 },
			{']', 57 },
			{'}', 1197 },
			{'>', 25137 }
		};

		private static readonly Dictionary<char, int> missingScores = new Dictionary<char, int>()
		{
			{')', 1 },
			{']', 2 },
			{'}', 3 },
			{'>', 4 }
		};

		private static readonly Dictionary<char, char> pairs = new Dictionary<char, char>()
		{
			{'(', ')' },
			{'[', ']' },
			{'{', '}' },
			{'<', '>' }
		};

		public static int ScoreCorruptions(string[] lines)
		{
			int score = 0;

			foreach (var line in lines)
			{
				if (!TryReadToEnd(line, out char[] remainingChars))
					score += corruptScores[remainingChars.Last()];
			}

			return score;
		}

		public static long ScoreMissing(string[] lines)
		{
			List<long> scores = new List<long>();
			foreach (var line in lines)
			{
				long score = 0;
				if (TryReadToEnd(line, out char[] remainingChars))
				{
					for (int i = remainingChars.Length - 1; i >= 0; i--)
					{
						score *= 5;
						score += missingScores[remainingChars[i]];
					}
				}
				else
					score = -1;

				scores.Add(score);
			}

			scores = scores.Where(score => score != -1 && score != 0).ToList();

			scores.Sort();

			return scores[(scores.Count - 1) / 2];
		}


		private static bool TryReadToEnd(string line, out char[] remainingCharacters)
		{
			List<char> expectedChars = new List<char>();

			foreach (var ch in line)
			{
				if (IsOpeningCharacter(ch))
				{
					expectedChars.Add(pairs[ch]);
					continue;
				}
				else
				{
					if (!expectedChars.Any() || expectedChars.Last() != ch)
					{
						expectedChars.Add(ch);
						remainingCharacters = expectedChars.ToArray();
						return false;
					}
					expectedChars.RemoveAt(expectedChars.Count - 1);
				}
			}

			remainingCharacters = expectedChars.ToArray();
			return true;
		}

		private static bool IsOpeningCharacter(char ch)
		{
			return pairs.Keys.Contains(ch);
		}
	}
}
