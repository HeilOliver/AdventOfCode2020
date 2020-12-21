using System.Collections.Generic;
using System.Text;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayTwenty
{
    public class Tile
    {
        public Tile(List<string> input)
        {
            string header = input[0];
            string id = header
                .Replace(":", "")
                .Replace("Tile ", "");
            Id = int.Parse(id);

            int ySize = input.Count - 1;
            int xSize = input[1].Length;
            Content = new char[xSize, ySize];

            for (int y = 1; y < input.Count; y++)
            for (int x = 0; x < input[y].Length; x++)
                Content[y - 1, x] = input[y][x];

            GenerateEdges();
        }

        private Tile(char[,] tile, int id)
        {
            Content = tile;
            Id = id;
            GenerateEdges();
        }

        public int Id { get; }

        public int Size { get; private set; }

        public int TopEdge { get; private set; }

        public int BottomEdge { get; private set; }

        public int LeftEdge { get; private set; }

        public int RightEdge { get; private set; }

        public char[,] Content { get; }

        private void GenerateEdges()
        {
            var topEdge = new StringBuilder();
            var bottomEdge = new StringBuilder();
            var leftEdge = new StringBuilder();
            var rightEdge = new StringBuilder();
            Size = Content.GetLength(0);

            for (int i = 0; i < Size; i++)
            {
                topEdge.Append(Content[0, i]);
                bottomEdge.Append(Content[Size - 1, i]);
                leftEdge.Append(Content[i, 0]);
                rightEdge.Append(Content[i, Size - 1]);
            }

            string topEdgeValues = topEdge.ToString();
            string bottomEdgeValues = bottomEdge.ToString();
            string leftEdgeValues = leftEdge.ToString();
            string rightEdgeValues = rightEdge.ToString();
            TopEdge = topEdgeValues.GetHashCode();
            BottomEdge = bottomEdgeValues.GetHashCode();
            LeftEdge = leftEdgeValues.GetHashCode();
            RightEdge = rightEdgeValues.GetHashCode();
        }

        #region Option Generation

        public IEnumerable<Tile> GetAllOptions()
        {
            var rot0 = this;
            for (int i = 0; i < 4; i++)
            {
                yield return rot0;
                yield return new Tile(rot0.Content.FlipX(), Id);
                yield return new Tile(rot0.Content.FlipY(), Id);
                yield return new Tile(rot0.Content.FlipX().FlipY(), Id);
                rot0 = new Tile(rot0.Content.RotateCw(), Id);
            }
        }

        #endregion

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(TopEdge)}: {TopEdge}, {nameof(BottomEdge)}: {BottomEdge}, {nameof(LeftEdge)}: {LeftEdge}, {nameof(RightEdge)}: {RightEdge}";
        }
    }
}