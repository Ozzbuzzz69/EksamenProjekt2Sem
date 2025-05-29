using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class Sandwich : Food
    {
        [Display(Name = "Kategori")]
        public string Category { get; set; }

        public Sandwich()
        { }

        public Sandwich(string? ingredients, string? inSeason, string meatType, double price, string category) : base(ingredients, inSeason, meatType, price)
        {
            Category = category;
        }
    }
}