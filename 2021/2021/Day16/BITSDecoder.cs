using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day16
{
	public static class BITSDecoder
	{
		private static readonly Dictionary<char, string> hexaToBinary = new Dictionary<char, string>
		{
			{ '0', "0000"},
			{ '1', "0001"},
			{ '2', "0010"},
			{ '3', "0011"},
			{ '4', "0100"},
			{ '5', "0101"},
			{ '6', "0110"},
			{ '7', "0111"},
			{ '8', "1000"},
			{ '9', "1001"},
			{ 'A', "1010"},
			{ 'B', "1011"},
			{ 'C', "1100"},
			{ 'D', "1101"},
			{ 'E', "1110"},
			{ 'F', "1111"},
		};

		public static Packet Decode(string input)
		{
			string binary = string.Concat(input.Select(h => hexaToBinary[h]));

			return Packet.Parse(binary, out string _);
		}
	}
}
