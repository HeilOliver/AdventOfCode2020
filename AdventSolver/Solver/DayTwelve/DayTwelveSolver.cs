using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Solver.DayEleven;

namespace AdventSolver.Solver.DayTwelve
{
    [AdventSolver(12)]
    public class DayTwelveSolver : SolverBase, IAdventSolver
    {
        public DayTwelveSolver() : base("Data\\Day12.txt")
        {
        }

        public void Solve()
        {
            var instructions = GetInstruction().ToList();
            var ferry = new Ferry();
            foreach (var instruction in instructions)
            {
                ferry.Execute(instruction);
            }

            int hemmingDistance = Math.Abs(ferry.XPos) + Math.Abs(ferry.YPos);
            Console.WriteLine($"{hemmingDistance} is the hemming distance of the Ferry");

            ferry = new Ferry(new WayPoint(10,1));
            foreach (var instruction in instructions)
            {
                ferry.ExecuteWithWayPoint(instruction);
            }

            hemmingDistance = Math.Abs(ferry.XPos) + Math.Abs(ferry.YPos);
            Console.WriteLine($"{hemmingDistance} is the hemming distance of the way point guided Ferry");
        }

        private IEnumerable<Instruction> GetInstruction()
        {
            var lines = GetDataInput()
                .ToList();

            return lines.Select(line => new Instruction(line));
        }

    }
}