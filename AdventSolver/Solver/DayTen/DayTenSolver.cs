using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayTen
{
    [AdventSolver(10)]
    public class DayTenSolver : SolverBase, IAdventSolver
    {
        private const int Preamble = 25;

        public DayTenSolver() : base("Data\\Day10.txt")
        {
        }

        private IEnumerable<int> GetAdapter()
        {
            var lines = GetDataInput();
            foreach (string line in lines)
            {
                yield return int.Parse(line);
            }
        }

        public void Solve()
        {
            var adapter = GetAdapter()
                .OrderBy(e => e)
                .ToList();

            int myAdapter = adapter.Max() + 3;
            adapter = adapter
                .Prepend(0)
                .Append(myAdapter)
                .ToList();

            int distanceOne = CountDistance(adapter, 1);
            int distanceThree = CountDistance(adapter, 3);
            Console.WriteLine($"{distanceOne * distanceThree} is the multiplied counted distance");


            long combinations = CountCombinations(adapter);
            Console.WriteLine($"{combinations} combinations are Possible");
        }

        private static long CountCombinations(IReadOnlyList<int> data)
        {
            long[] branchingFactors = new long[data.Count];
            branchingFactors[0] = 1;
            for (int i = 0; i < data.Count; i++)
            {
                long factor = branchingFactors[i];
                int curAdapt = data[i];

                for (int j = 1; i + j < data.Count; j++)
                {
                    int toCheck = data[i + j];
                    if (toCheck - curAdapt <= 3)
                        branchingFactors[i + j] += factor;
                    else
                        break;
                }
            }

            return branchingFactors.Last();
        }

        private static int CountDistance(IReadOnlyList<int> data, int distance)
        {
            int count = 0;
            for (int i = 0; i < data.Count- 1; i++)
            {
                int adapter0 = data[i];
                int adapter1 = data[i+1];

                if (adapter1 - adapter0 == distance)
                    count++;
            }

            return count;
        }

    }
}