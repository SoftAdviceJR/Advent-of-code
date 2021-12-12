using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Submarine.Helpers;

namespace Submarine.Day11
{
	public class OctopusFlasher
	{
		private int[,] octopi;

		int rowCount;
		int colCount;

		public OctopusFlasher(int[,] octopi)
		{
			this.octopi = octopi;
			this.rowCount = octopi.GetLength(0);
			this.colCount = octopi.GetLength(1);
		}

		public void Increment()
		{

			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					octopi[i, j]++;
				}
			}





		}

		public int Flash()
		{
			int flashes = 0;

			while (octopi.Any(o => o >= 10))
			{
				for (int i = 0; i < rowCount; i++)
				{
					for (int j = 0; j < colCount; j++)
					{
						if (octopi[i, j] >= 10)
						{
							flashes++;
							octopi[i, j] = -10000;
							octopi.ForeachNeighbour(i, j, x => x + 1);
						}
					}
				}
			}

			octopi = octopi.Select(x => x < 0 ? 0 : x);

			return flashes;

		}
	}
}
