using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day16
{
	public class Packet
	{
		public enum PacketType
		{
			Literal,
			Sum,
			Product,
			Minimum,
			Maximum,
			GreaterThan,
			LessThan,
			Equal
		}

		public int Version { get; set; }

		public int TypeId { get; set; }

		public PacketType Type { get; }

		private long _value;

		public long Value { get => CalculateValue(); }

		public Packet[] SubPackets { get; private set; } = new Packet[0];

		private Packet(string binary)
		{
			Version = (int)ConvertBinary(binary.Take(3));
			TypeId = (int)ConvertBinary(binary.Skip(3).Take(3));

			switch (TypeId)
			{
				case 0:
					Type = PacketType.Sum;
					break;
				case 1:
					Type = PacketType.Product;
					break;
				case 2:
					Type = PacketType.Minimum;
					break;
				case 3:
					Type = PacketType.Maximum;
					break;
				case 4:
					Type = PacketType.Literal;
					break;
				case 5:
					Type = PacketType.GreaterThan;
					break;
				case 6:
					Type = PacketType.LessThan;
					break;
				case 7:
					Type = PacketType.Equal;
					break;
				default:
					throw new InvalidOperationException("Unknown Type ID: " + TypeId);
			}
		}

		public static Packet Parse(string binary, out string remaining)
		{
			var packet = new Packet(binary);

			if (packet.Type == PacketType.Literal)
			{
				remaining = packet.ParseLiteral(binary);
			}
			else
			{
				remaining = packet.ParseOperator(binary);
			}

			return packet;
		}

		private string ParseLiteral(string binary)
		{
			int skip = 6;
			List<char> value = new List<char>();

			while (true)
			{
				var group = binary.Skip(skip);
				value.AddRange(group.Skip(1).Take(4));
				skip += 5;
				if (group.First() == '0')
					break;

			}

			_value = ConvertBinary(value);

			return string.Concat(binary.Skip(skip));
		}

		private string ParseOperator(string binary)
		{
			if (binary[6] == '0')
			{
				long length = ConvertBinary(binary.Skip(7).Take(15));
				string remaining = string.Concat(binary.Skip(22));
				return ParseOperatorByLength(remaining, length);
			}
			else
			{
				long groups = ConvertBinary(binary.Skip(7).Take(11));
				string remaining = string.Concat(binary.Skip(18));
				return ParseOperatorByGroups(remaining, groups);
			}
		}

		private string ParseOperatorByLength(string binary, long length)
		{
			List<Packet> subPackets = new List<Packet>();

			string remaining = binary;

			long parsedLength = 0;

			while (true)
			{
				subPackets.Add(Parse(remaining, out string afterParse));

				parsedLength += remaining.Length - afterParse.Length;
				remaining = afterParse;

				if (parsedLength == length)
					break;
				if (parsedLength > length)
					throw new InvalidOperationException("Unexpected length of parsed characters");
			}

			SubPackets = subPackets.ToArray();

			return remaining;
		}

		private string ParseOperatorByGroups(string binary, long groups)
		{
			List<Packet> subPackets = new List<Packet>();

			string remaining = binary;

			for (long i = 0; i < groups; i++)
			{
				subPackets.Add(Parse(remaining, out remaining));
			}

			SubPackets = subPackets.ToArray();

			return remaining;
		}

		private long CalculateValue()
		{
			switch (Type)
			{
				case PacketType.Literal:
					return _value;
				case PacketType.Sum:
					return SubPackets.Sum(x => x.Value);
				case PacketType.Product:
					long prod = 1;
					foreach (var p in SubPackets)
					{
						prod *= p.Value;
					}
					return prod;
				case PacketType.Minimum:
					return SubPackets.Min(p => p.Value);
				case PacketType.Maximum:
					return SubPackets.Max(p => p.Value);
				case PacketType.GreaterThan:
					if (SubPackets[0].Value > SubPackets[1].Value)
						return 1;
					return 0;
				case PacketType.LessThan:
					if (SubPackets[0].Value < SubPackets[1].Value)
						return 1;
					return 0;
				case PacketType.Equal:
					if (SubPackets[0].Value == SubPackets[1].Value)
						return 1;
					return 0;
				default:
					throw new InvalidOperationException("Unkown Packet Type " + Type.ToString());
			}
		}

		private static long ConvertBinary(IEnumerable<char> input)
		{
			return Convert.ToInt64(string.Concat(input), 2);
		}
	}
}
