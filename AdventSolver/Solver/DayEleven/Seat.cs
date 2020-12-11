namespace AdventSolver.Solver.DayEleven
{
    public class Seat
    {
        private readonly Seat[,] map;
        private readonly int posX;
        private readonly int posY;

        private SeatPlacement nextState;

        public Seat(char init, int posX, int posY, Seat[,] map)
        {
            this.posX = posX;
            this.posY = posY;
            this.map = map;
            State = init switch
            {
                'L' => SeatPlacement.Empty,
                '#' => SeatPlacement.Occupied,
                _ => SeatPlacement.Floor
            };
            nextState = State;
        }

        public SeatPlacement State { get; private set; }

        public bool IsSatisfied { get; private set; }

        public void Execute()
        {
            IsSatisfied = true;
            State = nextState;
        }

        #region Rule One

        public void CalculateRuleOne()
        {
            if (State == SeatPlacement.Floor)
                return;

            int occupied = GetOccupiedNeighbors();

            if (State == SeatPlacement.Empty && occupied == 0)
            {
                IsSatisfied = false;
                nextState = SeatPlacement.Occupied;
            }

            if (State == SeatPlacement.Occupied && occupied >= 4)
            {
                IsSatisfied = false;
                nextState = SeatPlacement.Empty;
            }
        }

        private int GetOccupiedNeighbors()
        {
            int occupied = 0;

            for (int y = 0; y < 3; y++)
            {
                int calcY = posY - 1 + y;
                if (calcY >= map.GetLength(0) || calcY < 0)
                    continue;

                for (int x = 0; x < 3; x++)
                {
                    int calcX = posX - 1 + x;
                    if (calcX >= map.GetLength(1) || calcX < 0)
                        continue;

                    var state = map[calcY, calcX].State;
                    if (state == SeatPlacement.Occupied)
                        occupied++;
                }
            }

            if (State == SeatPlacement.Occupied)
                occupied--;

            return occupied;
        }

        #endregion

        #region Rule Two

        public void CalculateRuleTwo()
        {
            if (State == SeatPlacement.Floor)
                return;

            int occupied = 0;
            occupied += HasNeighbor(1, 0) ? 1 : 0;
            occupied += HasNeighbor(-1, 0) ? 1 : 0;
            occupied += HasNeighbor(0, 1) ? 1 : 0;
            occupied += HasNeighbor(0, -1) ? 1 : 0;
            occupied += HasNeighbor(-1, -1) ? 1 : 0;
            occupied += HasNeighbor(1, 1) ? 1 : 0;
            occupied += HasNeighbor(-1, 1) ? 1 : 0;
            occupied += HasNeighbor(1, -1) ? 1 : 0;

            if (State == SeatPlacement.Empty && occupied == 0)
            {
                IsSatisfied = false;
                nextState = SeatPlacement.Occupied;
            }

            if (State == SeatPlacement.Occupied && occupied >= 5)
            {
                IsSatisfied = false;
                nextState = SeatPlacement.Empty;
            }
        }

        private bool HasNeighbor(int xInc, int yInc)
        {
            int multi = 0;
            while (true)
            {
                multi++;
                int calcX = posX + xInc * multi;
                int calcY = posY + yInc * multi;

                if (calcX >= map.GetLength(1) || calcX < 0)
                    return false;
                if (calcY >= map.GetLength(0) || calcY < 0)
                    return false;
                var state = map[calcY, calcX].State;
                switch (state)
                {
                    case SeatPlacement.Empty:
                        return false;
                    case SeatPlacement.Occupied:
                        return true;
                }
            }
        }

        #endregion
    }
}