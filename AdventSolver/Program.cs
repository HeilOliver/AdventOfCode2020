﻿using System;
using System.Linq;
using System.Reflection;
using AdventSolver.Solver;
using AdventSolver.Solver.DayFour;

namespace AdventSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var solvers = Assembly.GetExecutingAssembly().GetTypes()
                .Select(type => new Solver(type))
                .Where(solver => solver.IsSolver)
                .OrderBy(solver => solver.Attribute.DayToSolve)
                .ToList();

            foreach (var solver in solvers)
            {
                int dayToSolve = solver.Attribute.DayToSolve;
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"Advent Challenge Day {dayToSolve}");
                solver.Instance.Solve();
                Console.WriteLine("-------------------------------------");
            }
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
                ? (IAdventSolver)Activator
                    .CreateInstance(solverType)
                : null;

            internal bool IsSolver => Attribute != null;
        }
    }
}