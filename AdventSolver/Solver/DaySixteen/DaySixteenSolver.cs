using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Util;

namespace AdventSolver.Solver.DaySixteen
{
    [AdventSolver(16)]
    public class DaySixteenSolver : SolverBase, IAdventSolver
    {
        public DaySixteenSolver() : base("Data\\Day16.txt")
        {
        }

        public void Solve()
        {
            List<Ticket> tickets = new List<Ticket>();
            List<Rule> rules = new List<Rule>();
            Ticket myTicket = null;
            int sw = 0;
            
            foreach (string line in GetDataInput())
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.Contains("ticket"))
                {
                    sw++;
                    continue;
                }

                if (sw == 0)
                    rules.Add(new Rule(line));
                if (sw == 1)
                    myTicket = new Ticket(line);
                if (sw == 2)
                    tickets.Add(new Ticket(line));
            }
            
            PartOne(tickets, rules);
            PartTwo(tickets, rules, myTicket);
        }

        private static void PartOne(IEnumerable<Ticket> tickets, IReadOnlyCollection<Rule> rules)
        {
            var invalidNumbers = tickets
                .SelectMany(ticket => ticket.InvalidValues(rules))
                .ToList();

            Console.WriteLine($"{invalidNumbers.Sum()} is the ticket scanning error rate");
        }

        private static void PartTwo(IEnumerable<Ticket> tickets, IReadOnlyCollection<Rule> rules, Ticket myTicket)
        {
            var validTickets = tickets
                .Where(ticket => ticket.IsValid(rules))
                .ToList();

            IDictionary<Rule, int> ruleValidIndex = GetRuleIndex(validTickets, rules);
            
            long multiply = ruleValidIndex
                .Where(rule => rule.Key.Name.StartsWith("departure"))
                .Select(rule => rule.Value)
                .Select(myTicket.GetValue)
                .Select(n => (long) n)
                .Multiply();

            Console.WriteLine($"{multiply} is the multiplicand of the values starting with departure");
        }
        
        private static IDictionary<Rule, int> GetRuleIndex(ICollection<Ticket> tickets, IEnumerable<Rule> rules)
        {
            IDictionary<Rule, ISet<int>> ruleValidIndex = new Dictionary<Rule, ISet<int>>();
            foreach (var rule in rules)
            {
                var hashSet = tickets
                    .Select(ticket => ticket.IsValidOnIndex(rule))
                    .IntersectAll()
                    .ToHashSet();
                ruleValidIndex.Add(rule, hashSet);
            }

            var seenIndex = new HashSet<int>();

            while (ruleValidIndex.Values.Any(index => index.Count != 1))
            {
                foreach (var (_, indexVal) in ruleValidIndex)
                {
                    if (indexVal.Count != 1)
                        continue;
                    int indexToRemove = indexVal.First();
                    if (seenIndex.Contains(indexToRemove))
                        continue;
                    seenIndex.Add(indexToRemove);
                    foreach (var index in ruleValidIndex.Values)
                    {
                        if (indexVal == index)
                            continue;

                        index.Remove(indexToRemove);
                    }
                    break;
                }
            }

            return ruleValidIndex
                .ToDictionary(kv => kv.Key, kv => kv.Value.First());
        }
    }
}