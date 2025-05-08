using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.MockData
{
    public class MockFood
    {

        private static List<Sandwich> _sandwiches = new List<Sandwich>()
        {
            new(1, "Ham and Cheese", null, "Ham", 19.95,"Standard"),
            new(2, "Chicken Salad", null, "Chicken", 29.95,"Standard"),
            new(3, "Gold Flakes", null, null, 59.95,"Luxury"),
            new(4, "Potato", true, null, 15, "Standard")
        };

        private static List<WarmMeal> _warmMeals = new List<WarmMeal>()
        {
            new(1, "Spaghetti Bolognese", null, "Beef", 49.95, 2),
            new(2, "Sweet Chicken Curry", null, "Chicken", 39.95, 1),
            new(3, "Vegetable Stir Fry", null, null, 29.95, 1),
            new(4, "Monster Burger", null, "Beef", 199.95, 6)
        };

        private static List<Food> _foods = new List<Food>();
        /// <summary>
        /// Returns a list of mock sandwiches.
        /// </summary>
        /// <returns></returns>
        public static List<Sandwich> GetSandwiches()
        {
            return _sandwiches;
        }
        /// <summary>
        /// Returns a list of mock warm meals.
        /// </summary>
        /// <returns></returns>
        public static List<WarmMeal> GetWarmMeals()
        {
            return _warmMeals;
        }
        /// <summary>
        /// Returns a list of mock foods, which is a combination of sandwiches and warm meals.
        /// </summary>
        /// <returns></returns>
        public static List<Food> GetFoods()
        {
            if (_foods.Count == 0)
            {
                foreach (var sandwich in _sandwiches)
                {
                    _foods.Add(sandwich);
                }
                foreach (var warmMeal in _warmMeals)
                {
                    _foods.Add(warmMeal);
                }
            }
            return _foods;
        }

    }
}
