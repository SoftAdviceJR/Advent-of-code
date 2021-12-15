using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day15
{
	static class Solution
	{
		private static Tile[] ReadInput()
		{
			var lines = File.ReadAllLines("Day15/Input.txt");

			var tiles = new List<Tile>();

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				for (int j = 0; j < line.Length; j++)
				{
					tiles.Add(new Tile(j, i, int.Parse(line[j].ToString())));
				}
			}

			return tiles.ToArray();
		}

		public static decimal Part1()
		{
			var allTiles = ReadInput();

			var path = AStar(allTiles);

			

			return -1;
		}

		public static long Part2()
		{
			var firstSection = ReadInput();

			var allTiles = ExpandMap(firstSection, 5);


			var path = AStar(allTiles);

			//path.Print();

			return path.Risk;
		}

		private static Tile[] ExpandMap(Tile[] tiles, int multiplier)
		{
			int initialRowCount = tiles.Max(x => x.Y) + 1;
			int initialColCount = tiles.Max(x => x.X) + 1;

			var allTiles = new List<Tile>();

			for (int i = 0; i < initialRowCount * multiplier; i++)
			{
				for (int j = 0; j < initialColCount * multiplier; j++)
				{
					int risk = tiles.First(x => x.X == j % initialColCount && x.Y == i % initialRowCount)._risk + 
						(int)Math.Floor((double)i / initialRowCount) + (int)Math.Floor((double)j / initialColCount);

					while (risk >= 10)
						risk = risk - 9;

					allTiles.Add(new Tile(j, i, risk));
				}
			}

			return allTiles.ToArray();
		}

		private static Tile AStar(Tile[] allTiles)
		{
			var startTile = allTiles.First(x => x.X == 0 && x.Y == 0);
			int rowCount = allTiles.Max(x => x.Y) + 1;
			int colCount = allTiles.Max(x => x.X) + 1;
			var endTile = allTiles.First(x => x.X == colCount - 1 && x.Y == rowCount - 1);

			allTiles = allTiles.Select(t =>
			{
				t.SetDistance(endTile);
				return t;
			}).ToArray();

			List<Tile> activeTiles = new List<Tile>();
			activeTiles.Add(startTile);
			List<Tile> visitedTiles = new List<Tile>();

			var timer = new Stopwatch();
			timer.Start();

			while (activeTiles.Any())
			{
				var checkTile = activeTiles.OrderBy(x => x.Score).First();

				//Console.Write($"{activeTiles.Count}, ");

				if (checkTile.Distance == 0)
				{
					timer.Stop();
					Console.WriteLine(timer.ElapsedMilliseconds);
					return checkTile;
				}

				activeTiles.Remove(checkTile);
				visitedTiles.Add(checkTile);

				var walkableTiles = allTiles
					.Where(x =>
						Math.Abs(x.X - checkTile.X) + Math.Abs(x.Y - checkTile.Y) == 1
						)
					.Select(x => { var newTile = x.Copy(); newTile.PreviousTile = checkTile; return newTile; });

				foreach (var tile in walkableTiles)
				{
					if (visitedTiles.Any(t => t.Equals(tile)))
						continue;

					var existingTile = activeTiles.FirstOrDefault(t => t.Equals(tile));

					if (existingTile != null)
					{
						if (existingTile.Score > tile.Score)
						{
							activeTiles.Remove(existingTile);
							activeTiles.Add(tile);
						}

					}
					else
					{
						activeTiles.Add(tile);
					}
				}
			}

			throw new InvalidOperationException("No path was found");
		}


	}


}
