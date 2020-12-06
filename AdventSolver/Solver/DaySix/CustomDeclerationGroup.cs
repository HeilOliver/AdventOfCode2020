using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DaySix
{
    public class CustomDeclarationGroup
    {
        private readonly IReadOnlyList<string> currentGroup;

        public CustomDeclarationGroup(IReadOnlyList<string> currentGroup)
        {
            this.currentGroup = currentGroup;
        }

        public int IntersectYesAnswers()
        {
            var yesAnswers = currentGroup
                .Select(val => val.ToCharArray())
                .ToList();

            var hashSet = new HashSet<char>(yesAnswers.First());
            foreach (var list in yesAnswers.Skip(1))
                hashSet.IntersectWith(list);

            return hashSet.Count;
        }

        public int DistinctYesAnswers()
        {
            return string.Concat(currentGroup)
                .ToCharArray()
                .Distinct()
                .Count();
        }
    }
}