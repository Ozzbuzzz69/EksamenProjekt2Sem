using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class WarmMeal : Food
    {
        public int MinPersonAmount { get; set; }

        public WarmMeal()
        { }

       
        public WarmMeal(string ingredients, bool? inSeason, string? meatType, double price, int minPersonAmount) : base(ingredients, inSeason, meatType, price)
        {
            MinPersonAmount = minPersonAmount;
        }
    }
}
