using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Solver.DayTwo;

namespace AdventSolver.Solver.DayFive
{
    [AdventSolverAttribute(5)]
    public class DayFiveSolver : SolverBase, IAdventSolver
    {
        public DayFiveSolver() : base("Data\\Day5.txt")
        {
        }

        private IEnumerable<BoardingCard> CreateBoardingCards()
        {
            var dataInput = GetDataInput();
            return dataInput
                .Select(line => new BoardingCard(line))
                .AsEnumerable();
        }

        public void Solve()
        {
            var boardingCards = CreateBoardingCards()
                .ToList();

            var highestSeatIdCard = boardingCards
                .OrderByDescending(card => card.SeatId)
                .First();

            Console.WriteLine($"{highestSeatIdCard.SeatId} is the highest Seat id");

            int missingSeatId = FindMissingSeatId(boardingCards);
            Console.WriteLine($"{missingSeatId} is my Seat id");
        }

        private int FindMissingSeatId(IReadOnlyList<BoardingCard> boardingCards)
        {
            var cards = boardingCards
                .Where(card => card.Row > 0)
                .Where(card => card.Row < 127)
                .GroupBy(card => card.Row)
                .First(group => group.Count<BoardingCard>() != 8);

            var columns = cards
                .OrderBy(card => card.Column)
                .ToArray();

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Column == i)
                    continue;

                return columns[i].Row * 8 + i;
            }

            return -1;
        }
    }
}