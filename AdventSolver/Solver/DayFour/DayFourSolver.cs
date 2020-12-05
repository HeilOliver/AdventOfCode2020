using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventSolver.Solver.DayFour
{
    [AdventSolverAttribute(4)]
    public class DayFourSolver : IAdventSolver
    {
        private IEnumerable<Passport> CreatePassports()
        {
            var lines = File.ReadAllLines($"Data\\Day4.txt");

            StringBuilder currentBatch = new StringBuilder();

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return new Passport(currentBatch.ToString());
                    currentBatch = new StringBuilder();
                    continue;
                }
                currentBatch.Append($" {line}");
            }

            yield return new Passport(currentBatch.ToString());
        }

        public void Solve()
        {
            var passports = CreatePassports()
                .ToList();

            int consistPassports = passports
                .Count(passport => passport.IsConsistent());

            Console.WriteLine($"{consistPassports} are consistent Passwords");

            int validPassports = passports
                .Count(passport => passport.IsValid());

            Console.WriteLine($"{validPassports} are valid Passwords");
        }
    }
}