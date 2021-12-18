using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submarine.Day18
{
	class Node
	{
		public Node LeftNode { get; set; }
		public Node RightNode { get; set; }
		public Node Parent { get; set; }

		public long Magnitude
		{
			get
			{
				if (Value.HasValue)
					return Value.Value;
				else
				{
					return 3 * LeftNode.Magnitude + 2 * RightNode.Magnitude;
				}
			}
		}

		public Node Root
		{
			get => Parent == null ? this : Parent.Root;
		}

		public int Depth
		{
			get
			{
				if (Parent == null)
					return 0;
				return 1 + Parent.Depth;
			}
		}

		public bool ShouldExplode
		{
			get => Depth >= 4 && LeftNode.Value.HasValue && RightNode.Value.HasValue;

		}

		public bool ShouldSplit
		{
			get => Value.HasValue && Value >= 10;
		}

		public int? Value { get; set; } = null;

		private Node()
		{

		}

		public static Node Parse(string input, Node parent = null)
		{
			var root = new Node();
			root.Parent = parent;

			if (input[0] == '[')
			{
				var nodes = SplitLeftRight(input);

				root.LeftNode = Node.Parse(nodes[0], root);
				root.RightNode = Node.Parse(nodes[1], root);
			}
			else
			{
				root.Value = int.Parse(input);
			}

			return root;
		}

		public static Node Add(Node left, Node right)
		{
			var newRoot = new Node();
			left.Parent = newRoot;
			right.Parent = newRoot;

			newRoot.LeftNode = left;
			newRoot.RightNode = right;

			return newRoot;
		}

		private static string[] SplitLeftRight(string input)
		{
			input = string.Concat(input.Skip(1).Take(input.Length - 2));

			string leftPart = "";
			string rightPart = "";
			int level = 0;
			bool readingLeft = true;

			for (int i = 0; i < input.Length; i++)
			{
				char ch = input[i];

				if (level == 0 && ch == ',')
				{
					readingLeft = false;
					continue;
				}

				if (readingLeft)
					leftPart += ch;
				else
					rightPart += ch;

				if (ch == '[')
				{
					level++;
				}
				else if (ch == ']')
				{
					level--;
				}
			}

			return new string[] { leftPart, rightPart };
		}

		public void Reduce()
		{
			while (true)
			{
				var valueNodes = Flatten();
				var explodeNode = valueNodes.Select(node => node.Parent).FirstOrDefault(node => node.ShouldExplode);
				if (explodeNode != null)
				{
					explodeNode.Explode();
					continue;
				}

				var splitNode = valueNodes.FirstOrDefault(node => node.ShouldSplit);
				if (splitNode != null)
				{
					splitNode.Split();
					continue;
				}
				break;
			}

		}

		private List<Node> Flatten()
		{
			if (Value.HasValue)
				return new List<Node>() { this };
			else
			{
				var nodes = new List<Node>();

				nodes.AddRange(LeftNode.Flatten());
				nodes.AddRange(RightNode.Flatten());

				return nodes;
			}
		}

		private void Explode()
		{
			var valueNodes = Root.Flatten();
			var index = valueNodes.IndexOf(this.LeftNode);

			if (index > 0)
			{
				var leftNeighbor = valueNodes[index - 1];
				leftNeighbor.Value += LeftNode.Value;
			}
			index = valueNodes.IndexOf(this.RightNode);
			if (index < valueNodes.Count - 1)
			{
				var rightNeighbor = valueNodes[index + 1];
				rightNeighbor.Value += RightNode.Value;
			}
			Value = 0;
			LeftNode = null;
			RightNode = null;
		}

		private void Split()
		{
			int leftValue = (int)Math.Floor(Value.Value / 2f);
			int rightValue = (int)Math.Ceiling(Value.Value / 2f);

			var leftNode = new Node()
			{
				Parent = this,
				Value = leftValue
			};

			var rightNode = new Node()
			{
				Parent = this,
				Value = rightValue
			};

			Value = null;
			LeftNode = leftNode;
			RightNode = rightNode;
		}


		public override string ToString()
		{
			if (Value.HasValue)
				return Value.ToString();

			return $"[{LeftNode},{RightNode}]";
		}
	}


}
