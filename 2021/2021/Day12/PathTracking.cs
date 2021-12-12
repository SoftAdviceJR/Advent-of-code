using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day12
{
	class Connection
	{
		private readonly Cave start;
		private readonly Cave end;

		public Connection(string line)
		{
			var caves = line.Split('-');
			start = new Cave(caves[0]);
			end = new Cave(caves[1]);
		}

		public Cave MoveFrom(Cave start)
		{
			if (start.Equals(this.start))
				return this.end;
			if (start.Equals(this.end))
				return this.start;
			return null;
		}

		public bool ConnectsTo(Cave start)
		{
			return start.Equals(this.start) || start.Equals(this.end);
		}
	}

	class Cave
	{
		public string Name { get; }
		public bool IsSmall { get => Name.ToLower() == Name; }

		public Cave(string name)
		{
			Name = name;
		}

		public Cave(Cave cave)
		{
			this.Name = cave.Name;
		}

		public bool Equals(Cave cave)
		{
			return Name == cave.Name;
		}
		public override bool Equals(object obj)
		{
			Cave cave = obj as Cave;
			if (cave != null)
				return this.Equals(cave);

			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}

	class Path
	{
		private List<Cave> visitedCaves;

		public Path(Cave start)
		{
			visitedCaves = new List<Cave>() { start };
		}

		private Path()
		{
			visitedCaves = new List<Cave>();
		}

		public Cave GetPosition()
		{
			return visitedCaves.Last();
		}

		public void MoveTo(Cave cave)
		{
			visitedCaves.Add(cave);
		}

		public bool HasVisited(Cave cave)
		{
			return visitedCaves.Contains(cave);
		}

		public bool VisitedASmallCaveMoreThanOnce()
		{
			return visitedCaves.Where(c => c.IsSmall).GroupBy(c => c.Name).Any(g => g.Count() > 1);
		}

		public Path Clone()
		{
			var newPath = new Path();
			visitedCaves.ForEach(c => newPath.visitedCaves.Add(new Cave(c)));
			return newPath;
		}
	}
}
