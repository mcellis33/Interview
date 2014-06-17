using System;
using System.Linq;
using System.Security.Cryptography;

namespace Project1
{
    public static class SortingAlgorithms
    {
        public static void Test()
        {
            Console.WriteLine("Testing MergeSort:");
            TestSort(MergeSort);

            Console.WriteLine("Testing QuickSort:");
            TestSort(QuickSort);
        }

        public static void TestSort(Action<uint[]> sort)
        {
            uint testListLength = 7;
            uint testRunCount = 10;
            uint maxInt = 30;
            uint[] a = new uint[testListLength];

            var rng = new RNGCryptoServiceProvider();
            byte[] intBytes = new byte[sizeof(uint)];

            for (int j = 0; j < testRunCount; ++j)
            {
                for (int i = 0; i < a.Length; ++i)
                {
                    rng.GetBytes(intBytes);
                    a[i] = BitConverter.ToUInt32(intBytes, 0) % maxInt;
                }

                foreach (var e in a) Console.Write("{0}, ", e);
                Console.Write(" ->  ");

                sort(a);

                foreach (var e in a) Console.Write("{0}, ", e);
                Console.WriteLine();

                for (int k = 1; k < a.Length; ++k)
                {
                    if (a[k - 1] > a[k])
                    {
                        var oldColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR: in sorted list, element {0} ({1}) is greater than element {2} ({3})", k - 1, a[k-1], k, a[k]);
                        Console.ForegroundColor = oldColor;
                    }
                }
            }
        }

        public static void MergeSort<T>(T[] a) where T : IComparable<T>
        {
            MergeSortHelper(a, 0, a.Length - 1);
        }

        public static void MergeSortHelper<T>(T[] a, int start, int end) where T : IComparable<T>
        {
            if (end <= start)
                return;
            int middle = (end - start) / 2 + start;
            MergeSortHelper(a, start, middle);
            MergeSortHelper(a, middle + 1, end);
            Merge(a, start, end);
        }

        public static void Merge<T>(T[] a, int start, int end) where T : IComparable<T>
        {
            T[] result = new T[end - start + 1];
            int resultIndex = 0;
            int middle = (end - start) / 2 + start;
            int left = start;
            int right = middle + 1;
            while (left <= middle && right <= end)
            {
                if (a[left].CompareTo(a[right]) <= 0)
                    result[resultIndex++] = a[left++];
                else
                    result[resultIndex++] = a[right++];
            }
            while (left <= middle)
                result[resultIndex++] = a[left++];
            while (right <= end)
                result[resultIndex++] = a[right++];
            Array.Copy(result, 0, a, start, result.Length);
        }

        public static void QuickSort<T>(T[] a) where T : IComparable<T>
        {
            QuickSort(a, 0, a.Length - 1);
        }

        public static void QuickSort<T>(T[] a, int start, int end) where T : IComparable<T>
        {
            if (start >= end)
            {
                return;
            }
            int partition = ((end - start) / 2) + start; // Pick random partition element
            partition = Partition(a, start, end, partition);
            QuickSort(a, start, partition - 1);
            QuickSort(a, partition + 1, end);
        }

        public static int Partition<T>(T[] a, int start, int end, int partition) where T : IComparable<T>
        {
            int dst = start;
            int src = start;
            T partitionElement = a[partition];
            Swap(a, partition, end);
            while (src < end)
            {
                int result = a[src].CompareTo(partitionElement);
                if (result < 0)
                {
                    Swap(a, src, dst);
                    ++dst;
                }
                ++src;
            }
            Swap(a, end, dst);
            return dst;
        }

        public static void Swap<T>(T[] a, int x, int y)
        {
            if (x == y) return;
            T tmp = a[x];
            a[x] = a[y];
            a[y] = tmp;
        }
    }
}