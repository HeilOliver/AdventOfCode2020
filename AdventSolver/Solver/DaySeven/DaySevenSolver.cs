using System;
using System.Collections.Generic;
using System.Linq;using AdventSolver.Solver.DaySix;

namespace AdventSolver.Solver.DaySeven
{
    [AdventSolver(7)]
    public class DaySevenSolver : SolverBase, IAdventSolver
    {
        private const string BagColor = "shiny gold";

        public DaySevenSolver() : base("Data\\Day7.txt")
        {
        }

        private IEnumerable<Bag> GetBags()
        {
            var lines = GetDataInput();
         
            foreach (string line in lines)
            {
                yield return new Bag(line);
            }
        }

        public void Solve()
        {
            var bags = GetBags()
                .ToHashSet();

            int countOfBags = bags
                .Select(bag => ContainsColorCode(bag, bags, BagColor))
                .Count(result => result);

            Console.WriteLine($"{countOfBags} that includes the {BagColor} one");

            var shinyGoldBag = bags
                .First(e => e.ColorCode.Equals(BagColor));
            int totalAmount = AmountOfInsertedBags(bags, shinyGoldBag);
            totalAmount -= 1; // We dont need our shiny gold bag

            Console.WriteLine($"{totalAmount} Bags are in the {BagColor} bag");
        }

        private static int AmountOfInsertedBags(HashSet<Bag> bags, Bag bag)
        {
            int count = 1;
            foreach ((string color, int value) in bag.CanCarry)
            {
                var bagToFind = bags.First(e => e.ColorCode.Equals(color));

                count += AmountOfInsertedBags(bags, bagToFind) * value;
            }

            return count;
        }

        private static bool ContainsColorCode(Bag currentBag, ISet<Bag> bags, string searchedColor, ISet<string> alreadySeen = null)
        {
            alreadySeen ??= new HashSet<string>();
            if (alreadySeen.Contains(currentBag.ColorCode))
                return false;
            alreadySeen.Add(currentBag.ColorCode);

            foreach ((string color, int _) in currentBag.CanCarry)
            {
                if (color.Equals(searchedColor))
                    return true;

                var toSearch = bags
                    .First(e => e.ColorCode.Equals(color));

                bool result = ContainsColorCode(toSearch, bags, searchedColor, alreadySeen);

                if (!result)
                    continue;
                return true;
            }

            return false;
        }
    }
}