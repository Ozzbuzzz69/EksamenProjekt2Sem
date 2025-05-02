using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class OrderService : GenericDbService<Order>
    {
        private List<Order> _orders; // Overskud fra domain model
        private GenericDbService<Order> _dbService; // Overskud fra domain model

        public OrderService(OrderService orderService)
        {
            _orders = new List<Order>();
            _dbService = orderService;
        }
        public OrderService()
        {
            _orders = new List<Order>();
            _dbService = new GenericDbService<Order>();
        }
        public void CreateOrder(Order order)
        {
            // Add order to Database
        }
        public Order ReadOrder(int id)
        {
            // Read order from Database
            return new Order(); // Placeholder return
        }
        public List<Order> ReadAllOrders()
        {
            // Read all orders from Database
            return new List<Order>(); // Placeholder return
        }
        public void UpdateOrder(int id, Order order)
        {
            // Update order by id in Database
        }
        public Order DeleteOrder(int id)
        {
            // Delete order from Database
            return new Order(); // Placeholder return
        }
        public double CalculateTotalPrice(int id)
        {
            // Go through each orderline in the list to calculate the total price

            return 0; // Placeholder return
        }
    }
}
