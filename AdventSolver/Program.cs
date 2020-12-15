using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AdventSolver.Solver;

namespace AdventSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var solvers = Assembly.GetExecutingAssembly().GetTypes()
                .Select(type => new Solver(type))
                .Where(solver => solver.IsSolver)
                .OrderBy(solver => solver.Attribute.DayToSolve)
                .ToList();

            bool disableOther = solvers.Any(solver => solver.InDebug);
            if (disableOther)
                solvers = solvers
                    .Where(solver => solver.InDebug)
                    .ToList();

            foreach (var solver in solvers)
            {
                int dayToSolve = solver.Attribute.DayToSolve;
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"Advent Challenge Day {dayToSolve}");
                solver.Instance.Solve();
                Console.WriteLine("-------------------------------------");
            }

            stopwatch.Stop();
            Console.WriteLine($"Run for {stopwatch.ElapsedMilliseconds}");
        }

        private class Solver
        {
            private readonly Type solverType;

            public Solver(Type type)
            {
                solverType = type;
                Attribute = type.GetCustomAttribute<AdventSolverAttribute>();
            }

            internal AdventSolverAttribute Attribute { get; }

            internal IAdventSolver Instance => IsSolver
                ? (IAdventSolver) Activator
                    .CreateInstance(solverType)
                : null;

            internal bool IsSolver => Attribute != null;
            public bool InDebug => Attribute.InDebug;
        }
    }
}