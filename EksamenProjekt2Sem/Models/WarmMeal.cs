namespace EksamenProjekt2Sem.Models
{
    public class WarmMeal : Food
    {
        public int MinPersonAmount { get; set; }

        public WarmMeal()
        { }

        /// <summary>
        /// Constructor for WarmMeal class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ingredients"></param>
        /// <param name="inSeason"></param>
        /// <param name="meatType"></param>
        /// <param name="price"></param>
        /// <param name="minPersonAmount"></param>
        public WarmMeal(int id, string ingredients, bool? inSeason, string? meatType, double? price, int minPersonAmount) : base(id, ingredients, inSeason, meatType, price)
        {
            MinPersonAmount = minPersonAmount;
        }
    }
}
