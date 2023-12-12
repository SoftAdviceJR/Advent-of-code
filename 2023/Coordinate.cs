using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2023
{
	public struct Range
	{
		public long Start { get; }

		public long End { get; }

		public Range(long start, long end)
		{
			Start = Math.Min(start, end);
			End = Math.Max(end, start);
		}

		public bool Contains(long number)
		{
			return Start < number && End > number;
		}

		public bool ContainsInclusive(long number)
		{
			return Start <= number && End >= number;
		}
	}

	public struct Coordinate
	{
		public int Row { get; }

		public int Column { get; }

		public Coordinate(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public int ManhattanDistance(Coordinate coord)
		{
			return Math.Abs(coord.Row - Row) + Math.Abs(coord.Column - Column);
		}

		public override string ToString()
		{
			return $"({Row}, {Column})";
		}
	}
}
