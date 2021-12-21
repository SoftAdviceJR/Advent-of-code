using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day21
{
	struct Universe
	{
		public int Count { get; set; }

		public Player[] Players { get; set; }

		public Universe(Player[] players)
		{
			this.Players = players;
			Count = 1;
		}

	}
}
