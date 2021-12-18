using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Submarine.Day17
{
	class Cannon
	{
		public readonly int targetMinX;
		public readonly int targetMinY;
		public readonly int targetMaxX;
		public readonly int targetMaxY;

		public Cannon(string input)
		{
			var digits = new Regex(@"(-?[0-9]*)");

			var matches = digits.Matches(input).Where(m => !string.IsNullOrWhiteSpace(m.Value));
			if (matches.Count() != 4)
				throw new InvalidOperationException("Invalid input");

			int x1 = int.Parse(matches.ElementAt(0).Value);
			int x2 = int.Parse(matches.ElementAt(1).Value);

			int y1 = int.Parse(matches.ElementAt(2).Value);
			int y2 = int.Parse(matches.ElementAt(3).Value);

			targetMinX = Math.Min(x1, x2);
			targetMaxX = Math.Max(x1, x2);
			targetMinY = Math.Min(y1, y2);
			targetMaxY = Math.Max(y1, y2);

		}

		public bool TryShoot(int vx, int vy)
		{
			var landedOn = GetPath(vx, vy).Last();

			return OnTarget(landedOn.Item1, landedOn.Item2);
		}

		public Tuple<int, int>[] GetPath(int vx, int vy)
		{
			int x = 0;
			int y = 0;

			List<Tuple<int, int>> trajectory = new List<Tuple<int, int>>();

			while(!Overshot(x, y) && !OnTarget(x,y))
			{
				x += vx;
				y += vy;
				vx = Math.Max(0, vx - 1);
				vy--;
				trajectory.Add(new Tuple<int, int>(x, y));
			}

			return trajectory.ToArray();
		}

		public Tuple<int, int> VxRange()
		{
			int i = 1;
			while (true)
			{
				if (SumOfIntegers(i) >= targetMinX)
					break;
				i++;
			}
			return new Tuple<int, int>(i, targetMaxX);
		}

		public Tuple<int, int> VyRange()
		{
			int minVy = targetMinY;
			int maxVy = Math.Abs(minVy) - 1;

			return new Tuple<int, int>(minVy, maxVy);
		}

		private bool Overshot(int x, int y)
		{
			return x > targetMaxX || y < targetMinY;
		}

		private bool OnTarget(int x, int y)
		{
			return x >= targetMinX && x <= targetMaxX && y >= targetMinY && y <= targetMaxY;
		}

		private static int SumOfIntegers(int n)
		{
			return (n * (n + 1)) / 2;
		}


	}
}
