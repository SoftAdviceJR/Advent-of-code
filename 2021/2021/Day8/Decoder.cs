using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day8
{
	class Decoder
	{
		private readonly Code code;
		public int[] Signals { get; private set; }

		public int Output { get => output[0] * 1000 + output[1] * 100 + output[2] * 10 + output[3]; }

		private int[] output;


		public Decoder(Code code)
		{
			this.code = code;
			this.Signals = new int[code.Signals.Length];
			this.output = new int[code.Output.Length];
		}

		public void Decode()
		{
			Dictionary<char, char> mapping = GetMapping();

			for (int i = 0; i < code.Signals.Length; i++)
			{
				Signals[i] = SegmentDisplay.Convert(string.Concat(code.Signals[i].Select(ch => mapping[ch])));
			}

			for (int i = 0; i < code.Output.Length; i++)
			{
				output[i] = SegmentDisplay.Convert(string.Concat(code.Output[i].Select(ch => mapping[ch])));
			}
		}

		private Dictionary<char, char> GetMapping()
		{
			Dictionary<char, char> mapping = new Dictionary<char, char>();

			string one = code.Signals.First(str => str.Length == 2);
			string four = code.Signals.First(str => str.Length == 4);
			string seven = code.Signals.First(str => str.Length == 3);
			string eight = code.Signals.First(str => str.Length == 7);

			char aChar = seven.First(ch => !one.Contains(ch));



			//cChar: one of the 6-length signals is missing a character inside 1
			string[] length_6 = code.Signals.Where(str => str.Length == 6).ToArray();
			string six = length_6.First(str => !str.Contains(one[0]) || !str.Contains(one[1]));

			char cChar = one.First(ch => !six.Contains(ch));


			//fChar is the other character in 1
			char fChar = one.First(ch => ch != cChar);


			string[] length_5 = code.Signals.Where(str => str.Length == 5).ToArray();

			string five = length_5.First(str => !str.Contains(cChar));
			string two = length_5.First(str => !str.Contains(fChar));
			string three = length_5.First(str => str != five && str != two);

			char bChar = four.First(ch => !three.Contains(ch));

			char dChar = four.First(ch => ch != bChar && ch != cChar && ch != fChar);

			char eChar = eight.First(ch => !three.Contains(ch) && ch != bChar);

			char gChar = three.First(ch => ch != aChar && ch != cChar && ch != dChar && ch != fChar);

			mapping.Add(aChar, 'a');
			mapping.Add(bChar, 'b');
			mapping.Add(cChar, 'c');
			mapping.Add(dChar, 'd');
			mapping.Add(eChar, 'e');
			mapping.Add(fChar, 'f');
			mapping.Add(gChar, 'g');


			return mapping;
		}


	}

	static class SegmentDisplay
	{
		public static int Convert(string input)
		{
			var orderedInput = string.Concat(input.OrderBy(c => c));

			switch (orderedInput)
			{
				case "abcefg":
					return 0;
				case "cf":
					return 1;
				case "acdeg":
					return 2;
				case "acdfg":
					return 3;
				case "bcdf":
					return 4;
				case "abdfg":
					return 5;
				case "abdefg":
					return 6;
				case "acf":
					return 7;
				case "abcdefg":
					return 8;
				case "abcdfg":
					return 9;
				default:
					throw new InvalidOperationException($"Unkown digit " + input);
			}

		}




	}
}
