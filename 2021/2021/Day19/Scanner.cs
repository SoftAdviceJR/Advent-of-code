using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Spatial;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;

using Newtonsoft.Json;

namespace Submarine.Day19
{
	class Scanner
	{
		public int ID { get; }

		public Point3D[] Beacons { get => _beacons.ToArray(); }

		private readonly List<Point3D> _beacons = new List<Point3D>();

		public Scanner(string name)
		{
			ID = int.Parse(name.Split(' ')[2]);
		}

		private Scanner(Point3D[] beacons, int id)
		{
			ID = id;
			_beacons = beacons.ToList();
		}

		public void AddPoint(string point)
		{
			var points = point.Split(',');
			_beacons.Add(new Point3D(int.Parse(points[0]), int.Parse(points[1]), int.Parse(points[2])));
		}

		public Scanner Copy()
		{
			return new Scanner(Beacons, ID);
		}

		public Scanner Translate(Vector3D translation)
		{
			var beacons = _beacons.Select(b =>
				 new Point3D(
					Math.Round(b.X + translation.X),
					Math.Round(b.Y + translation.Y),
					Math.Round(b.Z + translation.Z)
					)
			).ToArray();

			return new Scanner(beacons, ID);
		}

		public Scanner Rotate90(Vector3D axis)
		{
			var copy = Copy();

			for (int i = 0; i < _beacons.Count; i++)
			{
				_beacons[i] = _beacons[i].Rotate(axis, Angle.FromDegrees(90));
			}

			return copy;
		}

		public bool TryMatchBeacons(IEnumerable<Point3D> beacons, out Point3D[] transformedBeacons, out Vector3D translation)
		{
			if(TryMatch(beacons, out transformedBeacons, out translation))
			{
				return true;
			}

			for (int r = 0; r < Rotations().Length; r++)
			{
				var rotation = Rotations()[r];
				if (Rotate90(rotation).TryMatch(beacons, out transformedBeacons, out translation))
					return true;
			}

			transformedBeacons = null;
			return false;
		}

		private bool TryMatch(IEnumerable<Point3D> beacons, out Point3D[] transformedBeacons, out Vector3D translation)
		{
			for (int i = 0; i < Beacons.Length; i++)
			{
				var beacon = Beacons[i];

				for (int j = 0; j < beacons.Count(); j++)
				{
					var target = beacons.ElementAt(j);

					translation = beacon.VectorTo(target);

					var translated = Translate(translation);

					if (translated.TwelveBeaconsMatch(beacons))
					{
						transformedBeacons = translated.Beacons;
						
						return true;
					}
				}
			}

			transformedBeacons = null;
			translation = Vector3D.NaN;
			return false;
		}


		public bool TwelveBeaconsMatch(IEnumerable<Point3D> beacons)
		{
			var matches = _beacons.Where(b => beacons.Contains(b));
			if(matches.Count() > 1)
			{

			}
			return matches.Count() >= 12;
		}


		private static Vector3D[] Rotations()
		{
			return new Vector3D[]
			{
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),

				new Vector3D(0,0,1),

				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),

				new Vector3D(0,0,1),

				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),

				new Vector3D(0,0,-1),

				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),

				new Vector3D(0,0,-1),

				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),

				new Vector3D(0,0,1),

				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
				new Vector3D(1,0,0),
			};
		}
	}
}
