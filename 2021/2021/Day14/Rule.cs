using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day14
{
	class Rule
	{
		public string Pair { get; }
		public char Insert { get; }

		public Rule(string line)
		{
			var inputs = line.Split("->");
			Pair = inputs[0].Trim();
			Insert = inputs[1].Trim()[0];
		}
	}
}
