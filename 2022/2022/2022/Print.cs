using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _2022.Day7;

using static _2022.Day7.Part1;

namespace _2022
{
	static class Print
	{
		public static void TwoDArray<T>(T[,] arr)
		{
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i, j]));
                }
                Console.Write(Environment.NewLine);
            }
        }

        public static void Array<T>(T[] arr)
		{
			foreach (var line in arr)
			{
				Console.WriteLine(line.ToString());
			}
		}

        public static void Directory(DirectoryEntry dir)
		{
			SubDirectory(dir, 0);
		}

		private static void SubDirectory(DirectoryEntry dir, int lvl)
		{
			Console.WriteLine($"{new string(' ', lvl * 3)}-{dir.Name} ({dir.Parent?.Name})");
			foreach (var child in dir.Children)
			{
				if (child.GetType() == typeof(DirectoryEntry))
					SubDirectory((DirectoryEntry)child, lvl + 1);
				else
					File((FileEntry)child, lvl + 1);
			}
		}

		private static void File(FileEntry file, int lvl)
		{
			Console.WriteLine($"{new string(' ', lvl * 3)}-{file.Name} ({file.Size})");
		}
	}
}
