namespace AdventSolver.Solver.DayFifteen
{
    public class SpokenNumber
    {
        private readonly int[] lastSpokenTurn;

        public bool IsNew { get; private set; }

        public SpokenNumber(int turn)
        {
            lastSpokenTurn = new[] { turn, -1 };
            IsNew = true;
        }

        public int Distance => lastSpokenTurn[0] - lastSpokenTurn[1];

        public void Add(int turn)
        {
            IsNew = false;
            lastSpokenTurn[1] = lastSpokenTurn[0];
            lastSpokenTurn[0] = turn;
        }

        public override string ToString()
        {
            return $"-> {lastSpokenTurn[0]} - {lastSpokenTurn[1]}";
        }
    }
}