using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventSolver.Solver.DayTwo
{
    [AdventSolverAttribute(2)]
    public class DayTwoSolver : SolverBase, IAdventSolver
    {
        public DayTwoSolver() : base("Data\\Day2.txt")
        {
        }

        private IEnumerable<Password> CreateExpenses()
        {
            var dataInput = GetDataInput();
            return dataInput
                .Select(line => new Password(line))
                .AsEnumerable();
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