using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day15
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day15\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
		}

		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day15\\input.txt");

			return lines[0].Split(',');

		}

		public string SolvePart1()
		{
			var instructions = ReadInput();

			long result = 0;

			foreach ( var instruction in instructions )
			{
				var hash = AocConverter.Hash(instruction);
                Console.WriteLine($"{instruction} -> {hash}");

                result += hash;
			}

			return result.ToString();

		}


		public string SolvePart2()
		{
			var instructions = ReadInput();

			Box[] boxes = new Box[256];

			for (int i = 0; i < boxes.Length; i++)
			{
				boxes[i] = new Box();
			}

			foreach (var instruction in instructions)
			{
				string label = Box.GetLabel(instruction);

				int index = AocConverter.Hash(label);

				boxes[index].Execute(instruction);
			}

			long result = 0;

			for (int i = 0; i < boxes.Length; i++)
			{
				result += (i + 1) * boxes[i].FocusPower;
			}

			return result.ToString();
		}

	}

	public static class AocConverter
	{
		public static int Hash(string input) 
		{ 
			int hash = 0;

			foreach (var c in input)
			{
				int value = Convert.ToInt32(c);

				hash += value;
				hash *= 17;
				hash = hash % 256;
			}

			return hash;
		}
	}

	public class Box
	{
		private OrderedDictionary Lenses { get; } = new OrderedDictionary();

		public void Execute(string instruction)
		{
			if(instruction.Contains('='))
			{
				var parts = instruction.Split('=');

				AddLens(parts[0], int.Parse(parts[1]));
			}
			else if(instruction.Contains('-'))
			{
				RemoveLens(instruction.Substring(0, instruction.Length - 1));
			}
		}

		private void AddLens(string input, int value)
		{
			if(Lenses.Contains(input))
				Lenses[input] = value;

			else
				Lenses.Add(input, value);
		}

		private void RemoveLens(string input)
		{
			Lenses.Remove(input);
		}

		public static string GetLabel(string instruction)
		{
			return instruction.Split('=')[0].Split('-')[0];
		}

		public int FocusPower
		{
			get
			{
				int i = 1;
				int power = 0;

				foreach (var key in Lenses.Keys)
				{
					power += i * (int)Lenses[key];

					i++;
				}

				return power;
			}
		}

		public override string ToString()
		{
			List<string> lenses = new List<string>();

			foreach (var key in Lenses.Keys)
			{
				lenses.Add($"[{key} {Lenses[key]}]");
			}

			return string.Join(" ", lenses);
		}
	}


}
