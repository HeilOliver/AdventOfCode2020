using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayNineteen
{
    public class RuleValidator
    {
        private readonly Dictionary<int, Rule> rules;

        public RuleValidator(IEnumerable<string> rawRules)
        {
            rules = new Dictionary<int, Rule>();
            
            foreach (string rawRule in rawRules)
            {
                string[] split = rawRule.Split(":");
                int index = int.Parse(split[0]);
                var rule = new Rule(split[1]);
                rules.Add(index, rule);
            }
        }

        public IEnumerable<string> ValidateMessages(IEnumerable<string> messages)
        {
            return messages.Where(ValidateMessage);
        }

        private bool ValidateMessage(string message)
        {
            const int startRule = 0;
            return MatchesRule(message, new RuleRef(startRule), 0).Any(x => x == message.Length);
        }

        private IEnumerable<int> MatchesRule(string message, RuleRef ruleIndex, int usedUpChars)
        {
            var results = MatchesRule(message, ruleIndex.Ref0, usedUpChars);
           
            if (ruleIndex.Ref1.HasValue)
            {
                List<int> secRuleResults = new List<int>();
                foreach (int usedChar in results)
                    secRuleResults = secRuleResults
                        .Concat(MatchesRule(message, ruleIndex.Ref1.Value, usedChar))
                        .ToList();
                results = secRuleResults;
            }

            if (ruleIndex.Ref2.HasValue)
            {
                List<int> thirdRuleResult = new List<int>();
                foreach (int usedChar in results)
                    thirdRuleResult = thirdRuleResult
                        .Concat(MatchesRule(message, ruleIndex.Ref2.Value, usedChar))
                        .ToList();
                results = thirdRuleResult;
            }

            return results;
        }

        private List<int> MatchesRule(string message, int ruleIndex, int usedUpChars)
        {
            var rule = rules[ruleIndex];

            if (rule.IsConst)
            {
                bool success = message
                    .Skip(usedUpChars)
                    .FirstOrDefault() == rule.Letter;
                
                return success ?
                    new List<int> { usedUpChars + 1 }:
                    new List<int>();
            }

            List<int> results = new List<int>();
            foreach (var orRule in rule.OrRules)
                results = results
                    .Concat(MatchesRule(message, orRule, usedUpChars))
                    .ToList();
            return results;
        }
    }
}