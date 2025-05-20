using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.MockData
{
    public class MockFood
    {

        private static List<Sandwich> _sandwiches = new List<Sandwich>()
        {
            new(1 ,"Ham and Cheese", null, "Ham", 19.95,"Standard"),
            new(2, "Chicken Salad", null, "Chicken", 29.95,"Standard"),
            new(3, "Gold Flakes", null, "Chicken", 59.95,"Luxury"),
            new(4, "Potato", "*", "Chicken, Ham", 15, "Standard")
        };

        private static List<WarmMeal> _warmMeals = new List<WarmMeal>()
        {

            new("Frikadeller m/kartoffelsalat", null, "Gris", 39, 4),
            new("Kogt skinke m/flødekartofler el. kartoffelsalat, grøn salat m/dressing og flutes", null, "Gris", 79, 6),
            new("Kalveculotte m/flødekartofler eller kartoffelsalat, grøn salat m/dressing og flutes", null, "Kalv", 99, 6),
            new("Marineret kalveculotte (sous vide), marineret svinekam (sous vide), marinerede kartofler, mix salat, broccolisalat m/bacon, flødekartofler, baguette, smør, dressing", null, "Kalv, Gris", 129.5, 10)

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
