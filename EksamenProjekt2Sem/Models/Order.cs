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

        private DateTime pickupTime;

        [Required(ErrorMessage = "Der skal angives en afhentningstid")]
        [DataType(DataType.DateTime)]
        public DateTime PickupTime
        {
            get { return pickupTime; }
            set
            {
                //var minTime = new TimeSpan(8, 0, 0);
                //var maxTime = new TimeSpan(20, 0, 0);
                //if (value.TimeOfDay < minTime || value.TimeOfDay > maxTime)
                //{
                //    throw new ArgumentException("Tiden skal være mellem 8 og 20");
                //}
                value = pickupTime;
            }
        }

        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public Order()
        { }

        public Order(User user, DateTime pickupTime)
        {
            User = user;
            PickupTime = pickupTime;
        }
    }
}
