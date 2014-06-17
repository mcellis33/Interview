using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class Recursion
    {
        public static int BinarySearch<T>(T[] sortedArray, T value) where T : IComparable<T>
        {
            return BinarySearch<T>(sortedArray, value, 0, sortedArray.Length - 1);
        }

        private static int BinarySearch<T>(T[] sortedArray, T value, int startIndex, int endIndex) where T : IComparable<T>
        {
            int middleIndex = (endIndex - startIndex)/2 + startIndex;
            int compareToMiddle = value.CompareTo(sortedArray[middleIndex]);
            if (compareToMiddle == 0)
                return middleIndex;
            if (startIndex == endIndex)
                return -1;
            if (compareToMiddle > 0)
                return BinarySearch(sortedArray, value, middleIndex + 1, endIndex);
            else
                return BinarySearch(sortedArray, value, startIndex, middleIndex);
        }
    }
}
