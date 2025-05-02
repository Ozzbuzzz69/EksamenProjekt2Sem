using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public abstract class Food
    {
        
        public int Id { get; private set; }
        public string Ingredients { get; private set; }
        public bool? InSeason { get; private set; }
        public string? MeatType { get; private set; }
        public double Price { get; private set; }

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
        public Food(int id, string ingredients, bool? inSeason, string? meatType, double price)
        {
            Id = id;
            Ingredients = ingredients;
            InSeason = inSeason;
            MeatType = meatType;
            Price = price;
        }
        /// <summary>
        /// Function to set otherwise private id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Function to set otherwise private ingredients
        /// </summary>
        /// <param name="ingredients"></param>
        public void SetIngredients(string ingredients)
        {
            Ingredients = ingredients;
        }
        /// <summary>
        /// Function to set otherwise private season status
        /// </summary>
        /// <param name="inSeason"></param>
        public void SetInSeason(bool? inSeason)
        {
            InSeason = inSeason;
        }
        /// <summary>
        /// Function to set otherwise private meat type
        /// </summary>
        /// <param name="meatType"></param>
        public void SetMeatType(string? meatType)
        {
            MeatType = meatType;
        }
        /// <summary>
        /// Function to set otherwise private price
        /// </summary>
        /// <param name="price"></param>
        public void SetPrice(double price)
        {
            Price = price;
        }
    }
}