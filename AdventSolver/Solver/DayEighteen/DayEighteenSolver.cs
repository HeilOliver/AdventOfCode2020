using System;
using System.Linq;

namespace AdventSolver.Solver.DayEighteen
{
    [AdventSolver(18)]
    public class DayEighteenSolver : SolverBase, IAdventSolver
    {
        public DayEighteenSolver() : base("Data\\Day18.txt")
        {
        }

        public void Solve()
        {
            var expressions = GetDataInput()
                .ToList();

            long partOneSum = expressions
                .Select(e => Solve(e, PartOneSolver))
                .Sum();

            long partTwoSum = expressions
                .Select(e => Solve(e, PartTwoSolver))
                .Sum();

            Console.WriteLine($"{partOneSum} is the sum for the first Homework result");
            Console.WriteLine($"{partTwoSum} is the sum for the second Homework result");
        }

        private static long PartOneSolver(string expression)
        {
            string[] toCalc = expression.Split(" ");
            long output = long.Parse(toCalc[0]);
            string operand = string.Empty;

            foreach (string val in toCalc)
            {
                if (val == "*" || val == "+")
                {
                    operand = val;
                    continue;
                }

                if (string.IsNullOrEmpty(operand))
                    continue;

                long toAdd = long.Parse(val);
                if (operand.Equals("*"))
                    output *= toAdd;
                else
                    output += toAdd;
            }

            return output;
        }

        private static long PartTwoSolver(string expression)
        {
            var toCalc = expression
                .Split(" ")
                .ToList();

            // Add
            for (int i = 0; i < toCalc.Count;)
            {
                if (toCalc[i] != "+")
                {
                    i++;
                    continue;
                }

                int toAdd0 = int.Parse(toCalc[i - 1]);
                int toAdd1 = int.Parse(toCalc[i + 1]);


                toCalc.RemoveAt(i + 1);
                toCalc.RemoveAt(i);
                toCalc.RemoveAt(i - 1);
                toCalc.Insert(i - 1, (toAdd0 + toAdd1).ToString());
            }

            // Multiply
            long output = int.Parse(toCalc[0]);
            for (int i = 2; i < toCalc.Count; i += 2)
            {
                int toMulti = int.Parse(toCalc[i]);
                output *= toMulti;
            }

            return output;
        }

        private static long Solve(string expression, Func<string, long> solver)
        {
            int openIndex = expression.IndexOf('(');
            if (openIndex <= -1)
                return solver.Invoke(expression);

            int closeIndex = 0;
            int openCount = 0;
            for (int i = openIndex + 1; i < expression.Length; i++)
            {
                char toCheck = expression[i];
                if (toCheck == ')')
                {
                    closeIndex = i;
                    if (openCount == 0)
                        break;
                    openCount--;
                }

                if (toCheck == '(')
                    openCount++;
            }

            string subExpression = expression.Substring(openIndex + 1, closeIndex - openIndex - 1);
            long result = Solve(subExpression, solver);
            expression = expression.Replace($"({subExpression})", result.ToString());
            return Solve(expression, solver);
        }
    }
}