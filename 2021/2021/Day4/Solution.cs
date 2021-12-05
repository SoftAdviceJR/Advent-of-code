using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Submarine.Helpers;

namespace Submarine.Day4
{
	static class Solution
	{
		public static long Part1()
		{
			var lines = File.ReadAllLines("Day4/Input.txt");

			var numbersToCall = lines[0];

			List<BingoBoard> boards = new List<BingoBoard>();

			for (int i = 2; i < lines.Length - 4; i += 6)
			{
				boards.Add(new BingoBoard(lines.Skip(i).Take(5).ToArray()));
			}

			BingoBoard winningBoard = null;
			int lastNumber = 0;

			foreach (int calledNumber in numbersToCall.Split(",").Select(x => int.Parse(x)))
			{
				lastNumber = calledNumber;
				foreach (var board in boards)
				{
					board.MarkNumber(calledNumber);
				}

				winningBoard = boards.FirstOrDefault(b => b.HasCompletedLine());

				if (winningBoard != null)
					break;
			}


			return winningBoard?.Score() * lastNumber ?? 0;
		}

		public static long Part2()
		{
			var lines = File.ReadAllLines("Day4/Input.txt");

			var numbersToCall = lines[0];

			List<BingoBoard> boards = new List<BingoBoard>();

			for (int i = 2; i < lines.Length - 4; i += 6)
			{
				boards.Add(new BingoBoard(lines.Skip(i).Take(5).ToArray()));
			}

			int lastNumber = 0;

			foreach (int calledNumber in numbersToCall.Split(",").Select(x => int.Parse(x)))
			{
				lastNumber = calledNumber;

				foreach (var board in boards)
				{
					board.MarkNumber(calledNumber);
				}

				if (boards.Count == 1 && boards.First().HasCompletedLine())
					break;

				boards = boards.Where(board => !board.HasCompletedLine()).ToList();
			}

			return boards.First().Score() * lastNumber;
			
		}

	}

	class BingoBoard
	{
		private readonly int rowCount;
		private readonly int colCount;

		private readonly BingoCell[,] cells;
		

		public BingoBoard(string[] lines)
		{
			colCount = lines.Length;
			rowCount = lines.First().Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Count();

			cells = new BingoCell[rowCount, colCount];

			for (int i = 0; i < rowCount; i++)
			{
				var lineInput = lines[i].Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

				for (int j = 0; j < colCount; j++)
				{
					cells[i,j] = new BingoCell(int.Parse(lineInput[j]));
				}
			}
		}

		public void MarkNumber(int input)
		{
			foreach (var cell in cells)
			{
				cell.Mark(input);
			}
		}

		public bool HasCompletedLine()
		{
			for (int i = 0; i < rowCount; i++)
			{
				if (cells.GetRow(i).All(cell => cell.Marked))
					return true;
			}

			for (int i = 0; i < colCount; i++)
			{
				if (cells.GetColumn(i).All(cell => cell.Marked))
					return true;
			}

			return false;
		}

		public void MarkNumber(string input)
		{
			MarkNumber(int.Parse(input));
		}

		public int Score()
		{
			int score = 0;
			foreach (var cell in cells)
			{
				score += cell.Score;
			}

			return score;
		}
	}

	class BingoCell
	{
		public int Value { get;  }
		public bool Marked { get; private set; } = false;

		public int Score { get => Marked ? 0 : Value; }

		public BingoCell(int value)
		{
			Value = value;
		}

		public void Mark(int value)
		{
			if (value == Value)
				Marked = true;
		}
	}

	
}
