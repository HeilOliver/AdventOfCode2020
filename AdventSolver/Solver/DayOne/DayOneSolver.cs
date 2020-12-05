using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventSolver.Solver.DayFour;

namespace AdventSolver.Solver.DayOne
{
    [AdventSolverAttribute(1)]
    public class DayOneSolver : IAdventSolver
    {
        private const int NumberToMatch = 2020;

        private static IEnumerable<int> CreateExpenses()
        {
            var lines = File.ReadAllLines($"Data\\Day1.txt");

            foreach (string line in lines)
            {
                yield return int.Parse(line);
            }
        }

        public void Solve()
        {
            var expenses = CreateExpenses()
                .ToArray();

            int expensiveSum = FindSum(expenses, 2);
            Console.WriteLine($"{expensiveSum} multiplied expensive with bound 2");

            expensiveSum = FindSum(expenses, 3);
            Console.WriteLine($"{expensiveSum} multiplied expensive with bound 3");
        }

        private static int FindSum(IReadOnlyList<int> items, int bound)
        {
            for (int i = 0; i < items.Count; i++)
            {
                int val0 = items[i];
                for (int j = i + 1; j < items.Count; j++)
                {
                    int val1 = items[j];
                    if (val0 + val1 == NumberToMatch && bound == 2)
                        return val0 * val1;

                    if (bound == 2)
                        continue;

                    for (int k = j; k < items.Count; k++)
                    {
                        int val2 = items[k];
                        if (val0 + val1 + val2 == NumberToMatch && bound == 3)
                            return val0 * val1 * val2;
                    }
                }
            }

            return -1;
        }

    }
}