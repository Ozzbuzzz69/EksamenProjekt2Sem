using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class OrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        public Food Food { get; set; }
        public CampaignOffer CampaignOffer { get; set; }
       
        
        
        public int CampaignOfferId { get; set; }

        public OrderLine()
        { }

        
        public OrderLine(object foodObj, int quantity)
        {
            Quantity = quantity;
        }
    }
}
