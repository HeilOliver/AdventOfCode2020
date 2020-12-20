using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayNineteen
{
    [AdventSolver(19)]
    public class DayNineteenSolver : SolverBase, IAdventSolver
    {

        public DayNineteenSolver() : base("Data\\Day19.txt")
        {
        }

        public void Solve()
        {
            var dataInput = GetDataInput();
            List<string> rawRules = new List<string>();
            List<string> messages = new List<string>();
            bool rulesProcessed = false;
            foreach (string line in dataInput)
            {
                if (string.IsNullOrEmpty(line))
                {
                    rulesProcessed = true;
                    continue;
                }
                
                if (rulesProcessed)
                    messages.Add(line);
                else
                    rawRules.Add(line);
            }

            int partOne = SolvePartOne(rawRules.ToList(), messages.ToList());
            Console.WriteLine($"{partOne} messages completely match rule 0");
            int partTwo = SolvePartTwo(rawRules.ToList(), messages.ToList());
            Console.WriteLine($"{partTwo} messages completely match rule 0 after modify rule 8 and 11");
        }

        private static int SolvePartOne(IEnumerable<string> rawRules, IEnumerable<string> messages)
        {
            var validator = new RuleValidator(rawRules);
            return validator
                .ValidateMessages(messages)
                .Count();
        }

        private static int SolvePartTwo(IEnumerable<string> rawRules, IEnumerable<string> messages)
        {
            rawRules = rawRules
                .Where(rule => !rule.StartsWith("8:") && !rule.StartsWith("11:"))
                .Append("8: 42 | 42 8")
                .Append("11: 42 31 | 42 11 31");
            
            var validator = new RuleValidator(rawRules);
            return validator
                .ValidateMessages(messages)
                .Count();
        }
    }
}