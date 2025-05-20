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

            //_orders = _dbService.GetObjectsAsync().Result.ToList();
            _orders = MockOrder.GetOrders();
            //_dbService.SaveObjects(_orders);

            //if (_orders == null)
            //{
            //    _orders = MockOrder.GetOrders();
            //}
            //else
            //    _orders = _dbService.GetObjectsAsync().Result.ToList();
        }


        public void AddFoodToCart(Food food, int quantity)
        {
            var cart = ReadCart();

            // Find if the sandwich already exists in the cart
            var existingOrderLine = cart.OrderLines
                .FirstOrDefault(ol => ol.Food.Id == food.Id);

            if (existingOrderLine != null)
            {
                // If it exists, just increase the quantity
                existingOrderLine.Quantity = quantity;

            }
            else
            {
                // If not, add a new order line
                cart.OrderLines.Add(new OrderLine(quantity, food));

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

        //public Order ReadCart()
        //{
        //    return _cart;
        //}
        /// <summary>
        /// Adds the order object from argument to the database, and the _orders list.
        /// </summary>
        /// <param name="order"></param>
        public void CreateOrder(Order order)
        {
            _orders.Add(order);
            _dbService.AddObjectAsync(order).Wait();
        }
        /// <summary>
        /// Reads an order from the database by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Order?</returns>
        public Order? ReadOrder(int id)
        {
            foreach (Order order in _orders)
            {
                if (order.Id == id)
                {
                    return order;
                }
            }
            return null;
        }

        /// <summary>
        /// Reads all order objects from the database.
        /// </summary>
        /// <returns>List<Order></returns>
        public List<Order> ReadAllOrders()
        {
            return _orders;
        }
        /// <summary>
        /// Reads a list of orders which belongs to the user in argument.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List<Order>?</returns>
        public List<Order>? ReadAllOrdersByUser(User? user)
        {
            //if (user == null)
            //{
            //    return null;
            //}
            //List<Order> temp = new();
            //foreach (Order o in _orders)
            //{
            //    if (o.User.Id == user.Id)
            //    {
            //        temp.Add(o);
            //    }
            //}
            //if (temp.Count > 0)
            //{
            //    return temp;
            //}
            //return null;
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
        public void UpdateOrder(int id, Order order)
        {
            if (order != null)
            {
                foreach (Order o in _orders)
                {
                    if (o.Id == id)
                    {
                        o.User = order.User;
                        o.PickupTime = order.PickupTime;
                        o.OrderLines = order.OrderLines;
                    }
                }
                _dbService.UpdateObjectAsync(order).Wait();
            }
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
        /// <summary>
        /// Calculates the total price of the order by going through each orderline in the list to calculate the total price.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Total price for all orderlines</returns>
        /// <exception cref="Exception"></exception>
        public double CalculateTotalPrice(int id)
        {
            Order? order = ReadOrder(id);
            if (order != null)
            {
                return GetTotalPrice(order);
            }
            else
            {
                throw new Exception("Order not found");
            }


        }


        public void AddSandwichToCart(Sandwich sandwich, int quantity)
        {
            if (sandwich != null && quantity > 0 && quantity <= 50)
            {
                _cart.OrderLines.Add(new OrderLine
                {
                    Quantity = quantity,
                    Food = sandwich
                });
            }
        }

        public void AddWarmMealToCart(WarmMeal warmmeal, int quantity)
        {
            if (warmmeal != null && quantity > 0 && quantity <= 50)
            {
                _cart.OrderLines.Add(new OrderLine
                {
                    Quantity = quantity,
                    Food = warmmeal
                });
            }
        }

        //public OrderLine? ReadOrderLine(int orderLineFoodId, int quantity)
        //{
        //    OrderLine tempOrderLine;

        //    foreach (var orderLine in _cart.OrderLines)
        //    {
        //        if (orderLine.Food.Id == orderLineFoodId && orderLine.Quantity == quantity)
        //        {
        //            tempOrderLine = orderLine;
        //            return tempOrderLine;
        //        }
        //    }
        //    return null;
        //}

        //public void DeleteOrderLine(OrderLine orderLine)
        //{
        //    _cart.OrderLines.Remove(_cart.OrderLines.Find(ol => ol.Food.Id == orderLine.Food.Id && ol.Quantity == orderLine.Quantity));
        //}

        //public Order ReadCart()
        //{
        //    return _cart;
        //}

        public Order? ReadOrderByUserId(int userId)
        {
            foreach (Order order in _orders)
            {
                if (order.User.Id == userId)
                {
                    return order;
                }
            }
            return null;
        }
#region Sorting/Filtering functions
    #region Filtering functions


        /// <summary>
        /// Gets all orders with the same pickup time as the one given in argument.
        /// </summary>
        /// <param name="pickTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersPickupTime(DateTime pickTime)
        {
            return _orders.FindAll(o => o.PickupTime == pickTime).ToList();
        }

        /// <summary>
        /// Gets all orders with a pickup time higher than the one given in argument.
        /// </summary>
        /// <param name="pickTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersPickupTimeLower(DateTime pickTime)
        {
            return _orders.FindAll(o => o.PickupTime >= pickTime).ToList();
        }

        /// <summary>
        /// Gets all orders with a pickup time lower than the one given in argument.
        /// </summary>
        /// <param name="pickTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersPickupTimeUpper(DateTime pickTime)
        {
            return _orders.FindAll(o => o.PickupTime <= pickTime).ToList();
        }

        /// <summary>
        /// Gets all orders with a pickup time in the range given in arguments (including lower and upper).
        /// </summary>
        /// <param name="lowerTime"></param>
        /// <param name="upperTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersPickupTimeRange(DateTime lowerTime, DateTime upperTime)
        {
            return _orders.FindAll(o => o.PickupTime >= lowerTime && o.PickupTime <= upperTime).ToList();
        }


        /// <summary>
        /// Gets all orders with the same order time as the one given in argument.
        /// </summary>
        /// <param name="orderTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersOrderTime(DateTime orderTime)
        {
            return _orders.FindAll(o => o.OrderTime == orderTime).ToList();
        }

        /// <summary>
        /// Gets all orders with an order time higher than the one given in argument.
        /// </summary>
        /// <param name="orderTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersOrderTimeLower(DateTime orderTime)
        {
            return _orders.FindAll(o => o.OrderTime >= orderTime).ToList();
        }

        /// <summary>
        /// Gets all orders with an order time lower than the one given in argument.
        /// </summary>
        /// <param name="orderTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersOrderTimeUpper(DateTime orderTime)
        {
            return _orders.FindAll(o => o.OrderTime <= orderTime).ToList();
        }

        /// <summary>
        /// Gets all orders with an order time in the range given in arguments (including lower and upper).
        /// </summary>
        /// <param name="lowerTime"></param>
        /// <param name="upperTime"></param>
        /// <returns></returns>
        public List<Order> FilterOrdersOrderTimeRange(DateTime lowerTime, DateTime upperTime)
        {
            return _orders.FindAll(o => o.OrderTime >= lowerTime && o.OrderTime <= upperTime).ToList();
        }
    #endregion

    #region Sorting functions

        /// <summary>
        /// Gets all orders sorted by id.
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOrdersSortedById()
        {
            return SortById(_orders);
        }

        /// <summary>
        /// Gets all orders sorted by pickuptime.
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOrdersSortedByPickupTime()
        {
            return SortByCriteria(_orders, "PickupTime");
        }

        /// <summary>
        /// Gets all orders sorted by ordertime.
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOrdersSortedByOrderTime()
        {
            return SortByCriteria(_orders, "OrderTime");
        }
    #endregion
        
    #region Sorting Orderline functions

        /// <summary>
        /// Sorts the orderlines in a given order by price in ascending order.
        /// </summary>
        /// <param name="order"></param>
        public void SortOrderlinesByPrice(Order order)
        {
            order.OrderLines = order.OrderLines.OrderBy(ol => ol.Price).ToList();
        }
        /// <summary>
        /// Sorts the orderlines in a given order by price in descending order.
        /// </summary>
        /// <param name="order"></param>
        public void SortOrderlinesByPriceDescending(Order order)
        {
            order.OrderLines = order.OrderLines.OrderByDescending(ol => ol.Price).ToList();
        }
        /// <summary>
        /// Sorts the orderlines in a given order by quantity in ascending order.
        /// </summary>
        /// <param name="order"></param>
        public void SortOrderlinesByQuantity(Order order)
        {
            order.OrderLines = order.OrderLines.OrderBy(ol => ol.Quantity).ToList();
        }
        /// <summary>
        /// Sorts the orderlines in a given order by quantity in descending order.
        /// </summary>
        /// <param name="order"></param>
        public void SortOrderlinesByQuantityDescending(Order order)
        {
            order.OrderLines = order.OrderLines.OrderByDescending(ol => ol.Quantity).ToList();
        }
        /// <summary>
        /// Sorts the orderlines in a given order by id in ascending order.
        /// </summary>
        /// <param name="order"></param>
        public void SortOrderlinesById(Order order)
        {
            order.OrderLines = order.OrderLines.OrderBy(ol => ol.Id).ToList();
        }
        /// <summary>
        /// Sorts the orderlines in a given order by id in descending order.
        /// </summary>
        /// <param name="order"></param>
        public void SortOrderlinesByIdDescending(Order order)
        {
            order.OrderLines = order.OrderLines.OrderByDescending(ol => ol.Id).ToList();
        }
    #endregion
#endregion

#region Orderline manipulation
        /// <summary>
        /// Calculates the total price of a given orderline.
        /// </summary>
        /// <param name="orderLine"></param>
        /// <returns></returns>
        public double GetTotalPriceLine(OrderLine orderLine)
        {
            return orderLine.Price * orderLine.Quantity;
        }
        /// <summary>
        /// Adds the orderline object from argument to the order object given by Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderLine"></param>
        public void AddOrderLine(int orderId, OrderLine orderLine)
        {
            Order? order = ReadOrder(orderId);
            if (order != null)
            {
                order.OrderLines.Add(orderLine);
                _dbService.UpdateObjectAsync(order).Wait();
            }
        }

        /// <summary>
        /// Adds the orderline object from argument to the order object given by Id
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderLine"></param>
        public void AddOrderLineToOrder(Order order, OrderLine orderLine)
        {
            if (order != null)
            {
                order.OrderLines.Add(orderLine);
            }
        }
        /// <summary>
        /// Gets the orderlines for an Order object from the id in argument
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>Orderline</returns>
        public List<OrderLine>? GetOrderLines(int orderId)
        {
            Order? order = ReadOrder(orderId);
            if (order != null)
            {
                return order.OrderLines;
            }
            return null;
        }
        /// <summary>
        /// Updates the orderline from ids in argument to the orderline given in argument
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderLineId"></param>
        /// <param name="orderLine"></param>
        public void UpdateOrderLine(int orderId, int orderLineId, OrderLine orderLine)
        {
            Order? order = ReadOrder(orderId);
            foreach (var ol in order.OrderLines)
            {
                if (ol.Id == orderLineId)
                {
                    ol.Quantity = orderLine.Quantity;
                    ol.Food = orderLine.Food;
                    ol.CampaignOffer = orderLine.CampaignOffer;
                    _dbService.UpdateObjectAsync(order).Wait();
                }
            }
        }
        /// <summary>
        /// Removes the orderline object from argument to the order object given by Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderLineId"></param>
        public void RemoveOrderLine(int orderId, int orderLineId) //Maybe needs to have a return value like the other remove
        {
            Order? order = ReadOrder(orderId);
            if (order != null)
            {
                order.OrderLines.Remove(GetOrderLine(order, orderLineId));
                _dbService.UpdateObjectAsync(order).Wait();
            }
        }
#endregion

#region Former model:
        /// <summary>
        /// Calculates the total price based on the order lines.
        /// </summary>
        /// <returns>0 or Orderlines total</returns>
        public double GetTotalPrice(Order order)
        {
            double totalPrice = 0;
            foreach (var orderLine in order.OrderLines)
            {
                totalPrice += orderLine.Price * orderLine.Quantity;
            }
            return totalPrice;
        }
        /// <summary>
        /// Gets the orderline object from id in argument
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Orderline</returns>
        public OrderLine? GetOrderLine(Order order, int id)
        {
            foreach (var orderLine in order.OrderLines)
            {
                if (orderLine.Id == id)
                {
                    return orderLine;
                }
            }
            return null;
        }
#endregion

    }
}
