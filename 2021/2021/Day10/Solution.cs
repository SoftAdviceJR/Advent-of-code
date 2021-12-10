using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day10
{
	static class Solution
	{
		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("Day10/Input.txt");

			

			return lines;
		}

		public static decimal Part1()
		{
			var lines = ReadInput();

			return SyntaxChecker.ScoreCorruptions(lines);
		}

		public static long Part2()
		{
			var lines = ReadInput();

			return SyntaxChecker.ScoreMissing(lines);
		}
	}
}
