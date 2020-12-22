using System;
using System.Collections.Generic;

namespace AdventSolver.Solver.DayTwentyTwo
{
    [AdventSolver(22, InDebug = true)]
    public class DayTwentyTwoSolver : SolverBase, IAdventSolver
    {
        public DayTwentyTwoSolver() : base("Data\\Day22.txt")
        {
        }

        public void Solve()
        {
            var (playerOne, playerTwo) = LoadPlayerCards();

            var game = new Game(playerOne, playerTwo);
            game.Play(false);

            Console.WriteLine($"{game.CalculateGameScore()} is the first game score");

            game = new Game(playerOne, playerTwo);
            game.Play(true);

            Console.WriteLine($"{game.CalculateGameScore()} is the second game score");
        }

        private (List<int> playerOne, List<int> playerTwo) LoadPlayerCards()
        {
            var lines = GetDataInput();
            var playerOne = new List<int>();
            var playerTwo = new List<int>();

            bool playerTwoData = false;
            foreach (string line in lines)
            {
                if (line.Contains("Player"))
                    continue;
                if (string.IsNullOrEmpty(line))
                {
                    playerTwoData = true;
                    continue;
                }

                if (playerTwoData)
                    playerTwo.Add(int.Parse(line));
                else
                    playerOne.Add(int.Parse(line));
            }

            return (playerOne, playerTwo);
        }
    }
}