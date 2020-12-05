using System;

namespace AdventSolver.Solver
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdventSolverAttribute : Attribute
    {
        public AdventSolverAttribute(int dayToSolve)
        {
            DayToSolve = dayToSolve;
        }

        public int DayToSolve { get;}
    }
}