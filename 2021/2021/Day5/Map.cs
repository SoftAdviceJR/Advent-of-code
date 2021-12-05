using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day5
{
	class Map
	{
		public int[,] crossings;

		public Map(int width, int height)
		{
			crossings = new int[width, height];
		}

		public void AddLine(Line line, bool includeDiagonals = false)
		{
			var xDiff = line.End.X - line.Start.X;
			var yDiff = line.End.Y - line.Start.Y;

			if (xDiff != 0 && yDiff != 0)
			{
				if(includeDiagonals)
				{
					if(xDiff > 0 && yDiff > 0)
					{
						int j = line.Start.Y;
						for (int i = line.Start.X; i < line.End.X + 1; i++)
						{
							crossings[j, i]++;
							j++;
						}
					}
					if (xDiff > 0 && yDiff < 0)
					{
						int j = line.Start.Y;
						for (int i = line.Start.X; i < line.End.X + 1; i++)
						{
							crossings[j, i]++;
							j--;
						}
					}
					if (xDiff < 0 && yDiff > 0)
					{
						int j = line.Start.Y;
						for (int i = line.Start.X; i > line.End.X - 1; i--)
						{
							crossings[j, i]++;
							j++;
						}
					}
					if (xDiff < 0 && yDiff < 0)
					{
						int j = line.Start.Y;
						for (int i = line.Start.X; i > line.End.X - 1; i--)
						{
							crossings[j, i]++;
							j--;
						}
					}
				}
				return;
			}

			if (xDiff > 0)
			{
				for (int i = line.Start.X; i < line.End.X + 1; i++)
				{
					crossings[line.Start.Y, i]++;
				}
				return;
			}
			if (xDiff < 0)
			{
				for (int i = line.Start.X; i > line.End.X - 1; i--)
				{
					crossings[line.Start.Y, i]++;
				}
				return;
			}
			if (yDiff > 0)
			{
				for (int i = line.Start.Y; i < line.End.Y + 1; i++)
				{
					crossings[i, line.Start.X]++;
				}
				return;
			}
			if (yDiff < 0)
			{
				for (int i = line.Start.Y; i > line.End.Y - 1; i--)
				{
					crossings[i, line.Start.X]++;
				}
				return;
			}

		}



	}
}
