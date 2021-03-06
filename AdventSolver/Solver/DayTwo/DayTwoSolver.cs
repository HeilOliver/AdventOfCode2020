﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayTwo
{
    [AdventSolver(2)]
    public class DayTwoSolver : SolverBase, IAdventSolver
    {
        public DayTwoSolver() : base("Data\\Day2.txt")
        {
        }

        public void Solve()
        {
            var passwords = CreatePasswords().ToList();

            int validPasswords = passwords.Count(password => password.IsForRentalPlaceValid());
            Console.WriteLine($"{validPasswords} are valid passwords for the sled rental place");

            validPasswords = passwords.Count(password => password.IsForCorporateValid());
            Console.WriteLine($"{validPasswords} are valid passwords for the Official Toboggan Corporate");
        }

        private IEnumerable<Password> CreatePasswords()
        {
            var dataInput = GetDataInput();
            return dataInput
                .Select(line => new Password(line))
                .AsEnumerable();
        }
    }
}