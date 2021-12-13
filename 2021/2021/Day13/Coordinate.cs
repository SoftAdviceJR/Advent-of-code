using Newtonsoft.Json;

namespace Submarine.Day13
{
	class Coordinate
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int GetCoordinate(char dim)
		{
			if (dim == 'x')
				return X;
			if (dim == 'y')
				return Y;
			return -1;
		}

		public void SetCoordinate(char dim, int value)
		{
			if (dim == 'x')
				X = value;
			if (dim == 'y')
				Y = value;
		}

		public bool Equals(Coordinate coord)
		{
			return this.X == coord.X && this.Y == coord.Y;
		}

		public override bool Equals(object obj)
		{
			Coordinate coord = obj as Coordinate;
			if (coord == null)
				return base.Equals(obj);
			return Equals(coord);
		}

		public override int GetHashCode()
		{
			return $"x:{X},y:{Y}".GetHashCode();
		}
	}
}