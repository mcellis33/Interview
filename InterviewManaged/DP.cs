using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class DP
    {
        public static void TestMaxNonContiguousSum()
        {
            int[] a = {6, 1, 2, 7};
            foreach (int i in a) Console.Write("{0}, ", i);
            Console.Write(": ");
            Console.Write(MaxNonContiguousSum(a));
        }

        public static int MaxNonContiguousSum(int[] a)
        {
            if (a == null || a.Length == 0)
                return 0;
            if (a.Length == 1)
                return a[0];
            int[] maxToHere = new int[a.Length];
            maxToHere[0] = a[0];
            maxToHere[1] = a[0] > a[1] ? a[0] : a[1];
            for (int i = 2; i < a.Length; ++i)
            {
                int useIt = maxToHere[i-2] + a[i];
                int loseIt = maxToHere[i-1];
                maxToHere[i] = Math.Max(useIt, loseIt);
            }
            return maxToHere[maxToHere.Length - 1];
        }
    }
}
