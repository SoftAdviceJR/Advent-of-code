using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day17
{
	static class Solution
	{
		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("Day17/Input.txt");

			return lines;
		}

		public static decimal Part1()
		{
			var lines = ReadInput();

			var cannon = new Cannon(lines[0]);
			var vxRange = cannon.VxRange();
			var vyRange = cannon.VyRange();


			var result = cannon.GetPath(vxRange.Item1, vyRange.Item2);

			return result.Max(t => t.Item2);
		}

		public static long Part2()
		{
			var lines = ReadInput();

			var cannon = new Cannon(lines[0]);
			var vxRange = cannon.VxRange();
			var vyRange = cannon.VyRange();

			int count = 0;

			for (int vx = vxRange.Item1; vx < vxRange.Item2 + 1; vx++)
			{
				for (int vy = vyRange.Item1; vy < vyRange.Item2 + 1; vy++)
				{
					if (cannon.TryShoot(vx, vy))
						count++;
				}
			}

			return count;
		}

		

		
	}


}
