using System.Linq;

namespace AdventSolver.Solver.DayTwo
{
    public class Password
    {
        private readonly int max;
        private readonly int min;
        private readonly string password;
        private readonly char toMatch;

        public Password(string value)
        {
            var values = value.Split(": ");
            password = values[1];
            toMatch = values[0][values[0].Length - 1];
            var boundValues = values[0]
                .Replace($" {toMatch}", "")
                .Split("-");

            min = int.Parse(boundValues[0]);
            max = int.Parse(boundValues[1]);
        }

        public bool IsForRentalPlaceValid()
        {
            int count = password.Count(t => t == toMatch);
            return count >= min && count <= max;
        }

        public bool IsForCorporateValid()
        {
            return (password[min - 1] == toMatch) ^ (password[max - 1] == toMatch);
        }
    }
}