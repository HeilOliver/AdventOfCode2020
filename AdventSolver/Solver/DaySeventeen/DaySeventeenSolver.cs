using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using AdventSolver.Solver.DaySixteen;
using AdventSolver.Util;

namespace AdventSolver.Solver.DaySeventeen
{
    [AdventSolver(17, InDebug = true)]
    public class DaySeventeenSolver : SolverBase, IAdventSolver
    {
        public DaySeventeenSolver() : base("Data\\Day17.txt")
        {
        }

        public void Solve()
        {
            CubePosition flyLight = new CubePosition();
            var cubePositions = LoadData();
            for (int i = 0; i < 6; i++)
            {
                cubePositions = DoCycles(cubePositions, flyLight, false);
            }

            Console.WriteLine($"{cubePositions.Count} cubes are active after six iterations with three dimensions");

            cubePositions = LoadData();
            for (int i = 0; i < 6; i++)
            {
                cubePositions = DoCycles(cubePositions, flyLight, true);
            }

            Console.WriteLine($"{cubePositions.Count} cubes are active after six iterations with four dimensions");
        }

        private ICollection<CubePosition> LoadData()
        {
            var dataInput = GetDataInput();
            var set = new HashSet<CubePosition>();

            int yPos = 0;
            foreach (string line in dataInput)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '.')
                        continue;

                    set.Add(new CubePosition()
                    {
                        X = x,
                        Y = yPos,
                        Z = 0
                    });
                }

                yPos++;
            }

            return set;
        }

        private ICollection<CubePosition> DoCycles(ICollection<CubePosition> activeCubes, CubePosition flyLight,
            bool includeW)
        {
            ICollection<CubePosition> newState = new ConcurrentHashSet<CubePosition>();
            ICollection<CubePosition> mayActive = new ConcurrentHashSet<CubePosition>();
            
            activeCubes.AsParallel().ForAll(currentCube =>
            {
                int active = 0;

                foreach (var toCheckPos in GetNeighbors(currentCube, new CubePosition(), includeW))
                {
                    if (toCheckPos.Equals(currentCube))
                        continue;
                    if (activeCubes.Contains(toCheckPos))
                        active++;
                    else
                        mayActive.Add(new CubePosition()
                        {
                            X = toCheckPos.X,
                            Y = toCheckPos.Y,
                            Z = toCheckPos.Z,
                            W = toCheckPos.W
                        });
                }

                if (active >= 2 && active <= 3)
                    newState.Add(currentCube);
            });

            mayActive.AsParallel().ForAll(currentCube =>
            {
                int active = GetNeighbors(currentCube, new CubePosition(), includeW)
                    .Where(toCheckPos => !toCheckPos.Equals(currentCube))
                    .Count(activeCubes.Contains);

                if (active == 3)
                    newState.Add(currentCube);
            });

            return newState;
        }

        private IEnumerable<CubePosition> GetNeighbors(CubePosition cubePosition, CubePosition flyLight, bool includeW)
        {
            int cubeX = cubePosition.X;
            int cubeY = cubePosition.Y;
            int cubeZ = cubePosition.Z;
            int cubeW = cubePosition.W;

            for (int x = cubeX - 1; x <= cubeX + 1; x++)
            {
                flyLight.X = x;
                for (int y = cubeY - 1; y <= cubeY + 1; y++)
                {
                    flyLight.Y = y;
                    for (int z = cubeZ - 1; z <= cubeZ + 1; z++)
                    {
                        flyLight.Z = z;
                        for (int w = cubeW - 1; w <= cubeW + 1; w++)
                        {
                            if (includeW)
                                flyLight.W = w;

                            yield return flyLight;

                            if (!includeW)
                                break;
                        }
                    }
                }
            }
        }
    }
}