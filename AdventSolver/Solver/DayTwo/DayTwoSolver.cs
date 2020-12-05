using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventSolver.Solver.DayTwo
{
    [AdventSolverAttribute(2)]
    public class DayTwoSolver : IAdventSolver
    {
        private static IEnumerable<Password> CreateExpenses()
        {
            var lines = File.ReadAllLines($"Data\\Day2.txt");
            foreach (string line in lines)
            {
                yield return new Password(line);
            }
        }

        public void Solve()
        {
            var passwords = CreateExpenses().ToList();

            int validPasswords = passwords.Count(password => password.IsForRentalPlaceValid());
            Console.WriteLine($"{validPasswords} are valid passwords for the sled rental place");

            validPasswords = passwords.Count(password => password.IsForCorporateValid());
            Console.WriteLine($"{validPasswords} are valid passwords for the Official Toboggan Corporate");
        }
    }
}