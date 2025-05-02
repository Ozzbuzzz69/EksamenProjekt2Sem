using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class WarmMeal : Food
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MinPersonAmount { get; set; }

        public WarmMeal()
        { }

       
        public WarmMeal(int id, string ingredients, bool? inSeason, string? meatType, double price, int minPersonAmount) : base(ingredients, inSeason, meatType, price)
        {
            Id = id;
            MinPersonAmount = minPersonAmount;
        }
    }
}
