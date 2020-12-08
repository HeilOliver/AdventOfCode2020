using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventSolver.Solver.DayEight
{
    [AdventSolver(8)]
    public class DayEightSolver : SolverBase, IAdventSolver
    {
        public DayEightSolver() : base("Data\\Day8.txt")
        {
        }

        private IEnumerable<CodeLine> GetLines()
        {
            var lines = GetDataInput();
            int lineNumber = 0;
            foreach (string line in lines)
            {
                yield return new CodeLine(line, lineNumber++);
            }
        }

        public void Solve()
        {
            var codeLines = GetLines()
                .ToArray();

            var programResult = RunProgram(codeLines);
            Console.WriteLine($"{programResult.Value} is the accumulator output before loop");

            int programOutput = AutoDebugTool(codeLines);
            Console.WriteLine($"{programOutput} is the accumulator output after fix");
        }

        private static int AutoDebugTool(IReadOnlyList<CodeLine> code)
        {
            foreach (var codeLine in code)
            {
                if (codeLine.Instruction == Instruction.Acc)
                    continue;
                if (codeLine.Value == 0)
                    continue;

                codeLine.SwitchInstruction();
                var result = RunProgram(code);

                if (result.Terminates)
                    return result.Value;
                codeLine.SwitchInstruction();
            }

            return -1;
        }

        private static RunResult RunProgram(IReadOnlyList<CodeLine> code)
        {
            var loopDetection = new HashSet<int>();
            int accumulator = 0;

            for (int i = 0; i < code.Count;)
            {
                var codeLine = code[i];
                if (loopDetection.Contains(codeLine.LineNumber))
                    return new RunResult(accumulator, false, true);
                loopDetection.Add(codeLine.LineNumber);

                switch (codeLine.Instruction)
                {
                    case Instruction.Nop:
                        i++;
                        break;
                    case Instruction.Acc:
                        i++;
                        accumulator += codeLine.Value;
                        break;
                    case Instruction.Jmp:
                        i += codeLine.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new RunResult(accumulator, true, false);
        }
    }
}