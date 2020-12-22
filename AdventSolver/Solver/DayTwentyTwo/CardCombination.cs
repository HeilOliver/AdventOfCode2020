using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayTwentyTwo
{
    public class CardCombination
    {
        private readonly int[] card0;
        private readonly int[] card1;
        private readonly int hash0;
        private readonly int hash1;

        public CardCombination(IEnumerable<int> card0, IEnumerable<int> card1)
        {
            this.card0 = card0.ToArray();
            this.card1 = card1.ToArray();

            hash0 = this.card0
                .Aggregate(17, (state, current) => state * 23 + current.GetHashCode());
            hash1 = this.card1
                .Aggregate(17, (state, current) => state * 23 + current.GetHashCode());
        }

        protected bool Equals(CardCombination other)
        {
            return card0.SequenceEqual(other.card0) && card1.SequenceEqual(other.card1);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((CardCombination) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(hash0, hash1);
        }
    }
}