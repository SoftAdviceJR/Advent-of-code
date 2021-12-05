using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day1
{
	class Solution
	{
		public static int Part1()
		{
			int lastInput = 0;

			int increases = 0;

			foreach (var line in File.ReadLines("Day1/Input.txt"))
			{
				int depth = int.Parse(line);
				if (depth > lastInput)
					increases++;
				lastInput = depth;
			}

			return increases - 1;
		}

		public static int Part2()
		{
			int lastSum = 0;
			int increases = 0;

			var lines = File.ReadAllLines("Day1/Input.txt");
			var numbers = new int[lines.Length];

			numbers[0] = int.Parse(lines[0]);
			numbers[1] = int.Parse(lines[1]);

			for (int i = 0; i < lines.Length - 2; i++)
			{
				numbers[i + 2] = int.Parse(lines[i + 2]);
				int sum = numbers[i] + numbers[i + 1] + numbers[i + 2];
				if (sum > lastSum)
					increases++;
				lastSum = sum;
			}

			return increases - 1;
		}
	}
}
