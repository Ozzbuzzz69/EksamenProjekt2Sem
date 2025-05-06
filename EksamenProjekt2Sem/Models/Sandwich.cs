using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class Sandwich : Food
    {
        public string Category { get; set; }

       
        public Sandwich()
        { }

        
        public Sandwich(string ingredients, bool? inSeason, string? meatType, double price, string category) : base(ingredients, inSeason, meatType, price)
        {
            Category = category;
        }
      
    }
}
