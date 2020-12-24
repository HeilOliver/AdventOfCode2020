using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventSolver.Solver.DayTwentyThree
{
    [AdventSolver(23)]
    public class DayTwentyThree : SolverBase, IAdventSolver
    {
        public DayTwentyThree() : base("Data\\Day23.txt")
        {
        }

        public void Solve()
        {
            var circleElements = GetCups()
                .ToList();
            var element = Solver(circleElements, 100);
            var builder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                element = element.Next;
                builder.Append(element.Label);
            }

            Console.WriteLine($"{builder} are all cups in row after 100 rounds");


            circleElements = GetCups()
                .ToList();
            for (int i = 10; i <= 1000000; i++)
                circleElements.Add(new Cup(i));

            element = Solver(circleElements, 10000000);
            long value0 = element.Next.Label;
            long value1 = element.Next.Next.Label;
            Console.WriteLine($"{value0 * value1} are the multiplied labels after the first cup");
        }

        private IEnumerable<Cup> GetCups()
        {
            var dataInput = GetDataInput();
            string values = dataInput.First();

            foreach (char num in values)
                yield return new Cup(int.Parse(num.ToString()));
        }

        private static Cup Solver(IReadOnlyList<Cup> circleElements, int rounds)
        {
            var lookUp = new Dictionary<int, Cup>();
            int maxValue = 0;

            for (int i = 0; i < circleElements.Count; i++)
            {
                int value = circleElements[i].Label;
                if (value > maxValue)
                    maxValue = value;

                lookUp.Add(value, circleElements[i]);

                if (i < circleElements.Count - 1)
                    circleElements[i].Next = circleElements[i + 1];
                else
                    circleElements.Last().Next = circleElements[0];
            }

            var currentCup = circleElements.First();

            for (int round = 0; round < rounds; round++)
            {
                var firstPick = currentCup.Next;
                var lastPick = currentCup.Next.Next.Next;

                currentCup.Next = lastPick.Next;
                lastPick.Next = null;

                int destLabel = currentCup.Label;
                do
                {
                    destLabel -= 1;
                    if (destLabel < 1)
                        destLabel = maxValue;
                } while (firstPick.Contains(destLabel));

                var destCup = lookUp[destLabel];
                lastPick.Next = destCup.Next;
                destCup.Next = firstPick;
                currentCup = currentCup.Next;
            }

            return lookUp[1];
        }
    }
}