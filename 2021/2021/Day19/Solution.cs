using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using MathNet.Spatial.Euclidean;

using Newtonsoft.Json;

using Submarine.Day2;
using Submarine.Helpers;

namespace Submarine.Day19
{
	static class Solution
	{
		private static string[] ReadInput()
		{
			var lines = File.ReadAllLines("Day19/Input.txt");

			return lines;
		}



		public static decimal Part1()
		{
			var lines = ReadInput();

			List<Scanner> scanners = new List<Scanner>();

			Scanner scanner = null;

			for (int i = 0; i < lines.Length; i++)
			{
				if (scanner == null)
				{
					scanner = new Scanner(lines[i]);
				}
				else if (string.IsNullOrWhiteSpace(lines[i]))
				{
					scanners.Add(scanner.Copy());
					scanner = null;
				}
				else
				{
					scanner.AddPoint(lines[i]);
				}
			}

			scanners.Add(scanner);

			List<Point3D> beacons = new List<Point3D>();

			beacons.AddRange(scanners[0].Beacons);
			scanners.RemoveAt(0);


			while (scanners.Any())
			{
				for (int i = 0; i < scanners.Count; i++)
				{
					if (scanners[i].TryMatchBeacons(beacons, out Point3D[] transformedBeacons, out Vector3D position))
					{
						beacons.AddRange(transformedBeacons);
						beacons = beacons.Distinct().ToList();
						scanners.RemoveAt(i);
						break;
					}

				}
			}




			return beacons.Count;
		}

		public static long Part2()
		{
			var lines = ReadInput();

			List<Scanner> scanners = new List<Scanner>();

			Scanner scanner = null;

			for (int i = 0; i < lines.Length; i++)
			{
				if (scanner == null)
				{
					scanner = new Scanner(lines[i]);
				}
				else if (string.IsNullOrWhiteSpace(lines[i]))
				{
					scanners.Add(scanner.Copy());
					scanner = null;
				}
				else
				{
					scanner.AddPoint(lines[i]);
				}
			}

			scanners.Add(scanner);

			List<Point3D> beacons = new List<Point3D>();

			beacons.AddRange(scanners[0].Beacons);
			scanners.RemoveAt(0);

			var positions = new List<Vector3D>();

			while (scanners.Any())
			{
				for (int i = 0; i < scanners.Count; i++)
				{
					if (scanners[i].TryMatchBeacons(beacons, out Point3D[] transformedBeacons, out Vector3D position))
					{
						beacons.AddRange(transformedBeacons);
						beacons = beacons.Distinct().ToList();
						scanners.RemoveAt(i);
						positions.Add(position);
						break;
					}

				}
			}

			return positions.Select(p1 => positions.Max(p2 => ManDist(p1, p2))).Max();

		}

		private static int ManDist(Vector3D p1, Vector3D p2)
		{
			return (int)(Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z));
		}
	}


}
