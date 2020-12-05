using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Util
{
    public static class DictionaryExtension
    {
        public static bool ContainsKeys<TK, TV>(this IDictionary<TK, TV> dictionary, IEnumerable<TK> keys)
        {
            return keys.All(dictionary.ContainsKey);
        }
    }
}