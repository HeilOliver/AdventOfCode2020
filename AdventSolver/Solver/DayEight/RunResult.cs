namespace AdventSolver.Solver.DayEight
{
    public class RunResult
    {
        public RunResult(int value, bool terminates, bool fault)
        {
            Value = value;
            Terminates = terminates;
            Fault = fault;
        }

        public int Value { get; }

        public bool Terminates { get; }

        public bool Fault { get; }
    }
}