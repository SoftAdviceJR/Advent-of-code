using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day8
{
	class Code
	{
		public string[] Signals { get; }
		public string[] Output { get; }

		public Code(string input)
		{
			bool passedSeparator = false;
			var signals = new List<string>();
			var output = new List<string>();
			foreach (var digit in input.Split(" "))
			{
				if(digit == "|")
				{
					passedSeparator = true;
					continue;
				}

				if(!passedSeparator)
				{
					signals.Add(digit);
				}
				else
				{
					output.Add(digit);
				}
			}

			Signals = signals.ToArray();
			Output = output.ToArray();
		}
	}
}
