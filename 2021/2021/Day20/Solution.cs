using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using MathNet.Spatial.Euclidean;

using Newtonsoft.Json;

using Submarine.Day2;
using Submarine.Helpers;

namespace Submarine.Day20
{
	static class Solution
	{
		private static char[,] ReadInput(out string key)
		{
			var lines = File.ReadAllLines("Day20/Input.txt");

			key = lines[0];

			char[,] image = new char[lines[2].Length, lines.Length - 2];

			for (int i = 2; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					image[i - 2, j] = lines[i][j];
				}
			}

			return image;
		}



		public static decimal Part1()
		{
			var image = ReadInput(out string key);

			char outsideChar = '.';

			image = image.ExpandWith(outsideChar);

			for (int n = 0; n < 2; n++)
			{
				image.Print2DArray();
				Console.WriteLine();

				var newImage = new char[image.GetLength(0), image.GetLength(1)];
				newImage.FillWithAt(image, 0, 0);

				image = image.ExpandWith(outsideChar);


				outsideChar = key[Convert.ToInt32(new string(image[0, 0] == '#' ? '1' : '0', 9), 2)];


				newImage = newImage.ExpandWith(outsideChar);

				for (int i = 1; i < image.GetLength(0) - 1; i++)
				{
					for (int j = 1; j < image.GetLength(1) - 1; j++)
					{
						var number = image.GetRange(i - 1, j - 1, 3, 3).Select(c => c == '#' ? '1' : '0').ToList();

						var index = Convert.ToInt32(string.Concat(number), 2);

						newImage[i, j] = key[index];
					}
				}

				image = newImage;
			}

			image.Print2DArray();

			return image.ToList().Count(c => c == '#');
		}

		public static long Part2()
		{
			var image = ReadInput(out string key);

			char outsideChar = '.';

			image = image.ExpandWith(outsideChar);

			for (int n = 0; n < 50; n++)
			{
				var newImage = new char[image.GetLength(0), image.GetLength(1)];
				newImage.FillWithAt(image, 0, 0);

				image = image.ExpandWith(outsideChar);


				outsideChar = key[Convert.ToInt32(new string(image[0, 0] == '#' ? '1' : '0', 9), 2)];


				newImage = newImage.ExpandWith(outsideChar);

				for (int i = 1; i < image.GetLength(0) - 1; i++)
				{
					for (int j = 1; j < image.GetLength(1) - 1; j++)
					{
						var number = image.GetRange(i - 1, j - 1, 3, 3).Select(c => c == '#' ? '1' : '0').ToList();

						var index = Convert.ToInt32(string.Concat(number), 2);

						newImage[i, j] = key[index];
					}
				}

				image = newImage;
			}

			return image.ToList().Count(c => c == '#');
		}
	}


}
