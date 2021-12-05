using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day2
{
	class Solution
	{
		public static int Part1()
		{
			string[] lines = File.ReadAllLines("Day2/Input.txt");
			Position position = new Position();

			foreach (var line in lines)
			{
				position.Update(line);
			}

			return position.X * position.Y;
		}

		public static int Part2()
		{
			string[] lines = File.ReadAllLines("Day2/Input.txt");
			Position position = new Position();

			foreach (var line in lines)
			{
				position.UpdateWithAim(line);
			}

			return position.X * position.Y;
		}
	}

	public class Position
	{
		public int X { get; private set; } = 0;
		public int Y { get; private set; } = 0;

		private int aim = 0;

		private enum Direction
		{
			Forward,
			Down,
			Up
		}

		public Position Update(string command)
		{
			TryParseCommand(command, out Direction direction, out int distance);

			switch (direction)
			{
				case Direction.Forward:
					X += distance;
					break;
				case Direction.Down:
					Y += distance;
					break;
				case Direction.Up:
					Y -= distance;
					break;
			}

			return this;
		}

		public void UpdateWithAim(string command)
		{
			TryParseCommand(command, out Direction direction, out int distance);

			switch (direction)
			{
				case Direction.Forward:
					X += distance;
					Y += distance * aim;
					break;
				case Direction.Down:
					aim += distance;
					break;
				case Direction.Up:
					aim -= distance;
					break;
			}
		}

		private bool TryParseCommand(string command, out Direction direction, out int distance)
		{
			var commandParts = command.Split(" ");

			if (commandParts.Length != 2)
				throw new ArgumentException($"Unkown command {command}");

			if (!Enum.TryParse(typeof(Direction), commandParts[0], true, out object result))
				throw new ArgumentException($"Unkown direction '{commandParts[0]}'.");

			direction = (Direction)result;

			if (!int.TryParse(commandParts[1], out distance))
				throw new ArgumentException($"Unkown distance '{commandParts[1]}'.");

			return true;
		}
	}
}
