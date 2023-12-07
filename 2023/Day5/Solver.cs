using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NLog;

namespace _2023.Day5
{
	internal class Solver
	{
		private readonly Logger logger = LogManager.Setup().LoadConfiguration(builder =>
			{
				builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\temp\\aoc\\Day5\\{DateTime.Now:yyMMdd}.log");
			}).GetCurrentClassLogger();

		public Solver()
		{
			var lines = File.ReadAllLines("C:\\Users\\jeroen\\source\\repos\\Advent-of-code\\2023\\Day5\\input.txt");

			var mappings = new List<TypeMapping>();

			var mappingLines = new List<string>();

			for (int i = 2; i < lines.Length; i++)
			{
				var line = lines[i];
				if (string.IsNullOrWhiteSpace(line))
				{
					mappings.Add(TypeMapping.Create(mappingLines.ToArray()));
					mappingLines.Clear();
				}
				else
				{
					mappingLines.Add(line);
				}

			}

			mappings.Add(TypeMapping.Create(mappingLines.ToArray()));

			Mappings = mappings.ToArray();

			Seeds = lines[0]
				.Split(':')[1]
				.Trim().Split(' ')
				.Select(n => long.Parse(n))
				.Select(n => new TypedValue() { Value = n, Type = "seed" })
				.ToArray();


		}

		private TypedValue[] Seeds { get; }

		private TypeMapping[] Mappings { get; }


		public string SolvePart1()
		{
			List<TypedValue> results = new List<TypedValue>();

			foreach (var seed in Seeds)
			{
				results.Add(MapTo(seed, "location"));
			}

			return results.Min(r => r.Value).ToString();
		}

		private TypedValue MapTo(TypedValue input, string type)
		{
			var output = input;

			while (output.Type != type)
			{
				var mapping = Mappings.First(m => m.From == output.Type);

				output = mapping.Map(output);
			}

			return output;
		}


		public string SolvePart2()
		{
			var seedRanges = new List<Range>();

			for (int i = 0; i < Seeds.Length; i += 2)
			{
				seedRanges.Add(new Range(Seeds[i].Value, Seeds[i].Value + Seeds[i + 1].Value - 1));
			}

			seedRanges = seedRanges.OrderBy(r => r.Start).ToList();

			Console.WriteLine($"seeds");

			foreach (var range in seedRanges)
			{
				Console.WriteLine(range);
			}

			Console.WriteLine();

			string type = "seed";

			var output = seedRanges;

			while (type != "location")
			{
				var mapping = Mappings.First(m => m.From == type);

				output = mapping.MapRanges(output);

				Console.WriteLine($"{type} -> {mapping.To}");

				foreach (var range in output)
				{
					Console.WriteLine(range);
				}

				Console.WriteLine();

				type = mapping.To;


			}

			return output.Min(r => r.Start).ToString();
		}



	}

	public struct TypedValue
	{
		public string Type { get; set; }

		public long Value { get; set; }

		public override string ToString()
		{
			return $"{Type} | {Value}";
		}
	}

	public class TypeMapping
	{
		public string From { get; }

		public string To { get; }

		private RangeMapping[] ranges;

		private RangeMapping[] Ranges => ranges.OrderBy(r => r.From.Start).ToArray();

		public static TypeMapping Create(string[] lines)
		{
			return new TypeMapping(lines);
		}

		private TypeMapping(string[] lines)
		{
			{
				string title = lines[0];

				var parts = title.Split(' ')[0].Split('-');

				From = parts[0];
				To = parts[2];
			}


			var ranges = new List<RangeMapping>();

			for (int i = 1; i < lines.Length; i++)
			{
				string line = lines[i];
				var numbers = line.Split(' ').Select(n => long.Parse(n)).ToArray();

				long destinationStart = numbers[0];
				long sourceStart = numbers[1];
				long length = numbers[2] - 1;

				ranges.Add(new RangeMapping()
				{
					From = new Range(sourceStart, sourceStart + length ),
					To = new Range(destinationStart, destinationStart + length)
				});
			}

			ranges = ranges.OrderBy(r => r.From.Start).ToList();

			long start = 0;

			var finalRanges = new List<RangeMapping>();

			for (int i = 0; i < ranges.Count; i++)
			{
				if (start < ranges[i].From.Start)
				{
					var newRange = new Range(start, ranges[i].From.Start - 1);
					finalRanges.Add(new RangeMapping()
					{
						From = newRange,
						To = newRange
					});

					start = ranges[i].From.End + 1;
				}

				finalRanges.Add(ranges[i]);
			}

			this.ranges = finalRanges.ToArray();
		}

		public List<Range> MapRanges(IEnumerable<Range> ranges)
		{
			return ranges.SelectMany(r => MapRange(r)).ToList();

		}

		public List<Range> MapRange(Range range)
		{
			var results = new List<Range>();

			long start = range.Start;


			for (int i = 0; i < Ranges.Length; i++)
			{
				var rangeMapping = Ranges[i];

				if (!rangeMapping.From.Contains(start))
					continue;

				long end = rangeMapping.From.End;

				if (rangeMapping.From.Contains(range.End))
					end = range.End;

				results.Add(new Range(Map(start), Map(end)));

				start = end + 1;
			}

			if(start < range.End)
			{
				results.Add(new Range(Map(start), Map(range.End)));
			}

			return results;
		}


		public long Map(long input)
		{
			foreach (var range in Ranges)
			{
				if (range.From.Contains(input))
				{
					return range.Map(input);
				}
			}
			return input;
		}


		public TypedValue Map(TypedValue input)
		{
			if (input.Type != From)
				throw new InvalidOperationException($"Map is from {From} to {To} and input is of type {input.Type}");

			return new TypedValue() { Type = To, Value = Map(input.Value) };

		}

		public override string ToString()
		{
			return $"{From}-to-{To}";
		}
	}

	public class RangeMapping
	{
		public Range From { get; set; }
		public Range To { get; set; }

		public long Map(long number)
		{
			if (!From.Contains(number))
				throw new InvalidOperationException($"{From} does not contain {number}");

			return To.Start + number - From.Start;
		}

		public override string ToString()
		{
			return $"{From} -> {To}";
		}
	}


	public readonly struct Range
	{
		public long Start { get; }
		public long End { get; }

		public long Length => End - Start + 1;

		public Range(long start, long end)
		{
			this.Start = start;
			this.End = end;
		}

		public bool Contains(long number)
		{
			return Start <= number && End >= number;
		}

		public override string ToString()
		{
			return $"[{Start} - {End}]";
		}
	}
}
