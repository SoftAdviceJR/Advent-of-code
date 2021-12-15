using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Submarine.Helpers;

namespace Submarine.Day15
{
	class Tile
	{
		public int X { get; }
		public int Y { get; }

		internal readonly int _risk;
		public int Risk
		{
			get
			{
				if (PreviousTile != null)
					return _risk + PreviousTile.Risk;

				return 0;
			}
		}
		public int Distance { get; private set; }
		public int Score { get => Risk + Distance; }

		//private int PathLength { get => GetPath().Count + 1; }

		public Tile PreviousTile { get; set; }

		public Tile(int x, int y, int risk)
		{
			this.X = x;
			this.Y = y;
			this._risk = risk;
		}

		public void SetDistance(Tile targetTile)
		{
			Distance = Math.Abs(targetTile.X - X) + Math.Abs(targetTile.Y - Y);
		}

		public Tile Copy()
		{
			var newTile = new Tile(X, Y, _risk);
			newTile.Distance = Distance;
			return newTile;
		}

		public void Print()
		{
			var path = GetPath();
			int rowCount = path.Max(x => x.Y) + 1;
			int colCount = path.Max(x => x.X) + 1;

			int[,] risks = new int[rowCount, colCount];

			risks.Fill(0);

			foreach (var tile in path)
			{
				risks[tile.Y, tile.X] = tile._risk;
			}

			risks.Print2DArray();
			Console.WriteLine();
		}

		private List<Tile> GetPath()
		{
			List<Tile> path = new List<Tile>();
			path.Add(this);
			if (PreviousTile != null)
				path.AddRange(PreviousTile.GetPath());
			return path;
		}

		public bool Equals(Tile tile)
		{
			return X == tile.X && Y == tile.Y;
		}

	}
}
