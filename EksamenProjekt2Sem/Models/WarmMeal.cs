using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EksamenProjekt2Sem.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(WarmMeal), "warmmeal")]
    public class WarmMeal : Food
    {
        public int MinPersonAmount { get; set; }

        public WarmMeal()
        { }

       
        public WarmMeal(int id, string ingredients, string? inSeason, string meatType, double price, int minPersonAmount) : base(id, ingredients, inSeason, meatType, price)
        {
            MinPersonAmount = minPersonAmount;
        }
    }
}
