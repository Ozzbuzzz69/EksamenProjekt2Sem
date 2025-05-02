using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class Sandwich : Food
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public Sandwich()
        { }

        
        public Sandwich(int id, string ingredients, bool? inSeason, string? meatType, double price) : base(ingredients, inSeason, meatType, price)
        {
            Id = id;
        }
      
    }
}
