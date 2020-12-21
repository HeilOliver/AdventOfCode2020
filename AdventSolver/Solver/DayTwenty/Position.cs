using System;

namespace AdventSolver.Solver.DayTwenty
{
    public class Position
    {
        public Position(char value, int xPos, int yPos)
        {
            Value = value;
            XPos = xPos;
            YPos = yPos;
        }

        public char Value { get; }

        public int XPos { get; }

        public int YPos { get; }

        protected bool Equals(Position other)
        {
            return XPos == other.XPos && YPos == other.YPos;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(XPos, YPos);
        }
    }
}