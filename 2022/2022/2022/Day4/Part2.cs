using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2022.Day4
{
	static class Part2
	{
		public static Assignment[][] Read()
		{
			List<Assignment[]> results = new List<Assignment[]>();

			var pairs = File.ReadAllLines("Day4/input.txt");

			foreach (string pair in pairs)
			{
				var assingments = new Assignment[2];

				var ass = pair.Split(',');
				var ranges = ass.Select(a => a.Split('-')).ToArray();

				assingments[0] = new Assignment(int.Parse(ranges[0][0]), int.Parse(ranges[0][1]));
				assingments[1] = new Assignment(int.Parse(ranges[1][0]), int.Parse(ranges[1][1]));

				results.Add(assingments);
			}


			return results.ToArray();
		}

		public static int Solve()
		{
			var input = Read();

			int count = input.Count(p => p[0].OverlapsWith(p[1]));

			return count;
		}

		

	}

	
		
}
