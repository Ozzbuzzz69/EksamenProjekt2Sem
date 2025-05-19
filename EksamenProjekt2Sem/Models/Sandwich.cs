using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EksamenProjekt2Sem.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Sandwich), "sandwich")]
    public class Sandwich : Food
    {
        [Display(Name = "Kategori")]
        public string? Category { get; set; }

       
        public Sandwich()
        { }

        
        public Sandwich( string? ingredients, string? inSeason, string meatType, double price, string category) : base( ingredients, inSeason, meatType, price)
        {
            Category = category;
        }
      
    }
}
