using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2022.Day6
{
	static class Part2
	{
		public static string Solve()
		{
			var input = File.ReadAllText("Day6/input.txt");

			string mem = input.Substring(0, 14);


			for (int i = 14; i < input.Length; i++)
			{
				if (mem.Distinct().Count() != mem.Length)
				{
					mem = mem.Substring(1, 14-1) + input[i];
					continue;
				}

				return i.ToString();
			}

			throw new InvalidOperationException("Start of packet never found...");
		}


	}





}
