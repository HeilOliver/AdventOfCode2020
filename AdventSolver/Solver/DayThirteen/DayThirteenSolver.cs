using System;
using System.Linq;

namespace AdventSolver.Solver.DayThirteen
{
    [AdventSolver(13)]
    public class DayThirteenSolver : SolverBase, IAdventSolver
    {
        public DayThirteenSolver() : base("Data\\Day13.txt")
        {
        }

        public void Solve()
        {
            var input = GetDataInput()
                .ToArray();

            PartOne(input[0], input[1]);
            PartTwo(input[1]);
        }

        private static void PartOne(string timeStamp, string ids)
        {
            int time = int.Parse(timeStamp);
            var busTimes = ids.Split(",")
                .Where(line => !line.Contains("x"))
                .Select(int.Parse)
                .ToDictionary(id => id, id => time % id * -1 + id);

            (int busId, int waitTime) = busTimes
                .OrderBy(e => e.Value)
                .First();

            int result = busId * waitTime;
            Console.WriteLine($"{result} is the multiplication of bus id {busId} and time to wait {waitTime}");
        }

        private static void PartTwo(string ids)
        {
            var busIds = ids
                .Split(",")
                .Select(e => e.Equals("x") ? 1 : ulong.Parse(e))
                .ToArray();

            uint offset = 1;
            ulong currentId = busIds[0];
            ulong currentTime = 0;

            while (offset < busIds.Length)
            {
                ulong nextId = busIds[offset];
                if (nextId == 1)
                {
                    offset++;
                    continue;
                }

                currentTime += currentId;
                if ((currentTime + offset) % nextId != 0)
                    continue;

                currentId *= nextId;
                offset++;
            }

            Console.WriteLine($"{currentTime} is the time where all buses depart with one minute delay");
        }
    }
}