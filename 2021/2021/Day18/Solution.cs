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

namespace Submarine.Day18
{
	static class Solution
	{
		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("Day18/Input.txt");

			return lines;
		}

		public static decimal Part1()
		{
			var lines = ReadInput();

			var solution = Node.Parse(lines[0]);

			solution.Reduce();

			for (int i = 1; i < lines.Length; i++)
			{
				Node nextNode = Node.Parse(lines[i]);
				solution = Node.Add(solution, nextNode);
				solution.Reduce();
			}

			Console.WriteLine(solution.ToString());

			return solution.Magnitude;
		}

		public static long Part2()
		{
			var lines = ReadInput();

			Node maxLeftNode = Node.Parse("0");
			Node maxRightNode = Node.Parse("0");
			long maxMagnitude = 0;

			for (int i = 0; i < lines.Length - 1; i++)
			{
				for (int j = i + 1; j < lines.Length; j++)
				{
					var leftNode = Node.Parse(lines[i]);
					var rightNode = Node.Parse(lines[j]);

					var sum = Node.Add(leftNode, rightNode);
					sum.Reduce();
					long mag = sum.Magnitude;
					if(mag > maxMagnitude)
					{
						maxMagnitude = mag;
						maxLeftNode = Node.Parse(lines[i]);
						maxRightNode = Node.Parse(lines[j]);
					}

					leftNode = Node.Parse(lines[i]);
					rightNode = Node.Parse(lines[j]);
					var inverseSum = Node.Add(rightNode, leftNode);
					inverseSum.Reduce();
					mag = inverseSum.Magnitude;
					if (mag > maxMagnitude)
					{
						maxMagnitude = mag;
						maxLeftNode = Node.Parse(lines[j]);
						maxRightNode = Node.Parse(lines[i]);
					}
				}
			}

			var maxSum = Node.Add(maxLeftNode, maxRightNode);

			Console.WriteLine(maxLeftNode.ToString() + " +");
			Console.WriteLine(maxRightNode.ToString() );
			Console.WriteLine("=");
			Console.WriteLine(maxSum.ToString());
			maxSum.Reduce();
			Console.WriteLine(maxSum.ToString());

			return maxSum.Magnitude;
		}

		

		
	}


}
