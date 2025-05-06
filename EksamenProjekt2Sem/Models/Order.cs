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
        public DateTime PickupTime
        {
            get; set
            {
                
                    if (PickupTime < DateTime.Now || PickupTime == DateTime.Now.AddDays(1))
                    {
                        throw new ArgumentNullException("Ugyldig dato");
                    }
                       
            }             
        }

        public double TotalPrice { get; set; }

        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

        public Order()
        { }

        
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
            foreach (var orderLine in OrderLines)
            {
                totalPrice += orderLine.Price * orderLine.Quantity;
            }
            return totalPrice;
        }
        /// <summary>
        /// Adds the orderline object from argument to the order object
        /// </summary>
        /// <param name="orderLine"></param>
        public void AddOrderLine(OrderLine orderLine)
        {
            OrderLines.Add(orderLine);
        }
        /// <summary>
        /// Removes the orderline object from argument to the order object
        /// </summary>
        /// <param name="orderLine"></param>
        public void RemoveOrderLine(int id)
        {
            OrderLines.Remove(GetOrderLine(id));
        }
        /// <summary>
        /// Gets the orderline object from id in argument
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Orderline</returns>
        public OrderLine? GetOrderLine(int id)
        {
            foreach (var orderLine in OrderLines)
            {
                if (orderLine.Id == id)
                {
                    return orderLine;
                }
            }
            return null;
        }
        /// <summary>
        /// Gets all orderlines from the order object
        /// </summary>
        /// <returns>All Orderlines</returns>
        public List<OrderLine> GetOrderLines()
        {
            return OrderLines;
        }
        /// <summary>
        /// Updates the orderline from id in argument to the orderline given in argument
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderLine"></param>
        public void UpdateOrderLine(int id, OrderLine orderLine)
        {
            foreach (var ol in OrderLines)
            {
                if (ol.Id == id)
                {
                    ol.Quantity = orderLine.Quantity;
                    ol.Price = orderLine.Price;
                    ol.Food = orderLine.Food;
                    ol.CampaignOffer = orderLine.CampaignOffer;
                }
            }
        }
    }
}
