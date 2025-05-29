namespace EksamenProjekt2Sem.Models
{
    public class WarmMeal : Food
    {
        public int MinPersonAmount { get; set; }

        public WarmMeal()
        { }

        public WarmMeal( string ingredients, string? inSeason, string meatType, double price, int minPersonAmount) : base( ingredients, inSeason, meatType, price)
        {
            MinPersonAmount = minPersonAmount;
        }
    }
}