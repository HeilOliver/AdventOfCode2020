using System.Collections.Generic;
using System.IO;
using AdventSolver.Solver.DayTwo;

namespace AdventSolver.Solver
{
    public class SolverBase
    {
        public SolverBase(string dataFileName)
        {
            DataFileName = dataFileName;
        }

        public string DataFileName { get; }

        protected IEnumerable<string> GetDataInput()
        {
            if (!File.Exists(DataFileName))
                yield break;
            
            var lines = File.ReadAllLines(DataFileName);
            foreach (string line in lines)
            {
                yield return line;
            }
        }
    }
}