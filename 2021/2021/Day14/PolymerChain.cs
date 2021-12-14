using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day14
{
	class PolymerChain
	{
		public Dictionary<string, long> PairCounts { get; private set; }

		public Dictionary<char, long> CharacterCounts { get => CountCharacters(); }

		public Rule[] Rules { get; }

		private readonly char firstElement;
		private readonly char lastElement;

		public PolymerChain(string[] lines)
		{
			Rules = new Rule[lines.Length - 2];

			for (int i = 0; i < Rules.Length; i++)
			{
				Rules[i] = new Rule(lines[i + 2]);
			}

			Rules = Rules.OrderBy(r => r.Pair).ToArray();

			PairCounts = new Dictionary<string, long>();

			foreach (var rule in Rules)
			{
				PairCounts.Add(rule.Pair, 0);
			}

			for (int i = 0; i < lines[0].Length - 1; i++)
			{
				string pair = string.Concat(lines[0][i], lines[0][i + 1]);
				PairCounts[pair]++;
			}

			firstElement = lines[0].First();
			lastElement = lines[0].Last();

		}

		public void GrowChain()
		{
			var newPairs = new Dictionary<string, long>();

			foreach (var key in PairCounts.Keys)
			{
				newPairs.Add(key, 0);
			}

			foreach (var pair in PairCounts)
			{
				if (pair.Value == 0)
					continue;

				var rule = Rules.First(r => r.Pair == pair.Key);

				newPairs[string.Concat(pair.Key[0], rule.Insert)] += pair.Value;
				newPairs[string.Concat(rule.Insert, pair.Key[1])] += pair.Value;
			}

			PairCounts = newPairs;
		}

		private Dictionary<char, long> CountCharacters()
		{
			var charCounts = new Dictionary<char, long>();

			foreach (var pair in PairCounts)
			{
				foreach (var ch in pair.Key)
				{
					if (!charCounts.ContainsKey(ch))
						charCounts.Add(ch, 0);

					charCounts[ch] += pair.Value;
				}
			}

			charCounts[firstElement]++;
			charCounts[lastElement]++;

			charCounts = charCounts.ToDictionary(
				kv => kv.Key,
				kv => kv.Value / 2
				);

			return charCounts;
		}


	}
}
