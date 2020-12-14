namespace AdventSolver.Solver.DayTwelve
{
    public class Instruction
    {
        public Instruction(string data)
        {
            Command = data[0];
            string valueData = data.Substring(1);
            Value = int.Parse(valueData);

            if (Command == 'R' || Command == 'L')
                Value /= 90;
        }

        public char Command { get; }

        public int Value { get; }

        public override string ToString()
        {
            return $"{Command} {Value}";
        }
    }
}