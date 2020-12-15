using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayFifteen
{
    [AdventSolver(15, InDebug = true)]
    public class DayFifteenSolver : SolverBase, IAdventSolver
    {
        public DayFifteenSolver() : base("Data\\Day15.txt")
        {
        }

        public void Solve()
        {
            var input = GetDataInput()
                .ToArray();
            var values = input[0]
                .Split(",")
                .Select(int.Parse)
                .ToArray();

            int result = MemoryGame(2020, values);
            Console.WriteLine($"{result} is the result when we play till 2020 iterations");

            result = MemoryGame(30000000, values);
            Console.WriteLine($"{result} is the result when we play till 30000000 iterations");
        }

        private static int MemoryGame(int iterations, IReadOnlyList<int> values)
        {
            int turn = values.Count;
            int lastNumber = values.Last();
            var spokenNumbers = new Dictionary<int, SpokenNumber>();

            for (int i = 0; i < values.Count; i++)
                spokenNumbers.Add(values[i], new SpokenNumber(i + 1));

            do
            {
                turn++;

                if (spokenNumbers[lastNumber].IsNew)
                {
                    if (!spokenNumbers.ContainsKey(0))
                        spokenNumbers.Add(0, new SpokenNumber(turn));
                    else
                        spokenNumbers[0].Add(turn);
                    lastNumber = 0;
                    continue;
                }

                lastNumber = spokenNumbers[lastNumber].Distance;

                if (!spokenNumbers.ContainsKey(lastNumber))
                    spokenNumbers.Add(lastNumber, new SpokenNumber(turn));
                else
                    spokenNumbers[lastNumber].Add(turn);

            } while (turn < iterations);

            return lastNumber;
        }
    }
}