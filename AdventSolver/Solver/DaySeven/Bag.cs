using System.Collections.Generic;

namespace AdventSolver.Solver.DaySeven
{
    public class Bag
    {
        public Bag(string bag)
        {
            CanCarry = new Dictionary<string, int>();

            var sets = bag.Split(" bags contain ");
            ColorCode = sets[0];

            string carryInfo = sets[1]
                .Replace("no other bags.", "")
                .Replace(" bags.", "")
                .Replace(" bag.", "")
                .Replace(" bags", "")
                .Replace(" bag", "");

            if (string.IsNullOrEmpty(carryInfo.Trim()))
                return;

            var carryInfoSet = carryInfo
                .Split(", ");

            foreach (string info in carryInfoSet)
            {
                var strings = info.Split(" ");
                int count = int.Parse(strings[0]);
                CanCarry.Add($"{strings[1]} {strings[2]}", count);
            }
        }

        public string ColorCode { get; }

        public IDictionary<string, int> CanCarry { get; }
    }
}