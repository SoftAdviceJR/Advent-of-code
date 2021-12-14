using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day13
{
	static class Solution
	{
		private static Sheet ReadInput(out string[] instructions)
		{
			var lines = File.ReadAllLines("Day13/Input.txt");

			List<Coordinate> dots = new List<Coordinate>();
			List<string> instr = new List<string>();

			bool readingCoordinates = true;

			foreach (var line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					readingCoordinates = false;
					continue;
				}
				if (readingCoordinates)
				{
					var coords = line.Split(",");

					dots.Add(new Coordinate(int.Parse(coords[0]), int.Parse(coords[1])));
				}
				else
				{
					instr.Add(line);
				}
			}

			var sheet = new Sheet(dots);

			instructions = instr.ToArray();

			return sheet;
		}

		public static decimal Part1()
		{
			var sheet = ReadInput(out string[] instructions);


			sheet.Fold(instructions[0]);

			return sheet.Dots.Count;
		}

		public static long Part2()
		{
			var sheet = ReadInput(out string[] instructions);


			foreach (var instruction in instructions)
			{
				sheet.Fold(instruction);
			}

			sheet.Print();

			return sheet.Dots.Count;
		}

		
	}


}
