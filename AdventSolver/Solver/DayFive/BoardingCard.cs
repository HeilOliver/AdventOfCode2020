using System.Collections.Generic;

namespace AdventSolver.Solver.DayFive
{
    public class BoardingCard
    {
        public BoardingCard(string barcode)
        {
            var rowNumber = barcode
                .Substring(0, 7)
                .Replace("F", "L")
                .Replace("B", "U")
                .ToCharArray();
            var columnNumber = barcode
                .Substring(7, 3)
                .Replace("L", "L")
                .Replace("R", "U")
                .ToCharArray();

            Row = Decode(rowNumber, 0, 0, 127);
            Column = Decode(columnNumber, 0, 0, 7);
        }

        public int Row { get; }

        public int Column { get; }

        public int SeatId => Row * 8 + Column;

        private static int Decode(IReadOnlyList<char> input, int pos, int lowerBound, int upperBound)
        {
            if (input.Count == pos)
                return lowerBound;

            int newBound = (upperBound - lowerBound) / 2 + 1;

            char direction = input[pos];
            if (direction == 'U')
                lowerBound += newBound;
            else
                upperBound -= newBound;
            return Decode(input, ++pos, lowerBound, upperBound);
        }
    }
}