using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.MockData
{
    public class MockFood
    {

        private static List<Sandwich> _sandwiches = new List<Sandwich>()
        {
            new(1, "Roastbeef m/remoulade", null, "Okse", 27,"Standard"),
            new(2, "Leverpostej m/bacon og agurkesalat", null, "Gris", 27,"Standard"),
            new(3, "Fiskefilet m/remoulade", null, "Fisk", 27,"Standard"),
            new(4, "Kartoffelmad, purløg og bacon", "*", "Gris", 35, "Luksus"),
            new(5, "Hønsesalat m/bacon", null, "Kylling, Gris", 35, "Luksus"),
            new(6, "Dyrlægens natmad", null, "Okse, Gris", 35, "Luksus"),
            new(7, "Roastbeef, hj. lavet æble-blomme relish, bernaisecreme, rodfrugtchips", null, "Okse", 49, "Signatur"),
            new(8, "Skinkesalat, lufttørret skinke, vesterhavsost", null, "Gris", 49, "Signatur"),
            new(9, "Hj. lavet andesylte", "*", "And", 49, "Signatur")
        };

        private static List<WarmMeal> _warmMeals = new List<WarmMeal>()
        {
            new(1, "Frikadeller m/kartoffelsalat", null, "Gris", 39, 4),
            new(2, "Kogt skinke m/flødekartofler el. kartoffelsalat, grøn salat m/dressing og flutes", null, "Gris", 79, 6),
            new(3, "Kalveculotte m/flødekartofler eller kartoffelsalat, grøn salat m/dressing og flutes", null, "Kalv", 99, 6),
            new(4, "Marineret kalveculotte (sous vide), marineret svinekam (sous vide), marinerede kartofler, mix salat, broccolisalat m/bacon, flødekartofler, baguette, smør, dressing", null, "Kalv, Gris", 129.5, 10)
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
