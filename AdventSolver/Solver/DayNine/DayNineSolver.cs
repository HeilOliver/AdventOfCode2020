using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Solver.DayEight;

namespace AdventSolver.Solver.DayNine
{
    [AdventSolver(9)]
    public class DayNineSolver : SolverBase, IAdventSolver
    {
        private const int Preamble = 25;

        public DayNineSolver() : base("Data\\Day9.txt")
        {
        }

        private IEnumerable<long> GetLines()
        {
            var lines = GetDataInput();
            foreach (string line in lines)
            {
                yield return long.Parse(line);
            }
        }

        public void Solve()
        {
            var codeLines = GetLines()
                .ToList();

            long? failedNumber = FindNotMatchingNumber(codeLines, Preamble);
            Console.WriteLine($"{failedNumber} is the first failed number");

            if (!failedNumber.HasValue)
            {
                Console.WriteLine("Cant find encryption weakness due to missing failed number");
                return;
            }

            long? encryptionWeakness = FindEncryptionWeakness(codeLines, failedNumber.Value);
            Console.WriteLine(encryptionWeakness.HasValue
                ? $"{encryptionWeakness} is the encryption weakness"
                : $"no encryption weakness for {failedNumber} found");
        }

        private static long? FindNotMatchingNumber(IReadOnlyList<long> input, int preamble)
        {
            for (int i = preamble; i < input.Count; i++)
            {
                long toMatch = input[i];
                
                if (HasCombination(input, toMatch, i - preamble, preamble))
                    continue;

                return toMatch;
            }

            return null;
        }

        private static long? FindEncryptionWeakness(IReadOnlyList<long> input, long weakness)
        {
            for (int i = 0; i < input.Count; i++)
            {
                long sum = input[i];

                for (int j = i + 1; j < input.Count; j++)
                {
                    sum += input[j];

                    if (sum > weakness)
                        break;

                    if (sum != weakness) 
                        continue;

                    long smallest = long.MaxValue;
                    long biggest = 0;

                    for (int k = i; k <= j; k++)
                    {
                        if (smallest > input[k])
                            smallest = input[k];

                        if (biggest < input[k])
                            biggest = input[k];
                    }

                    return smallest + biggest;
                }
            }

            return null;
        }

        private static bool HasCombination(IReadOnlyList<long> input, long value, int offset, int length)
        {
            for (int i = offset; i < offset + length - 1; i++)
            {
                for (int j = i + 1; j < offset + length; j++)
                {
                    long v1 = input[i];
                    long v2 = input[j];

                    if (v1 + v2 == value)
                        return true;
                }
            }

            return false;
        }
    }
}