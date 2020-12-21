using System.Collections.Generic;

namespace AdventSolver.Solver.DayTwentyOne
{
    public class Food
    {
        public Food(string input)
        {
            string[] values = input
                .Replace(",", "")
                .Replace(")", "")
                .Split(" (contains ");

            Ingredients = values[0].Split(" ");
            Allergens = values[1].Split(" ");
        }

        public IReadOnlyCollection<string> Allergens { get; }

        public IReadOnlyCollection<string> Ingredients { get; }
    }
}