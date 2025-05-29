using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public User User { get; set; }

        [Required(ErrorMessage = "Der skal angives en afhentningstid")]
        [DataType(DataType.DateTime)]
        public DateTime? PickupTime { get; set; }

        public Food Food { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }
        public int? FoodId { get; set; }

        public CampaignOffer CampaignOffer { get; set; }

        public int? CampaignOfferId { get; set; }

        public Order() { }


        public Order(User user, DateTime pickupTime, Food? food, CampaignOffer? campaignOffer)
        {
            User = user;
            PickupTime = pickupTime;
            Food = food;
            CampaignOffer = campaignOffer;
        }

    }
}
