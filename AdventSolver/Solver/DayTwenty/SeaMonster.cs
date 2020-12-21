using System.Collections.Generic;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayTwenty
{
    public class SeaMonster
    {
        private static readonly char[,] SeaMonsterDef =
        {
            {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' '},
            {'#', ' ', ' ', ' ', ' ', '#', '#', ' ', ' ', ' ', ' ', '#', '#', ' ', ' ', ' ', ' ', '#', '#', '#'},
            {' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', '#', ' ', ' ', ' '}
        };

        public IEnumerable<char[,]> GetViews()
        {
            char[,] rot0 = SeaMonsterDef;
            for (int i = 0; i < 4; i++)
            {
                yield return rot0;
                yield return rot0.FlipX();
                yield return rot0.FlipY();
                yield return rot0.FlipX().FlipY();
                rot0 = rot0.RotateCw();
            }
        }

        public IEnumerable<Position> FindSeaMonsterFragments(Picture picture)
        {
            int matches = 0;
            foreach (char[,] view in GetViews())
            {
                for (int i = 0; i < picture.Size - view.GetLength(0); i++)
                for (int j = 0; j < picture.Size - view.GetLength(1); j++)
                {
                    bool match = picture.Match(view, j, i);
                    if (!match)
                        continue;

                    matches++;
                    foreach (var position in GeneratePositions(view, j, i))
                        yield return position;
                }

                if (matches > 0)
                    break;
            }
        }

        private static IEnumerable<Position> GeneratePositions(char[,] toMatch, int offsetX, int offsetY)
        {
            for (int i = 0; i < toMatch.GetLength(0); i++)
            for (int j = 0; j < toMatch.GetLength(1); j++)
            {
                if (toMatch[i, j] == ' ')
                    continue;

                yield return new Position('#', j + offsetX, i + offsetY);
            }
        }
    }
}