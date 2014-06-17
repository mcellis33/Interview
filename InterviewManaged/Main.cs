using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Project1
    {
        public static void Main()
        {
            // int[] sortedArray = { 1, 3, 7, 8, 52, 104, 1000 };
            // Console.WriteLine(Recursion.BinarySearch(sortedArray, 53));

            /*
            int[] a = new int[] { 1, 2, 3 };
            int[] b = new int[3];
            int ai = 0;
            int bi = 0;
            while (ai <= a.Length - 1)
            {
                b[bi++] = a[ai++];
            }
            foreach (var i in a)
                Console.Write("{0}, ", i);
            Console.WriteLine();
            foreach (var i in b)
                Console.Write("{0}, ", i);
             */

            // SortingAlgorithms.Test();

            // TestBST.Test();

            // TestHeap.Test();

            // Dijkstra.Test();

            // KnapsackSolver.Test();

            DP.TestMaxNonContiguousSum();

            Console.ReadKey();
        }
    }
}
