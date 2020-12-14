namespace AdventSolver.Solver.DayTwelve
{
    public class Ferry
    {
        private readonly WayPoint position;
        private readonly WayPoint wayPoint;

        public Ferry(WayPoint wayPoint = null)
        {
            this.wayPoint = wayPoint;
            position = new WayPoint();
            Direction = Direction.East;
        }

        public Direction Direction { get; private set; }

        public int XPos => position.XAxis;
        public int YPos => position.YAxis;

        private void RotateCw()
        {
            int direction = ((int)Direction + 1) % 4;
            Direction = (Direction)direction;
        }

        private void RotateCcw()
        {
            int direction = ((int)Direction + 3) % 4;
            Direction = (Direction)direction;
        }

        public void Execute(Instruction instruction)
        {
            switch (instruction.Command)
            {
                case 'N':
                    position.Drive(Direction.North, instruction.Value);
                    break;
                case 'S':
                    position.Drive(Direction.South, instruction.Value);
                    break;
                case 'E':
                    position.Drive(Direction.East, instruction.Value);
                    break;
                case 'W':
                    position.Drive(Direction.West, instruction.Value);
                    break;
                case 'L':
                    for (int i = 0; i < instruction.Value; i++)
                        RotateCcw();
                    break;
                case 'R':
                    for (int i = 0; i < instruction.Value; i++)
                        RotateCw();
                    break;
                case 'F':
                    position.Drive(Direction, instruction.Value);
                    break;
            }
        }

        public void ExecuteWithWayPoint(Instruction instruction)
        {
            switch (instruction.Command)
            {
                case 'N':
                    wayPoint.Drive(Direction.North, instruction.Value);
                    break;
                case 'S':
                    wayPoint.Drive(Direction.South, instruction.Value);
                    break;
                case 'E':
                    wayPoint.Drive(Direction.East, instruction.Value);
                    break;
                case 'W':
                    wayPoint.Drive(Direction.West, instruction.Value);
                    break;

                case 'L':
                    for (int i = 0; i < 4 - instruction.Value; i++)
                        wayPoint.RotateOrigin();
                    break;
                case 'R':
                    for (int i = 0; i < instruction.Value; i++)
                        wayPoint.RotateOrigin();
                    break;

                case 'F':
                    int xVal = wayPoint.XAxis * instruction.Value;
                    int yVal = wayPoint.YAxis* instruction.Value;
                    position.Drive(Direction.East, xVal);
                    position.Drive(Direction.North, yVal);
                    break;
            }
        }
    }
}