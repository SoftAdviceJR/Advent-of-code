using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Submarine.Helpers;

namespace Submarine.Day13
{
	class Sheet
	{
		public List<Coordinate> Dots { get; private set; }

		public int Width { get; private set; }
		public int Height { get; private set; }

		public Sheet(List<Coordinate> dots)
		{
			this.Dots = dots;
			Width = dots.Max(d => d.X) + 1;
			Height = dots.Max(d => d.Y) + 1;
		}

		public void Fold(string instruction)
		{
			var newDots = new List<Coordinate>();

			string line = instruction.Split(" ")[2];

			char axis = line[0];
			int value = int.Parse(line.Split("=")[1]);

			foreach (var dot in Dots)
			{
				dot.SetCoordinate(axis, value - Math.Abs((dot.GetCoordinate(axis) - value)));
				if (!newDots.Any(d => d.X == dot.X && d.Y == dot.Y))
					newDots.Add(dot);
			}

			if (axis == 'x')
				Width = (Width -1 ) / 2;
			else
				Height = (Height - 1) / 2;


			Dots = newDots;
		}

		public void Print()
		{

			char[,] map = new char[Height, Width];

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (Dots.Contains(new Coordinate(j, i)))
						map[i, j] = '#';
					else
						map[i, j] = '.';
				}
			}

			map.Print2DArray();
			Console.WriteLine();
		}
	}
}
