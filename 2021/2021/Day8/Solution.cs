using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day8
{
	static class Solution
	{
		private static string[] ReadInput()
		{
			return File.ReadAllLines("Day8/Input.txt");
		}

		public static decimal Part1()
		{
			var lines = ReadInput();

			Code[] codes = new Code[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				codes[i] = new Code(lines[i]);
			}

			var outputs = codes.SelectMany(code => code.Output);

			int oneCount = outputs.Count(x => x.Length == 2);
			int fourCount = outputs.Count(x => x.Length == 4);
			int sevenCount = outputs.Count(x => x.Length == 3);
			int eightCount = outputs.Count(x => x.Length == 7);

			return oneCount + fourCount + sevenCount + eightCount;
		}

		public static long Part2()
		{
			var lines = ReadInput();

			Decoder[] decoders = new Decoder[lines.Length];

			for (int i = 0; i < lines.Length; i++)
			{
				decoders[i] = new Decoder(new Code(lines[i]));
				decoders[i].Decode();
			}


			return decoders.Select(d => d.Output).Sum();
		}

		


	}
}
