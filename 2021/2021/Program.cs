using System;
using System.IO;

namespace SolarSweep
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Day 1, part 1: " + Day1.Solution.Part1());
			Console.WriteLine("Day 1, part 2: " + Day1.Solution.Part2());
			Console.WriteLine();

			Console.WriteLine("Day 2, part 1: " + Day2.Solution.Part1());
			Console.WriteLine("Day 2, part 2: " + Day2.Solution.Part2());
			Console.WriteLine();

			Console.WriteLine("Day 3, part 1: " + Day3.Solution.Part1());
			Console.WriteLine("Day 3, part 2: " + Day3.Solution.Part2());
			Console.WriteLine();

			Console.WriteLine("Day 4, part 1: " + Day4.Solution.Part1());
			Console.WriteLine("Day 4, part 2: " + Day4.Solution.Part2());
			Console.ReadLine();
		}

		
	}
}
