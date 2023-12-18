using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		public static bool operator ==(Coordinate c1, Coordinate c2)
		{
			return c1.Equals(c2);
		}

		public static bool operator !=(Coordinate c1, Coordinate c2)
		{
			return !c1.Equals(c2);
		}

		public override bool Equals([NotNullWhen(true)] object? obj)
		{
			if(obj?.GetType() != typeof(Coordinate)) return false;

			return Equals((Coordinate)obj);
		}

		private bool Equals(Coordinate other)
		{
			return this.Row == other.Row && this.Column == other.Column;
		}

		public override int GetHashCode()
		{
			return Row.GetHashCode() ^ Column.GetHashCode();
		}
	}
}
