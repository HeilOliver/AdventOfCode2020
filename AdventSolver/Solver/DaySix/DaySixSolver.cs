using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DaySix
{
    [AdventSolver(6)]
    public class DaySixSolver : SolverBase, IAdventSolver
    {
        public DaySixSolver() : base("Data\\Day6.txt")
        {
        }

        public void Solve()
        {
            var questionsPerGroup = GetCustomDeclarationQuestionsPerGroup()
                .ToList();

            int sumOfAll = questionsPerGroup.Sum(group => group.DistinctYesAnswers());
            Console.WriteLine($"{sumOfAll} are the sum of all yes answers");

            int intersectYes = questionsPerGroup.Sum(group => group.IntersectYesAnswers());
            Console.WriteLine($"{intersectYes} are all yes answers in a group answered by everyone");
        }

        private IEnumerable<CustomDeclarationGroup> GetCustomDeclarationQuestionsPerGroup()
        {
            var lines = GetDataInput();
            var currentGroup = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return new CustomDeclarationGroup(currentGroup);
                    currentGroup = new List<string>();
                    continue;
                }

                currentGroup.Add(line);
            }

            yield return new CustomDeclarationGroup(currentGroup);
        }
    }
}