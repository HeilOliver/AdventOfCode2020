namespace AdventSolver.Solver.DayNineteen
{
    public class RuleRef
    {
        public RuleRef(int ref0, int? ref1 = null, int? ref2 = null)
        {
            Ref0 = ref0;
            Ref1 = ref1;
            Ref2 = ref2;
        }

        public int Ref0 { get; }
        public int? Ref1 { get; }
        public int? Ref2 { get; }
    }
}