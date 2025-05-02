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

        public DateTime OrderTime { get; } = DateTime.Now;
        public DateTime PickupTime { get; set; }

        public double TotalPrice { get { return GetTotalPrice(); } }

        private List<OrderLine> orderLines = new List<OrderLine>();


        //Lav orderline funktion
        public Order()
        { }

        /// <summary>
        /// Constructor for Order class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="pickupTime"></param>
        /// <param name="totalPrice"></param>
        public Order(User user, DateTime pickupTime)
        {
            User = user;
            PickupTime = pickupTime;
        }
       
        /// <summary>
        /// Calculates the total price based on the order lines.
        /// </summary>
        /// <returns>0 or Orderlines total</returns>
        public double GetTotalPrice()
        {
            double totalPrice = 0;
            foreach (var orderLine in orderLines)
            {
                totalPrice += orderLine.Price * orderLine.Quantity;
            }
            return totalPrice;
        }
    }
}
