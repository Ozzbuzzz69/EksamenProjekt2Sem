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

        [DataType(DataType.DateTime)]
        public DateTime OrderTime { get; } = DateTime.Now;

        [Required(ErrorMessage = "Der skal angives en afhentningstid")]
        [DataType(DataType.DateTime)]
        public DateTime? PickupTime { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        public Order()
        {
            User = new();        
        }

        public Order(User user, DateTime pickupTime, List<OrderLine> orderLines)
        {
            User = user;
            PickupTime = pickupTime;
            if (orderLines != null)
            {
                OrderLines = orderLines;
            }
            else { OrderLines = new List<OrderLine>(); }
        }
    }
}
