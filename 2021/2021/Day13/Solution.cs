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
		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("Day13/Input.txt");

			return lines;
		}

		public static decimal Part1()
		{
			var lines = ReadInput();

			List<Coordinate> dots = new List<Coordinate>();
			List<string> instructions = new List<string>();

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
					instructions.Add(line);
				}
			}

			var sheet = new Sheet(dots);

			Console.WriteLine();

			//sheet.Print();
			//Console.WriteLine(sheet.Width);

			//foreach (var instruction in instructions)
			//{
			//	sheet.Fold(instruction);
			//}

			sheet.Fold(instructions[0]);
			//Console.WriteLine(sheet.Width);
			//sheet.Print();



			return sheet.Dots.Distinct().Count();
		}

		public static long Part2()
		{
			var lines = ReadInput();

			return 0;
		}

		
	}


}
