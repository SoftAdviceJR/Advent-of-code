using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Submarine.Helpers;

namespace Submarine.Day5
{
	static class Solution
	{
		public static long Part1()
		{
			var lines = File.ReadAllLines("Day5/Input.txt");

			List<Line> mapLines = new List<Line>();

			foreach (var line in lines)
			{
				var coords = line.Split(" -> ");

				var start = coords[0].Split(",");
				var end = coords[1].Split(",");

				var startCoordinate = new Coordinate(int.Parse(start[0]), int.Parse(start[1]));
				var endCoordinate = new Coordinate(int.Parse(end[0]), int.Parse(end[1]));

				mapLines.Add(new Line( startCoordinate, endCoordinate));
			}

			int width = mapLines.Max(line => Math.Max(line.Start.X, line.End.X));
			int height = mapLines.Max(line => Math.Max(line.Start.Y, line.End.Y));

			Map map = new Map(width + 1, height + 1);

			foreach (var line in mapLines)
			{
				map.AddLine(line);

				//map.crossings.Print2DArray();
				//Console.WriteLine();
			}

			return map.crossings.ToList().Count(x => x >= 2);
		}

		public static long Part2()
		{
			var lines = File.ReadAllLines("Day5/Input.txt");

			List<Line> mapLines = new List<Line>();

			foreach (var line in lines)
			{
				var coords = line.Split(" -> ");

				var start = coords[0].Split(",");
				var end = coords[1].Split(",");

				var startCoordinate = new Coordinate(int.Parse(start[0]), int.Parse(start[1]));
				var endCoordinate = new Coordinate(int.Parse(end[0]), int.Parse(end[1]));

				mapLines.Add(new Line(startCoordinate, endCoordinate));
			}

			int width = mapLines.Max(line => Math.Max(line.Start.X, line.End.X));
			int height = mapLines.Max(line => Math.Max(line.Start.Y, line.End.Y));

			Map map = new Map(width + 1, height + 1);

			foreach (var line in mapLines)
			{
				map.AddLine(line, true);

				//map.crossings.Print2DArray();
				//Console.WriteLine();
			}

			return map.crossings.ToList().Count(x => x >= 2);
		}
	}
}
