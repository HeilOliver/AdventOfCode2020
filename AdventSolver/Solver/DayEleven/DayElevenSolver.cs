using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayEleven
{
    [AdventSolver(11)]
    public class DayElevenSolver : SolverBase, IAdventSolver
    {
        public DayElevenSolver() : base("Data\\Day11.txt")
        {
        }

        private IEnumerable<Seat> GetSeats()
        {
            var lines = GetDataInput()
                .ToList();

            Seat[,] places = new Seat[lines.Count, lines.First().Length];
            for (int i = 0; i < places.GetLength(0); i++)
            {
                string line = lines[i];
                for (int j = 0; j < places.GetLength(1); j++)
                {
                    places[i, j] = new Seat(line[j], j, i, places);
                    yield return places[i, j];
                }
            }
        }

        public void Solve()
        {
            SolvePartOne();
            SolvePartTwo();
        }

        private void SolvePartOne()
        {
            var seats = GetSeats().ToList();
            do
            {
                seats.ForEach(e => e.Execute());
                seats.ForEach(e => e.CalculateRuleOne());
            } while (!seats.All(e => e.IsSatisfied));

            int occupiedSeats = seats
                .Count(seat => seat.State == SeatPlacement.Occupied);
            Console.WriteLine($"{occupiedSeats} are Occupied (First Rule)");
        }

        private void SolvePartTwo()
        {
            var seats = GetSeats().ToList();
            do
            {
                seats.ForEach(e => e.Execute());
                seats.ForEach(e => e.CalculateRuleTwo());
            } while (!seats.All(e => e.IsSatisfied));

            int occupiedSeats = seats
                .Count(seat => seat.State == SeatPlacement.Occupied);
            Console.WriteLine($"{occupiedSeats} are Occupied (Second Rule)");
        }
    }
}