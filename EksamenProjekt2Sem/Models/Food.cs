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

        /// <summary>
        /// Constructor for Food class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ingredients"></param>
        /// <param name="inSeason"></param>
        /// <param name="meatType"></param>
        /// <param name="price"></param>
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
