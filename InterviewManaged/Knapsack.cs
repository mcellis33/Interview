using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public static class KnapsackSolver
    {
        public static void Test()
        {
            int[] cost = new int[] { 12, 15, 2, 6, 12,  8, 11, 5 };
            int[] value = new int[] { 7,  7, 6, 4, 13, 11,  3, 9 };
            int[] solution = Knapsack(cost, value, 20);
            Array.Sort(solution);
            Console.WriteLine("Included items:");
            int totalCost = 0;
            int totalValue = 0;
            foreach (int itemIndex in solution)
            {
                Console.WriteLine("{0}", itemIndex);
                totalCost += cost[itemIndex];
                totalValue += value[itemIndex];
            }
            Console.WriteLine("Total cost: {0}", totalCost);
            Console.WriteLine("Total value: {0}", totalValue);
        }

        // Solves the Knapsack problem, where the cost and value arrays indicate the cost and value of each item.
        // Returns the indices of the items included in the optimal solution
        public static int[] Knapsack(int[] cost, int[] value, int capacity)
        {
            if (cost.Length != value.Length)
                throw new ArgumentException("Cost and value array lengths must match.");

            int numItems = cost.Length;

            /* DP strategy:
             * two 2-dimensional tables
             * table 1:
             *   cells: int: for the subproblem indicated by the row/col, what is the optimal solution's value (the total value of items included in the knapsack)
             *   dimension 1: the number of items to include in the solution
             *   dimension 2: the capacity of the solution
             * table 2 (only needed for backtracking the solution):
             *   cells: bool: for the corresponding cell in table 1, was the element indicated by dimension 1 included in the solution
             *   dimensions are equivalent to table 1's
             */
            int[,] optimalValue = new int[numItems, capacity + 1];
            bool[,] itemIncluded = new bool[numItems, capacity + 1];
            int i;
            int c;

            // With only the first item in the problem and any capacity, the solution is trivial: does the item fit in the capacity or not?
            for (c = 0; c <= capacity; ++c)
            {
                // If it fits, it sits
                if (cost[0] <= c)
                {
                    optimalValue[0, c] = cost[0];
                    itemIncluded[0, c] = true;
                }
                else
                {
                    optimalValue[0, c] = 0;
                    itemIncluded[0, c] = false;
                }
            }
            
            // Add one item at a time to the problem. For each item, solve the problem for all possible capacities.
            for (i = 1; i < numItems; ++i)
            {
                for (c = 0; c <= capacity; ++c)
                {
                    // The maximum total value if we exclude item i
                    int loseItValue = optimalValue[i - 1, c];

                    // The maximum total value if we include item i
                    int useItValue;
                    int useItRemainingCost = c - cost[i];
                    if (useItRemainingCost >= 0)
                    {
                        useItValue = value[i] + optimalValue[i - 1, useItRemainingCost];
                    }
                    // If item i doesn't fit, then don't use it
                    else
                    {
                        useItValue = -1;
                    }

                    if (loseItValue > useItValue)
                    {
                        optimalValue[i, c] = loseItValue;
                        itemIncluded[i, c] = false;
                    }
                    else
                    {
                        optimalValue[i, c] = useItValue;
                        itemIncluded[i, c] = true;
                    }
                }
            }

            // Backtrack the optimal solution from the tables
            var solutionList = new List<int>();
            c = capacity;
            for (i = numItems - 1; i >= 0; --i)
            {
                if (itemIncluded[i, c])
                {
                    solutionList.Add(i);
                    c -= cost[i];
                }
            }

            return solutionList.ToArray();
        }
    }
}
