using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayTwenty
{
    public class Picture
    {
        private char[,] picture;
        private Tile[,] tiles;

        public Picture(IEnumerable<Tile> toCombine)
        {
            var allOptions = toCombine
                .SelectMany(tile => tile.GetAllOptions())
                .ToList();

            CombineTiles(allOptions);
            FillImage();
        }

        public int Size { get; private set; }

        private void CombineTiles(IReadOnlyCollection<Tile> allOptions)
        {
            int size = (int) Math.Sqrt(allOptions.Count / 16);
            var combineTiles = new CombineHelper[size, size];

            var usedIds = new HashSet<int>();
            for (int i = 0; i < size * size; i++)
            {
                int yPos = i / size;
                int xPos = i % size;
                combineTiles[yPos, xPos] ??= new CombineHelper();
                var helper = combineTiles[yPos, xPos];

                if (helper.Tile != null)
                {
                    usedIds.Remove(helper.Tile.Id);
                    helper.AlreadyUsed.Add(helper.Tile);
                    helper.Tile = null;
                }

                var query = allOptions
                    .Where(tile => !usedIds.Contains(tile.Id))
                    .Where(tile => !helper.AlreadyUsed.Contains(tile));

                if (xPos > 0)
                {
                    var lastTile = combineTiles[yPos, xPos - 1].Tile;
                    query = query.Where(tile => tile.LeftEdge == lastTile.RightEdge);
                }

                if (yPos > 0)
                {
                    var aboveTile = combineTiles[yPos - 1, xPos].Tile;
                    query = query.Where(tile => tile.TopEdge == aboveTile.BottomEdge);
                }

                var thisTile = query
                    .FirstOrDefault();

                if (thisTile == null)
                {
                    i -= 2;
                    continue;
                }

                helper.Tile = thisTile;
                usedIds.Add(thisTile.Id);
            }


            tiles = new Tile[size, size];
            for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
                tiles[i, j] = combineTiles[i, j].Tile;
        }

        private void FillImage()
        {
            var tile = tiles[0, 0];
            Size = tiles.GetLength(0) * (tile.Size - 2);
            picture = new char[Size, Size];

            for (int i = 0; i < tiles.GetLength(0); i++)
            for (int j = 0; j < tiles.GetLength(0); j++)
            {
                char[,] content = tiles[i, j].Content;
                int offsetX = j * (tile.Size - 2);
                int offsetY = i * (tile.Size - 2);
                InsertTile(content, offsetX, offsetY);
            }
        }

        private void InsertTile(char[,] content, int offsetX, int offsetY)
        {
            for (int i = 1; i < content.GetLength(0) - 1; i++)
            for (int j = 1; j < content.GetLength(0) - 1; j++)
            {
                char atPos = content[i, j];
                picture[i - 1 + offsetY, j - 1 + offsetX] = atPos;
            }
        }

        public IEnumerable<int> EdgeIds()
        {
            yield return tiles[0, 0].Id;
            yield return tiles[0, tiles.GetLength(1) - 1].Id;
            yield return tiles[tiles.GetLength(0) - 1, 0].Id;
            yield return tiles[tiles.GetLength(0) - 1, tiles.GetLength(1) - 1].Id;
        }

        public IEnumerable<Position> Values()
        {
            for (int i = 0; i < picture.GetLength(0); i++)
            for (int j = 0; j < picture.GetLength(1); j++)
                yield return new Position(picture[i, j], j, i);
        }

        public bool Match(char[,] toMatch, int offsetX, int offsetY)
        {
            for (int i = 0; i < toMatch.GetLength(0); i++)
            for (int j = 0; j < toMatch.GetLength(1); j++)
            {
                if (toMatch[i, j] == ' ')
                    continue;

                if (picture[i + offsetY, j + offsetX] == toMatch[i, j])
                    continue;

                return false;
            }

            return true;
        }

        private class CombineHelper
        {
            public readonly HashSet<Tile> AlreadyUsed;

            public Tile Tile;

            public CombineHelper()
            {
                AlreadyUsed = new HashSet<Tile>();
            }
        }
    }
}