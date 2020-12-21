using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayTwenty
{
    [AdventSolver(20)]
    public class DayTwentySolver : SolverBase, IAdventSolver
    {
        public DayTwentySolver() : base("Data\\Day20.txt")
        {
        }

        public void Solve()
        {
            var allTiles = GetTiles();

            var picture = new Picture(allTiles);

            long check = picture.EdgeIds()
                .Select(id => (long) id)
                .Multiply();

            Console.WriteLine($"{check} is the multiplied edge ids");

            var allValues = picture.Values()
                .Where(val => val.Value == '#')
                .ToHashSet();
            var seaMonster = new SeaMonster();
            var fragments = seaMonster.FindSeaMonsterFragments(picture);
            allValues.RemoveRange(fragments);

            Console.WriteLine($"{allValues.Count} are the leftover rough water");
        }

        private IEnumerable<Tile> GetTiles()
        {
            var buffer = new List<string>();
            var dataInput = GetDataInput();
            foreach (string line in dataInput)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return new Tile(buffer);
                    buffer.Clear();
                    continue;
                }

                buffer.Add(line);
            }

            if (!buffer.Any())
                yield break;
            yield return new Tile(buffer);
        }
    }
}