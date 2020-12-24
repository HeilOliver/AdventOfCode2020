using System.Collections.Generic;

namespace AdventSolver.Solver.DayTwentyThree
{
    public class Cup
    {
        public Cup(int label)
        {
            Label = label;
        }

        public Cup Next { get; set; }

        public int Label { get; }

        public bool Contains(in int value)
        {
            return InternalContains(value, new HashSet<Cup>());
        }

        private bool InternalContains(in int value, ISet<Cup> alreadySeen)
        {
            if (Label == value)
                return true;
            if (Next == null)
                return false;
            if (alreadySeen.Contains(this))
                return false;
            alreadySeen.Add(this);
            return Next.InternalContains(value, alreadySeen);
        }
    }
}