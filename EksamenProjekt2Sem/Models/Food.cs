using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public abstract class Food
    {
        
        public int Id { get; set; }
        public string Ingredients { get; set; }
        public bool? InSeason { get; set; }
        public string? MeatType { get; set; }
        public double? Price { get; set; }

        public Food()
        { }

        public Food(int id, string ingredients, bool? inSeason, string? meatType, double? price)
        {
            Id = id;
            Ingredients = ingredients;
            InSeason = inSeason;
            MeatType = meatType;
            Price = price;
        }
    }
}
