using System;
using System.Collections.Generic;
using System.Linq;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayTwentyOne
{
    [AdventSolver(21)]
    public class DayTwentyOneSolver : SolverBase, IAdventSolver
    {
        public DayTwentyOneSolver() : base("Data\\Day21.txt")
        {
        }

        public void Solve()
        {
            var foodData = GetFood();

            var allergenInIngredient = GetAllergenInIngredient(foodData);

            int ingredientsWithoutAllergen = foodData
                .SelectMany(food => food.Ingredients)
                .Count(ingredient => !allergenInIngredient.Values.Contains(ingredient));
            Console.WriteLine($"{ingredientsWithoutAllergen} ingredients have no allergens in it");

            var sortedIngredients = allergenInIngredient
                .OrderBy(e => e.Key)
                .Select(e => e.Value)
                .AsEnumerable();
            string dangerousIngredient = string.Join(',', sortedIngredients);
            Console.WriteLine($"{dangerousIngredient} is the canonical dangerous ingredient list!");
        }

        private ICollection<Food> GetFood()
        {
            return GetDataInput()
                .Select(line => new Food(line))
                .ToList();
        }

        private static IDictionary<string, string> GetAllergenInIngredient(IEnumerable<Food> foodList)
        {
            var allergens = new Dictionary<string, HashSet<string>>();

            foreach (var food in foodList)
            foreach (string allergen in food.Allergens)
            {
                if (!allergens.ContainsKey(allergen))
                {
                    allergens.Add(allergen, new HashSet<string>());
                    allergens[allergen].AddRange(food.Ingredients);
                    continue;
                }

                allergens[allergen]
                    .IntersectWith(food.Ingredients);
            }

            var alreadySeen = new HashSet<string>();
            while (alreadySeen.Count < allergens.Count)
            {
                (string allergen, var inIngredients) = allergens
                    .Where(kv => !alreadySeen.Contains(kv.Key))
                    .First(kv => kv.Value.Count == 1);

                alreadySeen.Add(allergen);
                string determinedIngredient = inIngredients.First();

                allergens
                    .Where(al => !al.Key.Equals(allergen))
                    .Where(kv => kv.Value.Count > 1)
                    .Select(kv => kv.Value)
                    .ForEach(ingredients => ingredients.Remove(determinedIngredient));
            }

            return allergens
                .ToDictionary(k => k.Key, v => v.Value.First());
        }
    }
}