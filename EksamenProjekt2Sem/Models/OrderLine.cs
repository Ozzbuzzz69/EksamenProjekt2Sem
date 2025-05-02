using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class OrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        [Required]
        public int Quantity { get; private set; }
        [Required]
        public double Price { get; set; }
        public Food Food { get; set; }
        public CampaignOffer CampaignOffer { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public int CampaignOfferId { get; set; }

        public OrderLine()
        { }

        
        public OrderLine(int id, int quantity)
        {
            Id = id;
            Quantity = quantity;
           
            
        }
    }
}
