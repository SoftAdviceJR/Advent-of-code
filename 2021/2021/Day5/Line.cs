using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day5
{
	class Coordinate
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	class Line
	{
		public Coordinate Start { get; private set; }
		public Coordinate End { get; private set; }

		public Line(Coordinate start, Coordinate end)
		{
			Start = start;
			End = end;
		}

		public Line(int x1, int y1, int x2, int y2): this(new Coordinate(x1, y1), new Coordinate(x2, y2))
		{

		}
	}
}
