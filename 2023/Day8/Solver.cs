using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day8
{
	internal class Solver
	{
		public static readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day8\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		private InstructionSet InstructionSet { get; }

		private NodeMapping NodeMapping { get; }

		public Solver()
		{
			var lines = ReadInput();

			InstructionSet = new InstructionSet(lines[0]);

			NodeMapping = new NodeMapping(lines.Skip(2).ToArray());
		}

		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day8\\input.txt");

			return lines;
		}

		public string SolvePart1()
		{
			var node = NodeMapping.Nodes.First(n => n.Name == "AAA");

			int steps = 0;

			while (node.Name != "ZZZ")
			{
				node = InstructionSet.Next(node);

				steps++;
			}

			return steps.ToString();
		}


		public string SolvePart2()
		{
			logger.Info("Start");

			var infos = NodeMapping.Nodes.Where(n => n.IsStart).Select(n => InstructionSet.CycleInfo(n)).ToArray();

			var info = infos[0];

			for (int i = 1; i < infos.Length; i++)
			{
				info = info.Join(infos[i]);
			}

			string result = $"{info.Start + info.EndIndices[0]}";
			logger.Info("End");
			return result;
		}



	}

	public class NodeMapping
	{
		public Node[] Nodes { get; }

		public NodeMapping(string[] lines)
		{
			List<Node> nodes = new List<Node>();

			foreach (var line in lines)
			{
				nodes.Add(new Node(line));
			}

			foreach (var node in nodes)
			{
				var line = lines.First(l => l.StartsWith(node.Name));

				node.Link(line, nodes);
			}

			Nodes = nodes.ToArray();
		}
	}

	public class Node
	{
		public string Name { get; }

		public Node? Left { get; set; }

		public Node? Right { get; set; }

		public bool IsStart => Name.EndsWith('A');
		public bool IsEnd => Name.EndsWith('Z');

		public Node(string line)
		{
			Name = line.Split('=')[0].Trim();
		}

		public void Link(string instruction, IEnumerable<Node> nodes)
		{
			var names = instruction.Split('=')[1].Trim().Split(',').Select(s => s.Trim(' ', '(', ')')).ToArray();

			Left = nodes.First(node => node.Name == names[0]);
			Right = nodes.First(node => node.Name == names[1]);
		}

		public override string ToString()
		{
			return $"{Name} = ({Left?.Name}, {Right?.Name})";
		}
	}

	public class InstructionSet
	{
		private readonly Instruction[] instructions;

		private int index = 0;

		public InstructionSet(string line)
		{
			instructions = line.Select(c => new Instruction(c)).ToArray();
		}

		public override string ToString()
		{
			return $"{string.Join("", instructions.Select(i => i.ToString()))}";
		}

		public Node Next(Node node)
		{
			var instruction = instructions[index];

			index = (index + 1) % instructions.Length;

			if (instruction.Symbol == 'L')
				return node.Left ?? throw new InvalidOperationException();
			if (instruction.Symbol == 'R')
				return node.Right ?? throw new InvalidOperationException();

			throw new InvalidOperationException();
		}

		public Node[] Next(Node[] nodes)
		{
			var instruction = instructions[index];

			index = (index + 1) % instructions.Length;

			if (instruction.Symbol == 'L')
				return nodes.Select(n => n.Left ?? throw new InvalidOperationException()).ToArray();
			if (instruction.Symbol == 'R')
				return nodes.Select(n => n.Right ?? throw new InvalidOperationException()).ToArray();

			throw new InvalidOperationException();
		}

		private void Reset()
		{
			index = 0;
		}

		public CycleInfo CycleInfo(Node node)
		{
			int _indexCache = index;

			Reset();

			List<HistoryEntry> history = new List<HistoryEntry>();


			while (true)
			{
				history.Add(new HistoryEntry(index, node));

				node = Next(node);

				var nextEntry = new HistoryEntry(index, node);

				if (history.Contains(nextEntry))
				{
					history.Add(nextEntry);
					break;
				}
			}

			index = _indexCache;

			return new CycleInfo(history);
		}
	}

	public class HistoryEntry
	{
		public Node Node { get; set; }

		public int Index { get; set; }

		public HistoryEntry(int index, Node node)
		{
			Node = node;
			Index = index;
		}

		public override bool Equals(object? obj)
		{
			var other = obj as HistoryEntry ?? throw new InvalidOperationException();

			return this.Node == other.Node && this.Index == other.Index;
		}

		public override int GetHashCode()
		{
			return this.Node.GetHashCode() + this.Index.GetHashCode();
		}

		public override string ToString()
		{
			return $"{Index} {Node}";
		}
	}

	public class CycleInfo
	{
		public int Start { get; set; }

		public long Length { get; set; }

		public long[] EndIndices { get; set; } = new long[0];

		public CycleInfo(List<HistoryEntry> history)
		{
			var duplicateName = history.Last();

			Start = history.IndexOf(duplicateName);

			Length = history.Count - Start - 1;

			EndIndices = history.Where(name => name.Node.IsEnd).Select(n => (long)history.IndexOf(n) - Start).ToArray();
		}

		public CycleInfo()
		{

		}

		public CycleInfo Translate(int newStart)
		{
			return new CycleInfo()
			{
				Length = Length,
				Start = newStart,
				EndIndices = EndIndices.Select(i =>
				{
					var d = Math.Abs(Start - newStart);

					return (i + (Length - d)) % Length;
				}).ToArray()
			};
		}

		public CycleInfo Join(CycleInfo other, bool log = false)
		{
			var left = this;
			var right = other;

			if(left.Start > right.Start)
				right = right.Translate(left.Start);
			if (left.Start < right.Start)
				left = left.Translate(right.Start);

			var newLength = determineLCM(left.Length, right.Length);

			var thisExpansion = left.ExpandIndeces(newLength);
			var otherExpansion = right.ExpandIndeces(newLength);

			List<long> newIndeces = new List<long>();

			int stepSize = 1000000;

			for (int i = 0; i < otherExpansion.Length; i++)
			{
				if (thisExpansion.Contains(otherExpansion[i]))
				{
					newIndeces.Add(otherExpansion[i]);
				}

				if(log && i % stepSize == 0)
				{
					Solver.logger.Debug($"{i}/{otherExpansion.Length}");
				}
			}

			return new CycleInfo()
			{
				Start = left.Start,
				Length = newLength,
				EndIndices = newIndeces.ToArray()
			};


		}

		public long[] ExpandIndeces(long until)
		{
			return EndIndices.SelectMany(i => ExpandIndex(i, until)).ToArray();
		}

		private long[] ExpandIndex(long index, long until)
		{
			var list = new List<long>();

			long value = index;

			while(value < until)
			{
				list.Add(value);
				value += Length;
			}

			return list.ToArray();
		}

		private static long determineLCM(long a, long b)
		{
			long num1, num2;
			if (a > b)
			{
				num1 = a; num2 = b;
			}
			else
			{
				num1 = b; num2 = a;
			}

			for (int i = 1; i < num2; i++)
			{
				long mult = num1 * i;
				if (mult % num2 == 0)
				{
					return mult;
				}
			}
			return num1 * num2;
		}

	}

	public class Instruction
	{
		public char Symbol { get; }

		public int Index { get; }

		public Instruction(char c)
		{
			Symbol = c;

			if (c == 'L')
			{
				Index = 0;
			}
			else if (c == 'R')
			{
				Index = 1;
			}
			else
			{
				throw new InvalidOperationException($"Unkown Instruction: '{c}'");
			}

		}

		public override string ToString()
		{
			return Symbol.ToString();
		}
	}

}
