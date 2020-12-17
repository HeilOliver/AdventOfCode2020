using System.Linq;

namespace AdventSolver.Solver.DaySixteen
{
    public class Rule
    {
        private readonly int min0;
        private readonly int min1;
        private readonly int max0;
        private readonly int max1;

        public string Name { get; }
        
        public Rule(string serializedRule)
        {
            string[] strings = serializedRule
                .Split(":")
                .ToArray();
            Name = strings[0];

            string[][] sRanges = strings[1]
                .Split(" or ")
                .Select(s => s.Split("-"))
                .ToArray();
            min0 = int.Parse(sRanges[0][0]);
            min1 = int.Parse(sRanges[1][0]);
            max0 = int.Parse(sRanges[0][1]);
            max1 = int.Parse(sRanges[1][1]);
        }

        public bool InRange(int value)
        {
            if (value >= min0 && value <= max0)
                return true;
            if (value >= min1 && value <= max1)
                return true;

            return false;
        }
    }
}