using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> toAdd)
        {
            foreach (var add in toAdd) 
                set.Add(add);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            HashSet<T> hashSet = null;
            foreach (var list in lists)
                if (hashSet == null)
                    hashSet = new HashSet<T>(list);
                else
                    hashSet.IntersectWith(list);
            return hashSet == null ? new List<T>() : hashSet.AsEnumerable();
        }

        public static bool RemoveRange<T>(this HashSet<T> set, IEnumerable<T> toRemove)
        {
            bool allRemoved = true;
            foreach (var remove in toRemove)
            {
                bool result = set.Remove(remove);
                if (!result)
                    allRemoved = false;
            }

            return allRemoved;
        }
    }
}