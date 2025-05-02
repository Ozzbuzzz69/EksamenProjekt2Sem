using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class Order
    {
        
        public int Id { get; set; }

        public User User { get; set; }

        
        public DateTime PickupTime { get; set; }

        public double TotalPrice { get; set; }

        private List<OrderLine> orderLines = new List<OrderLine>();

        public Order()
        { }

        public Order(int id, User user, DateTime pickupTime, double totalPrice)
        {
            Id = id;
            User = user;
            PickupTime = pickupTime;
            TotalPrice = totalPrice;
        }
    }
}
