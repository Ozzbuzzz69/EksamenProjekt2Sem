using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class Order
    {

        public int Id { get; private set; }

        public User User { get; private set; }


        public DateTime PickupTime { get; private set; }

        public double TotalPrice { get { return GetTotalPrice(); } }

        private List<OrderLine> orderLines = new List<OrderLine>();

        public Order()
        { }

        /// <summary>
        /// Constructor for Order class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="pickupTime"></param>
        /// <param name="totalPrice"></param>
        public Order(int id, User user, DateTime pickupTime)
        {
            Id = id;
            User = user;
            PickupTime = pickupTime;
        }
        /// <summary>
        /// Function to set otherwise private id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Function to set otherwise private user
        /// </summary>
        /// <param name="user"></param>
        public void SetUser(User user)
        {
            User = user;
        }
        /// <summary>
        /// Function to set otherwise private pickup time.
        /// Checks for pickup time in the past.
        /// </summary>
        /// <param name="pickupTime"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetPickupTime(DateTime pickupTime)
        {
            if (pickupTime < DateTime.Now)
            {
                throw new ArgumentException("Pickup time cannot be in the past");
            }
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
