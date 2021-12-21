using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day21
{
	struct Player
	{
		public int ID { get; private set; }

		public int Position { get; private set; }

		public long Score { get; private set; }

		public Player(string start)
		{
			var parts = start.Split(' ');
			ID = int.Parse(parts[1]);
			Position = int.Parse(parts[4]);
			Score = 0;
		}

		public void Move(int moves)
		{
			Position = (Position - 1 + moves) % 10 + 1;
			Score += Position;
		}
	}
}
