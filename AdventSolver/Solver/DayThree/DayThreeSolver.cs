using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayThree
{
    [AdventSolver(3)]
    public class DayThreeSolver : SolverBase, IAdventSolver
    {

        public DayThreeSolver() : base("Data\\Day3.txt")
        {
        }

        public void Solve()
        {
            var expenses = GetDataInput()
                .ToArray();

            int hitTrees = CalculateHitTrees(expenses, 3,1);
            Console.WriteLine($"{hitTrees} Trees are hit when you go 3 right and 1 down");

            long hitTreesMultiply = CalculateHitTreeSum(expenses);
            Console.WriteLine($"{hitTreesMultiply} multiplied Trees");
        }

        private static long CalculateHitTreeSum(IReadOnlyList<string> map)
        {
            var values = new long[]
            {
                CalculateHitTrees(map, 1, 1),
                CalculateHitTrees(map, 3, 1),
                CalculateHitTrees(map, 5, 1),
                CalculateHitTrees(map, 7, 1),
                CalculateHitTrees(map, 1, 2),
            };

            return values.Multiply();
        }

        private static int CalculateHitTrees(IReadOnlyList<string> map, int right, int down)
        {
            int xPos = 0;
            int treesHit = 0;
            int mapLength = map[0].Length;

            for (int i = 0; i < map.Count; i += down)
            {
                char pos = map[i][xPos % mapLength];
                xPos += right;
                if (pos == '#')
                    treesHit++;
            }

            return treesHit;
        }
    }
}