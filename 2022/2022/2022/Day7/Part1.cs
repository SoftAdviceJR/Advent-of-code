using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _2022.Day7
{
	static class Part1
	{
		public static DirectoryEntry ReadStructure()
		{
			var input = File.ReadAllLines("Day7/input.txt");

			var root = new DirectoryEntry("/");

			var currentDir = root;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];

				if (line.StartsWith("$ cd"))
				{
					string newDirName = line.Split(' ')[2];
					if (newDirName == "..")
						currentDir = currentDir.Parent;
					else if (newDirName == "/")
						currentDir = root;
					else
						try
						{
							currentDir = currentDir.GetDirectory(newDirName);
						}
						catch (Exception e)
						{
							Print.Directory(root);
							Console.WriteLine(e.Message);
							throw;
						}
				}
				if (line == "$ ls")
				{
					while (!input[i + 1].StartsWith("$"))
					{
						var entryLine = input[i + 1];
						var args = entryLine.Split(' ');
						string name = args[1];
						if (entryLine.StartsWith("dir"))
						{
							currentDir.Children.Add(new DirectoryEntry(name, currentDir));
						}
						if (char.IsDigit(entryLine[0]))
						{
							int size = int.Parse(args[0]);
							currentDir.Children.Add(new FileEntry(size, name));
						}
						i++;
						if (i + 1 >= input.Length)
							break;
					}

				}
			}

			return root;
		}

		public static string Solve()
		{
			var root = ReadStructure();

			List<int> dirSizes = new List<int>();

			var dirs = root.GetAllDirectories();

			return dirs.Where(dir => dir.Size <= 100000).Sum(dir => dir.Size).ToString();
		}

		



	}

	





}
