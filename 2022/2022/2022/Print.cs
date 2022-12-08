using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
