using System.Collections.Generic;

namespace AdventSolver.Util
{
    public static class EnumerableExtension
    {
        public static long Multiply(this IEnumerable<long> values)
        {
            long multiply = 0;
            bool isSet = false;

            foreach (long value in values)
            {
                if (!isSet)
                {
                    isSet = true;
                    multiply = value;
                    continue;
                }

                multiply *= value;
            }

            return multiply;
        }
    }
}