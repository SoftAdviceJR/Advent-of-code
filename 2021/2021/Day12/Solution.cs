using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Submarine.Helpers;

namespace Submarine.Day12
{
	static class Solution
	{
		private static Connection[] ReadInput()
		{
			var lines = File.ReadAllLines("Day12/Input.txt");

			return lines.Select(ln => new Connection(ln)).ToArray();
		}

		public static decimal Part1()
		{
			var connections = ReadInput();

			var start = new Cave("start");
			var end = new Cave("end");

			List<Path> paths = new List<Path>() {
				new Path(start)
			};

			while (!paths.All(p => p.GetPosition().Equals(end)))
			{
				List<Path> newPaths = new List<Path>();
				foreach (var path in paths)
				{
					if (end.Equals(path.GetPosition()))
					{
						newPaths.Add(path);
						continue;
					}

					var validConnections = connections.Where(c => c.ConnectsTo(path.GetPosition()));

					foreach (var connection in validConnections)
					{
						var destination = connection.MoveFrom(path.GetPosition());

						if (path.HasVisited(destination) && destination.IsSmall)
							continue;
						Path newPath = path.Clone();
						newPath.MoveTo(destination);
						newPaths.Add(newPath);
					}
				}

				paths = newPaths;
			}


			return paths.Count;
		}

		public static long Part2()
		{
			var connections = ReadInput();

			var start = new Cave("start");
			var end = new Cave("end");

			List<Path> paths = new List<Path>() {
				new Path(start)
			};

			while (!paths.All(p => p.GetPosition().Equals(end)))
			{
				List<Path> newPaths = new List<Path>();
				foreach (var path in paths)
				{
					if (end.Equals(path.GetPosition()))
					{
						newPaths.Add(path);
						continue;
					}

					var validConnections = connections.Where(c => c.ConnectsTo(path.GetPosition()));

					foreach (var connection in validConnections)
					{
						var destination = connection.MoveFrom(path.GetPosition());

						if (start.Equals(destination))
							continue;

						if (path.HasVisited(destination) && destination.IsSmall)
							if (path.VisitedASmallCaveMoreThanOnce())
								continue;
						Path newPath = path.Clone();
						newPath.MoveTo(destination);
						newPaths.Add(newPath);
					}

					paths = newPaths;
				}
			}

			return paths.Count;
		}
	}


}
