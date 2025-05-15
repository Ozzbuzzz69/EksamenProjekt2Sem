using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class Order
    {
        private DateTime pickupTime;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public User? User { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderTime { get; } = DateTime.Now;

        [Required(ErrorMessage = "Der skal angives en afhentningstid")]
        [DataType(DataType.DateTime)]
        public DateTime? PickupTime {get;set;}

        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public Order()
        { }

        public Order(User? user, DateTime pickupTime)
        {
            User = user;
            PickupTime = pickupTime;
        }
    }
}
