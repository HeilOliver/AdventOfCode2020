namespace AdventSolver.Solver.DayEight
{
    public class CodeLine
    {
        public CodeLine(string input, int lineNumber)
        {
            LineNumber = lineNumber;
            var values = input.Split(" ");
            Instruction = values[0] switch
            {
                "acc" => Instruction.Acc,
                "jmp" => Instruction.Jmp,
                _ => Instruction.Nop
            };
            Value = int.Parse(values[1]);
        }

        public Instruction Instruction { get; private set; }

        public int Value { get; }

        public int LineNumber { get; }

        public void SwitchInstruction()
        {
            if (Instruction == Instruction.Acc)
                return;

            Instruction = Instruction == Instruction.Jmp ? Instruction.Nop : Instruction.Jmp;
        }

        public override string ToString()
        {
            return $"{nameof(LineNumber)}: {LineNumber}, {Instruction}, {nameof(Value)}: {Value}";
        }
    }
}