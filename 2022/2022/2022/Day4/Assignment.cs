using System;

namespace _2022.Day4
{
	public class Assignment
	{
		private readonly int start;
		private readonly int stop;

		public Assignment(int start, int stop)
		{
			if (stop < start)
				throw new ArgumentException($"Invalid start-stop for assignment: {start}-{stop}");

			this.start = start;
			this.stop = stop;
		}

		public bool Contains(Assignment other)
		{
			return this.start <= other.start && this.stop >= other.stop;
		}

		public bool OverlapsWith(Assignment other)
		{
			return (this.start <= other.start && this.stop >= other.start) || (other.start <= this.start && other.stop >= this.start);
		}
	}
		
}
