namespace EksamenProjekt2Sem.Models
{
    public class Sandwich : Food
    {
        public string Category { get; private set; }
        public Sandwich()
        { }

        /// <summary>
        /// Constructor for Sandwich class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ingredients"></param>
        /// <param name="inSeason"></param>
        /// <param name="meatType"></param>
        /// <param name="price"></param>
        /// <param name="category"></param>
        public Sandwich(int id, string ingredients, bool? inSeason, string? meatType, double price, string category) : base(id, ingredients, inSeason, meatType, price)
        {
            Category = category;
        }
        /// <summary>
        /// Function to set otherwise private category
        /// </summary>
        /// <param name="category"></param>
        public void SetCategory(string category)
        {
            Category = category;
        }
    }
}
