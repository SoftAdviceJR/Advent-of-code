using _2023.Day1;

using NLog;

namespace _2023
{
	internal class Program
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		static void SetupLogging()
		{

		}


		static void Main(string[] args)
		{
			SetupLogging();

			{
				//var solver = new Day1Solver();

				//Console.WriteLine($"Day 01 | Part 1: " + solver.SolvePart1());
				//Console.WriteLine($"Day 01 | Part 2: " + solver.SolvePart2());
			}

			{
				//var solver = new Day2.Solver();

				//Console.WriteLine($"Day 02 | Part 1: " + solver.SolvePart1());
				//Console.WriteLine($"Day 02 | Part 2: " + solver.SolvePart2());
			}

			//{
			//	var solver = new Day3.Solver();

			//	Console.WriteLine($"Day 03 | Part 1: " + solver.SolvePart1());
			//	Console.WriteLine($"Day 03 | Part 2: " + solver.SolvePart2());
			//}

			//{
			//	var solver = new Day4.Solver();

			//	Console.WriteLine($"Day 04 | Part 1: " + solver.SolvePart1());
			//	Console.WriteLine($"Day 04 | Part 2: " + solver.SolvePart2());
			//}

			{
				var solver = new Day5.Solver();

				Console.WriteLine($"Day 05 | Part 1: " + solver.SolvePart1());
				Console.WriteLine($"Day 05 | Part 2: " + solver.SolvePart2());
			}
		}
	}
}