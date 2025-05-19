using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EksamenProjekt2Sem.Services
{
    public class OrderService___Eksperimental
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";
        private List<Order> _orders; 
        private GenericDbService<Order> _dbService;

        //        public OrderService___Eksperimental(GenericDbService<Order> dbService, IHttpContextAccessor httpContextAccessor)
        //        {
        //            _httpContextAccessor = httpContextAccessor;
        //            _dbService = dbService;

        //            //_orders = _dbService.GetObjectsAsync().Result.ToList();
        //            _orders = MockOrder.GetOrders();
        //            //_dbService.SaveObjects(_orders);

        //            //if (_orders == null)
        //            //{
        //            //    _orders = MockOrder.GetOrders();
        //            //}
        //            //else
        //            //    _orders = _dbService.GetObjectsAsync().Result.ToList();
        //        }





        //        public void AddSandwichToCart(Sandwich sandwich, int quantity)
        //        {
        //            var cart = ReadCart();

        //            // Find if the sandwich already exists in the cart
        //            var existingOrderLine = cart.OrderLines
        //                .FirstOrDefault(ol => ol.Food.Id == sandwich.Id);

        //            if (existingOrderLine != null)
        //            {
        //                // If it exists, just increase the quantity
        //                existingOrderLine.Quantity = quantity;

        //            }
        //            else
        //            {
        //                // If not, add a new order line
        //                cart.OrderLines.Add(new OrderLine(quantity, sandwich));

        //            }
        //            SaveCart(cart);
        //        }




        //        public OrderLine? ReadOrderLine(int orderLineFoodId, int quantity)
        //        {
        //            var cart = ReadCart();
        //            foreach (var orderLine in cart.OrderLines)
        //            {
        //                if (orderLine.Food.Id == orderLineFoodId && orderLine.Quantity == quantity)
        //                {
        //                    return orderLine;
        //                }
        //            }
        //            return null;
        //        }



        //        public void DeleteOrderLine(OrderLine orderLine)
        //        {
        //            var cart = ReadCart();
        //            var toRemove = cart.OrderLines
        //                .FirstOrDefault(ol => ol.Food.Id == orderLine.Food.Id && ol.Quantity == orderLine.Quantity);
        //            if (toRemove != null)
        //            {
        //                cart.OrderLines.Remove(toRemove);
        //                SaveCart(cart);
        //            }
        //        }



        //        public Order ReadCart()
        //        {
        //            var session = _httpContextAccessor.HttpContext.Session;
        //            var cartJson = session.GetString(CartSessionKey);
        //            if (cartJson != null)
        //            {
        //                return JsonSerializer.Deserialize<Order>(cartJson);
        //            }
        //            return new Order();
        //        }

        //        public void SaveCart(Order cart)
        //        {
        //            var session = _httpContextAccessor.HttpContext.Session;
        //            var cartJson = JsonSerializer.Serialize(cart);
        //            session.SetString(CartSessionKey, cartJson);
        //        }



    }
}




