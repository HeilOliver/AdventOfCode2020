using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventSolver.Solver.DaySixteen
{
    public class Ticket
    {
        private readonly int[] values;

        public Ticket(string serializedTicket)
        {
            values = serializedTicket
                .Split(",")
                .Select(int.Parse)
                .ToArray();
        }

        public int GetValue(int index)
        {
            return values[index];
        }

        public IEnumerable<int> IsValidOnIndex(Rule rule)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (rule.InRange(values[i]))
                    yield return i;
            }
        }

        public IEnumerable<int> InvalidValues(IReadOnlyCollection<Rule> ruleSet)
        {
            return values
                .Select(value => new {value, valid = ruleSet.Any(rule => rule.InRange(value))})
                .Where(t => !t.valid)
                .Select(t => t.value);
        }

        public bool IsValid(IReadOnlyCollection<Rule> ruleSet)
        {
            return !InvalidValues(ruleSet).Any();
        }
    }
}