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

namespace Submarine.Day16
{
	static class Solution
	{
		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("Day16/Input.txt");

			return lines.ToArray();
		}

		public static decimal Part1()
		{
			var lines = ReadInput();

			var packet = BITSDecoder.Decode(lines[0]);

			return SumVersions(packet);
		}

		public static long Part2()
		{

			var lines = ReadInput();

			var packet = BITSDecoder.Decode(lines[0]);

			return packet.Value;
		}

		private static int SumVersions(Packet packet)
		{
			return packet.Version + packet.SubPackets.Sum(p => SumVersions(p));
		}
	}


}
