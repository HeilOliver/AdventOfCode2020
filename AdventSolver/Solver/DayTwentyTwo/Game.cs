using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayTwentyTwo
{
    public class Game
    {
        private readonly Queue<int> playerOne;
        private readonly Queue<int> playerTwo;

        public Game(IEnumerable<int> playerOne, IEnumerable<int> playerTwo)
        {
            this.playerOne = new Queue<int>(playerOne);
            this.playerTwo = new Queue<int>(playerTwo);
        }

        public IEnumerable<int> PlayerOneDeck => playerOne;

        public IEnumerable<int> PlayerTwoDeck => playerTwo;

        public Player? WinBy { get; private set; }

        public void Play(bool recursive)
        {
            var combinations = new HashSet<CardCombination>();

            while (playerOne.Any() && playerTwo.Any())
            {
                if (recursive)
                {
                    var combination = new CardCombination(playerOne, playerTwo);
                    if (combinations.Contains(combination))
                    {
                        WinBy = Player.One;
                        break;
                    }

                    combinations.Add(combination);
                }

                int playerOneCard = playerOne.Dequeue();
                int playerTwoCard = playerTwo.Dequeue();

                Player winBy;
                if (recursive && playerOne.Count >= playerOneCard && playerTwo.Count >= playerTwoCard)
                {
                    var newPlayerOne = playerOne.Take(playerOneCard);
                    var newPlayerTwo = playerTwo.Take(playerTwoCard);

                    var game = new Game(newPlayerOne, newPlayerTwo);
                    game.Play(true);
                    winBy = game.WinBy.Value;
                }
                else
                {
                    winBy = playerOneCard > playerTwoCard ? Player.One : Player.Two;
                }

                if (winBy == Player.One)
                {
                    playerOne.Enqueue(playerOneCard);
                    playerOne.Enqueue(playerTwoCard);
                }
                else
                {
                    playerTwo.Enqueue(playerTwoCard);
                    playerTwo.Enqueue(playerOneCard);
                }
            }

            WinBy ??= playerOne.Any() ? Player.One : Player.Two;
        }

        public int CalculateGameScore()
        {
            if (WinBy == null)
                return -1;

            int[] score = new int[0];
            if (WinBy == Player.One)
                score = PlayerOneDeck.Reverse().ToArray();
            if (WinBy == Player.Two)
                score = PlayerTwoDeck.Reverse().ToArray();

            for (int i = 0; i < score.Length; i++) score[i] *= i + 1;

            return score.Sum();
        }
    }
}