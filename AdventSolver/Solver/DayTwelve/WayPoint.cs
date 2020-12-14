using System;

namespace AdventSolver.Solver.DayTwelve
{
    public class WayPoint
    {
        public int YAxis { get; private set; }

        public int XAxis { get; private set; }

        public WayPoint(int initX = 0, int initY = 0)
        {
            XAxis = initX;
            YAxis = initY;
        }

        public void Drive(Direction direction, int amount)
        {
            YAxis = direction switch
            {
                Direction.North => YAxis + amount,
                Direction.East => YAxis,
                Direction.South => YAxis - amount,
                Direction.West => YAxis,
            };
            XAxis = direction switch
            {
                Direction.North => XAxis,
                Direction.East => XAxis + amount,
                Direction.South => XAxis,
                Direction.West => XAxis - amount,
            };
        }

        public void RotateOrigin()
        {
            double s = Math.Sin(-90 * Math.PI / 180);
            double c = Math.Cos(-90 * Math.PI / 180);

            int newX = (int)Math.Round(XAxis * c - YAxis * s);
            int newY = (int)Math.Round(XAxis * s + YAxis * c);

            XAxis = newX;
            YAxis = newY;
        }
    }
}