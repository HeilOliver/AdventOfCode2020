using System.Collections.Generic;

namespace AdventSolver.Solver.DayNineteen
{
    public class Rule
    {
        public bool IsConst { get; }
        
        public char Letter { get; }
        
        public List<RuleRef> OrRules { get; }

        public Rule(string rawRule)
        {
            OrRules = new List<RuleRef>();
            rawRule = rawRule.Trim();
            
            if (rawRule.Contains("\""))
            {
                IsConst = true;
                Letter = rawRule[1];
            }
            else
            {
                string[] rules = rawRule.Split("|");
               
                foreach (string rule in rules)
                {
                    string[] ruleRefs = rule.Trim().Split(" ");
                    int firstRule = int.Parse(ruleRefs[0]);
                    int? secondRule = null;
                    int? thirdRule = null;

                    if (ruleRefs.Length > 1)
                        secondRule = int.Parse(ruleRefs[1]);
                    if (ruleRefs.Length > 2)
                        thirdRule = int.Parse(ruleRefs[2]);

                    OrRules.Add(new RuleRef(firstRule, secondRule, thirdRule));
                }
            }
        }
    }
}