namespace EksamenProjekt2Sem.Models
{
    public class WarmMeal : Food
    {
        public int MinPersonAmount { get; private set; }

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
        public WarmMeal(int id, string ingredients, bool? inSeason, string? meatType, double price, int minPersonAmount) : base(id, ingredients, inSeason, meatType, price)
        {
            MinPersonAmount = minPersonAmount;
        }
        /// <summary>
        /// Function to set otherwise private min person amount
        /// Checks if min person amount is less than 1 and throws an exception if it is
        /// </summary>
        /// <param name="minPersonAmount"></param>
        public void SetMinPersonAmount(int minPersonAmount)
        {
            if (minPersonAmount < 1)
            {
                throw new ArgumentException("Min person amount must be at least 1.");
            }
            MinPersonAmount = minPersonAmount;
        }
    }
}
