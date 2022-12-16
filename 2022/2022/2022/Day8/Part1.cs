using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2022.Day8
{
	static class Part1
	{
		public static int[,] ReadForest()
		{
			var input = File.ReadAllLines("Day8/input.txt");

			int[,] forest = new int[input.Length, input[0].Length];

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[i].Length; j++)
				{
					forest[i,j] = int.Parse(input[i][j].ToString());
				}
			}
			

			return forest;
		}

		public static string Solve()
		{
			var forest = ReadForest();

			int size = forest.GetLength(0);

			bool[,] visible = new bool[size, size];

			int lrMax = 0;
			int rlMax = 0;
			int udMax = 0;
			int duMax = 0;

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (i == 0 || j == 0 || i == size - 1 || j == size - 1)
						visible[i, j] = true;
					else
					{
						
					}
				}
			}

			

			return visible.Cast<bool>().Count(v => v).ToString();
		}

		



	}

	





}
