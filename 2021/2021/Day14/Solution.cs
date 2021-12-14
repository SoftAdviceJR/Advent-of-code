using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day14
{
	static class Solution
	{
		private static PolymerChain ReadInput()
		{
			var lines = File.ReadAllLines("Day14/Input.txt");

			return new PolymerChain(lines);
		}

		public static decimal Part1()
		{
			var chain = ReadInput();

			for (int i = 0; i < 10; i++)
			{
				chain.GrowChain();
			}

			var counts = chain.CharacterCounts.OrderBy(kv => kv.Value);

			return counts.Last().Value - counts.First().Value;
		}

		public static long Part2()
		{
			var chain = ReadInput();

			for (int i = 0; i < 40; i++)
			{
				chain.GrowChain();
			}

			var counts = chain.CharacterCounts.OrderBy(kv => kv.Value);

			return counts.Last().Value - counts.First().Value;
		}

		
	}


}
