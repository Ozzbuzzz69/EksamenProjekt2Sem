using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace EksamenProjekt2Sem.Services
{
    public class OrderService : GenericDbService<Order>
    {
        private List<Order> _orders; // Overskud fra domain model
        private List<Food> _food;
        private List<CampaignOffer> _campaignOffers;
        private GenericDbService<Order> _dbService; // Overskud fra domain model
        private GenericDbService<Food> _dbFoodService;
        private GenericDbService<CampaignOffer> _dbCampaignOfferService;

        public OrderService(GenericDbService<Order> dbService, GenericDbService<Food> dbFoodService, GenericDbService<CampaignOffer> dbCampaignOfferService)
        {
            _dbService = dbService;
            _dbFoodService = dbFoodService;
            _dbCampaignOfferService = dbCampaignOfferService;
            try
            {
                _orders = _dbService.GetObjectsAsync().Result.ToList();
                if (_orders == null || _orders.Count() < 1)
                {
                    //SeedOrderAsync().Wait();
                    _orders = _dbService.GetObjectsAsync().Result.ToList();
                }
                _food = _dbFoodService.GetObjectsAsync().Result.ToList();
                _campaignOffers = _dbCampaignOfferService.GetObjectsAsync().Result.ToList();
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

            //if (_orders == null)
            //{
            //    _orders = MockOrder.GetOrders();
            //}
            //else
            //    _orders = _dbService.GetObjectsAsync().Result.ToList();
        }
       
        //public async Task SeedOrderAsync()
        //{
        //    _orders = new List<Order>();
        //    var order = MockOrder.GetOrders();
        //    await _dbService.SaveObjects(order);
        //}

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
            if (user == null) return null;

            List<Order> orders = _orders.FindAll(o => o.UserId == user.Id);

            foreach (Order o in orders)
            {
                if (o.FoodId != null)
                {
                    o.Food = _food.FirstOrDefault(f => f.Id == o.FoodId);
                }
                if (o.CampaignOfferId != null)
                {
                    o.CampaignOffer = _campaignOffers.FirstOrDefault(c => c.Id == o.CampaignOfferId);
                }
            }
            return orders;
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
