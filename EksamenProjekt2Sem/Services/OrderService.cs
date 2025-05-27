using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace EksamenProjekt2Sem.Services
{
    public class OrderService : GenericDbService<Order>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";
        private List<Order> _orders; // Overskud fra domain model
        private GenericDbService<Order> _dbService; // Overskud fra domain model
        private Order _cart = new Order();

        public OrderService(GenericDbService<Order> dbService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbService = dbService;
            try
            {
                _orders = _dbService.GetObjectsAsync().Result.ToList();
                if (_orders == null || _orders.Count() < 1)
                {
                    SeedOrderAsync().Wait();
                    _orders = _dbService.GetObjectsAsync().Result.ToList();
                }
            }
            catch (AggregateException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
            }
            if (_orders == null)
            {
                _orders = new();
            }

            //_orders = _dbService.GetObjectsAsync().Result.ToList();
            //_orders = MockOrder.GetOrders();
            //_dbService.SaveObjects(_orders);

            if (_orders == null)
            {
                _orders = MockOrder.GetOrders();
            }
            else
                _orders = _dbService.GetObjectsAsync().Result.ToList();
        }
       
        public async Task SeedOrderAsync()
        {
            _orders = new List<Order>();
            var order = MockOrder.GetOrders();
            await _dbService.SaveObjects(order);
        }

        public void AddFoodToCart(Food food, int quantity)
        {
            var cart = ReadCart();

            // Ensure OrderLines is initialized
            if (cart.OrderLines == null)
                cart.OrderLines = new List<OrderLine>();

            // Find if the food already exists in the cart (by type and id)
            var existingOrderLine = cart.OrderLines
                .FirstOrDefault(ol => ol.Food.Id == food.Id && ol.Food.GetType() == food.GetType());

            if (existingOrderLine != null)
            {
                // If it exists, increase the quantity
                existingOrderLine.Quantity = quantity;
            }
            else
            {
                // If not, add a new order line
                cart.OrderLines.Add(new OrderLine(quantity, food));
            }
            SaveCart(cart);
        }
        public void AddOfferToCart(CampaignOffer offer, int quantity)
        {
            var cart = ReadCart();

            if (cart.OrderLines == null)
                cart.OrderLines = new List<OrderLine>();

            // Find if the sandwich already exists in the cart
            var existingOrderLine = cart.OrderLines
                .FirstOrDefault(ol => ol.CampaignOffer.Id == offer.Id);

            if (existingOrderLine != null)
            {
                // If it exists, just increase the quantity
                existingOrderLine.Quantity = quantity;

            }
            else
            {
                // If not, add a new order line
                cart.OrderLines.Add(new OrderLine(quantity, offer));

            }
            SaveCart(cart);
        }




        public OrderLine? ReadOrderLine(int orderLineFoodId, int quantity)
        {
            var cart = ReadCart();
            foreach (var orderLine in cart.OrderLines)
            {
                if (orderLine.Food.Id == orderLineFoodId && orderLine.Quantity == quantity)
                {
                    return orderLine;
                }
            }
            return null;
        }



        public void DeleteOrderLine(OrderLine orderLine)
        {
            var cart = ReadCart();
            var toRemove = cart.OrderLines
                .FirstOrDefault(ol => ol.Food.Id == orderLine.Food.Id && ol.Quantity == orderLine.Quantity);
            if (toRemove != null)
            {
                cart.OrderLines.Remove(toRemove);
                SaveCart(cart);
            }
        }



        public Order ReadCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = session.GetString(CartSessionKey);
            if (cartJson != null)
            {
                return JsonSerializer.Deserialize<Order>(cartJson);
            }
            return new Order();
        }

        public void SaveCart(Order cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = JsonSerializer.Serialize(cart);
            session.SetString(CartSessionKey, cartJson);
        }


        /// <summary>
        /// Adds the order object from argument to the database, and the _orders list.
        /// </summary>
        /// <param name="order"></param>
        public async Task CreateOrder(Order order)
        {
            _orders.Add(order);
            await _dbService.AddObjectAsync(order);
        }



        /// <summary>
        /// Reads an order from the database by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Order?</returns>
        public Order? ReadOrder(int id)
        {
            Order? order = _dbService.GetObjectsAsync().Result.FirstOrDefault(s => s.Id == id);
            return order;
        }
   

        /// <summary>
        /// Reads a list of orders which belongs to the user in argument.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List<Order>?</returns>
        public List<Order>? ReadAllOrdersByUser(User? user)
        {
           
            if (user == null)
            {
                return null;
            }
            List<Order> temp = new();
            foreach (Order o in _orders)
            {
                if (o.User.Email == user.Email)
                {
                    temp.Add(o);
                }
            }
            if (temp.Count > 0)
            {
                return temp;
            }
            return null;
        }

        /// <summary>
        /// Updates the order object from argument to the database, and the _orders list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        public void UpdateOrder(Order order)
        {
            if (order == null)
                return;

            var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder == null)
                return;

            existingOrder.User = order.User;
            existingOrder.PickupTime = order.PickupTime;
            existingOrder.OrderLines = order.OrderLines;


            _dbService.UpdateObjectAsync(existingOrder).Wait(); // Only update if the meal exists
        }
        /// <summary>
        /// Deletes the order object from argument to the database, and the _orders list. Returns the deleted order.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The deleted Order/null</returns>
        public Order? DeleteOrder(int id)
        {
            Order? ToBeDeleted = null;
            foreach (Order order in _orders)
            {
                if (order.Id == id)
                {
                    ToBeDeleted = order;
                    break;
                }
            }
            if (ToBeDeleted != null)
            {
                _orders.Remove(ToBeDeleted);
                _dbService.DeleteObjectAsync(ToBeDeleted).Wait();
            }
            return ToBeDeleted; // Return the deleted order
        }


    }
}
