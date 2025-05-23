using Microsoft.VisualStudio.TestTools.UnitTesting;
using EksamenProjekt2Sem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace EksamenProjekt2Sem.Services.Tests
{
    [TestClass]
    public class OrderServiceTests
    {
        private DbContextOptions<FoodContext> _options;
        private GenericDbService<Order> _dbService;
        private IHttpContextAccessor _httpContextAccessor;

        public OrderServiceTests()
        {
            _options = new DbContextOptions<FoodContext>();
            _dbService = new GenericDbService<Order>(_options);
            _httpContextAccessor = new HttpContextAccessor();
        }

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseInMemoryDatabase("TestDb_ReadOrder")
                .Options;

            _dbService = new GenericDbService<Order>(_options);

            // Make sure _httpContextAccessor is initialized BEFORE setting HttpContext
            if (_httpContextAccessor == null)
                _httpContextAccessor = new HttpContextAccessor();

            var httpcontext = new DefaultHttpContext();
            httpcontext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Name, "TestUser1"),
        new Claim(ClaimTypes.NameIdentifier, "123")
            }, "TestAuthentication"));

            _httpContextAccessor.HttpContext = httpcontext;

            using (var context = new FoodContext(_options))
            {
                context.Database.EnsureDeleted();

                context.Orders.AddRange(
                    new Order
                    {
                        User = new User("TestUser1", "tester@example.com", "12345678", "password123"),
                        PickupTime = DateTime.Now.AddDays(2),
                        OrderLines = new List<OrderLine>
                        {
                            new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                            new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Luxury" }, Quantity = 1 }
                        }
                    },
                    new Order
                    {
                        User = new User("TestUser2", "tester@example.com", "12345678", "password123"),
                        PickupTime = DateTime.Now.AddDays(4),
                        OrderLines = new List<OrderLine>
                        {
                            new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                            new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Luxury" }, Quantity = 1 }
                        }
                    });

                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task CreateOrderTest_AddsOrderToDatabase()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);

            var newOrder = new Order
            {
                User = new User("TestUser3", "tester@example.com", "12345678", "password123"),
                PickupTime = DateTime.Now.AddDays(4),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        Food = new Sandwich
                        {
                            Ingredients = "Turkey & Cheese",
                            MeatType = "Turkey",
                            Price = 55,
                            Category = "Standard"
                        },
                        Quantity = 1
                    }
                }
            };

            // Act
            await orderService.CreateOrder(newOrder);  // Prefer await over .Wait()

            // Assert
            using (var context = new FoodContext(_options))
            {
                var orderInDb = context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderLines)
                        .ThenInclude(ol => ol.Food)
                    .FirstOrDefault(o => o.User.Name == newOrder.User.Name);

                Assert.IsNotNull(orderInDb);
                Assert.AreEqual("TestUser3", orderInDb.User.Name);
                Assert.AreEqual(1, orderInDb.OrderLines.Count);
                Assert.AreEqual("Turkey & Cheese", orderInDb.OrderLines[0].Food.Ingredients);
            }
        }

        [TestMethod]
        public void ReadOrderTest_ValidId_ReturnsCorrectOrder()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            int orderid = _dbService.GetObjectsAsync().Result.First().Id;

            // Act
            var orderToFind = orderService.ReadOrder(orderid);


            // Assert
            Assert.IsNotNull(orderToFind);
            Assert.AreEqual(orderToFind.Id, orderid);            
        }
        [TestMethod]
        public void ReadOrderTest_InvalidId_ReturnsNull()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            int invalidOrderId = 999;

            // Act
            var order = orderService.ReadOrder(invalidOrderId);

            // Assert
            Assert.IsNull(order);
        }
        /*
        [TestMethod]
        public void ReadAllOrdersTest_ReturnsAllOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);

            // Act
            var orders = orderService.ReadAllOrders();

            // Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(2, orders.Count());
        }
        */
        /*
        [TestMethod]
        public void ReadAllOrdersByUser_ValidUser_ReturnsOrders()
        {
            // Ensure clean setup
            using (var context = new FoodContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user = new User("TestUser1", "test1@example.com", "12345678", "password123");
                var order = new Order
                {
                    User = user,
                    PickupTime = DateTime.Now.AddDays(2),
                    OrderLines = new List<OrderLine>
            {
                new OrderLine
                {
                    Food = new Sandwich
                    {
                        Ingredients = "Ham & Cheese",
                        MeatType = "Ham",
                        Price = 50,
                        Category = "Standard"
                    },
                    Quantity = 2
                }
            }
                };

                context.Users.Add(user);
                context.Orders.Add(order);
                context.SaveChanges();
            }

            // Act using your service
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var result = orderService.ReadAllOrdersByUser(new User("TestUser1", "test1@example.com", "", ""));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TestUser1", result[0].User.Name);
            Assert.AreEqual("Ham & Cheese", result[0].OrderLines[0].Food.Ingredients);
        }
        */



        /*
        [TestMethod]
        public void ReadAllOrdersByUser_InvalidUser_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var user = new User { Name = "NonExistentUser" };

            // Act
            var orders = orderService.ReadAllOrdersByUser(user);

            // Assert
            Assert.IsNotNull(orders);
            Assert.AreEqual(0, orders.Count());
        }
        */
        /*
        
        Can't be tested as I don't have enough databases accessed

        [TestMethod]
        public void UpdateOrderTest_ValidOrder_UpdatesOrderInDatabase()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var orderToUpdate = new Order
            {
                Id = 1,
                User = new User("TestUser2", "tester@example.com", "12345678", "password123"),
                PickupTime = DateTime.Now.AddDays(4),
                OrderLines = new List<OrderLine>
                        {
                            new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                            new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Luxury" }, Quantity = 1 }
                        }
            };

            // Act
            orderService.UpdateOrder(orderToUpdate);

            // Assert
            using (var context = new FoodContext(_options))
            {
                var updatedOrder = _dbService.GetObjectByIdAsync(orderToUpdate.Id).Result;
                Assert.IsNotNull(updatedOrder);
                //Assert.AreEqual(orderToUpdate.User.Name, updatedOrder.User.Name);
                Assert.AreEqual(2, updatedOrder.OrderLines.Count);
            }
        }
        */
        [TestMethod]
        public async Task UpdateOrderTest_InvalidOrderId_DoesNothing()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var invalidOrderId = 999;
            var orderToUpdate = new Order
            {
                Id = invalidOrderId,
                User = new User { Name = "UpdatedUser" },
                PickupTime = DateTime.Now.AddDays(999),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Updated Sandwich", MeatType = "Turkey", Price = 55, Category = "Standard" }, Quantity = 2 }
                }
            };

            // Act
            orderService.UpdateOrder(orderToUpdate);

            // Assert
            using (var context = new FoodContext(_options))
            {

                var updatedOrder = context.Orders.FirstOrDefault(o => o.Id == invalidOrderId);
                Assert.IsNull(updatedOrder);
                /*
                // Check that the original orders are unchanged
                var orders = await orderService.GetObjectsAsync();
                var originalOrder1 = orders.First();
                var originalOrder2 = orders.First(o => o.Id != originalOrder1.Id);

                // Check for updated Id
                Assert.AreNotEqual(orderToUpdate.Id, originalOrder1.Id);
                Assert.AreNotEqual(orderToUpdate.Id, originalOrder2.Id);

                // Check for updated User
                Assert.AreNotEqual(orderToUpdate.User, originalOrder1.User);
                Assert.AreNotEqual(orderToUpdate.User, originalOrder2.User);

                // Check for updated PickupTime
                Assert.AreNotEqual(orderToUpdate.PickupTime, originalOrder1.PickupTime);
                Assert.AreNotEqual(orderToUpdate.PickupTime, originalOrder2.PickupTime);
                */
            }
        }
        [TestMethod]
        public void DeleteOrderTest_ValidOrderId_DeletesOrderFromDatabase()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            int orderIdToDelete = 1;

            // Act
            orderService.DeleteOrder(orderIdToDelete);

            // Assert
            using (var context = new FoodContext(_options))
            {
                var deletedOrder = context.Orders.FirstOrDefault(o => o.Id == orderIdToDelete);
                Assert.IsNull(deletedOrder);
            }
        }
        [TestMethod]
        public void DeleteOrderTest_InvalidOrderId_ReturnsNull()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            int invalidOrderId = 999;

            // Act
            var deletedOrder = orderService.DeleteOrder(invalidOrderId);

            // Assert
            Assert.IsNull(deletedOrder);
            Assert.AreEqual(2, _dbService.GetObjectsAsync().Result.Count());
        }
        /*
        [TestMethod]
        public void CalculateTotalPriceTest_ValidOrder_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);

            // Act
            double totalPrice = orderService.CalculateTotalPrice(1);

            // Assert
            Assert.AreEqual(175, totalPrice);
        }
        [TestMethod]
        public void CalculateTotalPriceTest_InvalidId_ThrowsException()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            int invalidOrderId = 999;

            // Act & Assert
            Assert.ThrowsException<Exception>(() => orderService.CalculateTotalPrice(invalidOrderId));
        }
        [TestMethod]
        public void CalculateTotalPriceTest_EmptyOrder_ReturnsZero()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var emptyOrder = new Order
            {
                Id = 4,
                User = new User { Name = "EmptyOrderUser" },
                PickupTime = DateTime.Now.AddDays(4),
                OrderLines = new List<OrderLine>()
            };
            orderService.CreateOrder(emptyOrder); // Assuming this method is implemented correctly

            // Act
            double totalPrice = orderService.CalculateTotalPrice(emptyOrder.Id);

            // Assert
            Assert.AreEqual(0, totalPrice);
        }
        */
        /*
        [TestMethod]
        public void ReadOrderByUserId_ValidUserId_ReturnsOrder()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var userId = 1; // Assuming this user exists in the test data

            // Act
            var order = orderService.ReadOrderByUserId(userId);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual("TestUser1", order.User.Name);
        }
        [TestMethod]
        public void ReadOrderByUserId_InvalidUserId_ReturnsNull()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var invalidUserId = 999;

            // Act
            var order = orderService.ReadOrderByUserId(invalidUserId);

            // Assert
            Assert.IsNull(order);
        }
        */
        /*
        [TestMethod]
        public void FilterOrdersPickupTime_ValidDate_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(2);

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTime(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(1, filteredOrders.Count());
            Assert.AreEqual("TestUser1", filteredOrders.First().User.Name);
        }
        [TestMethod]
        public void FilterOrdersPickupTime_InvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist for this date

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTime(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void FilterOrdersPickupTimeLower_ValidDate_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(1);

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTimeLower(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
        }
        [TestMethod]
        public void FilterOrdersPickupTimeLower_InvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist after this date

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTimeLower(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void FilterOrdersPickupTimeUpper_ValidDate_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(3);

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTimeUpper(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
        }
        [TestMethod]
        public void FilterOrdersPickupTimeUpper_InvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(1); // Assuming no orders exist before this date

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTimeUpper(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void FilterOrdersPickupTimeRange_ValidDates_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime startDate = DateTime.Now.AddDays(1);
            DateTime endDate = DateTime.Now.AddDays(2);

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTimeRange(startDate, endDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(1, filteredOrders.Count());
            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
        }
        [TestMethod]
        public void FilterOrdersPickupTimeRange_InvalidDates_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime startDate = DateTime.Now.AddDays(10);
            DateTime endDate = DateTime.Now.AddDays(20); // Assuming no orders exist in this range

            // Act
            var filteredOrders = orderService.FilterOrdersPickupTimeRange(startDate, endDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        */
        /*
        [TestMethod]
        public void FilterOrdersOrderTime_ValidDate_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = new DateTime(2025, 05, 25, 12, 0, 0); // Fixed time

            var newOrder = new Order
            {
                User = new User("TestUser3", "tester@example.com", "12345678", "password123"),
                OrderTime = filterDate,
                OrderLines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        Food = new Sandwich
                        {
                            Ingredients = "Turkey & Cheese",
                            MeatType = "Turkey",
                            Price = 55,
                            Category = "Standard"
                        },
                        Quantity = 1
                    }
                }
            };

            orderService.CreateOrder(newOrder).Wait();

            // Act
            List<Order> filteredOrders = orderService.FilterOrdersOrderTime(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(1, filteredOrders.Count());
            Assert.AreEqual("TestUser3", filteredOrders[0].User.Name);
        }
        
        [TestMethod]
        public void FilterOrdersOrderTime_InvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist for this date

            // Act
            var filteredOrders = orderService.FilterOrdersOrderTime(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        */
        /*
        [TestMethod]
        public void FilterOrdersOrderTimeLower_ValidDate_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(-5); // This includes TestUser1 and TestUser2

            // Act
            List<Order> filteredOrders = orderService.FilterOrdersOrderTimeLower(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.IsTrue(filteredOrders.Any(o => o.User.Name == "TestUser1"));
            Assert.IsTrue(filteredOrders.Any(o => o.User.Name == "TestUser2"));
        }

        [TestMethod]
        public void FilterOrdersOrderTimeLower_InvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist after this date

            // Act
            var filteredOrders = orderService.FilterOrdersOrderTimeLower(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void FilterOrdersOrderTimeUpper_ValidDate_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(3);

            // Act
            var filteredOrders = orderService.FilterOrdersOrderTimeUpper(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
        }
        [TestMethod]
        public void FilterOrdersOrderTimeUpper_InvalidDate_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime filterDate = DateTime.Now.AddDays(1); // Assuming no orders exist before this date

            // Act
            var filteredOrders = orderService.FilterOrdersOrderTimeUpper(filterDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void FilterOrdersOrderTimeRange_ValidDates_ReturnsFilteredOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime startDate = DateTime.Now.AddDays(1);
            DateTime endDate = DateTime.Now.AddDays(2);

            // Act
            var filteredOrders = orderService.FilterOrdersOrderTimeRange(startDate, endDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(1, filteredOrders.Count());
            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
        }
        [TestMethod]
        public void FilterOrdersOrderTimeRange_InvalidDates_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            DateTime startDate = DateTime.Now.AddDays(10);
            DateTime endDate = DateTime.Now.AddDays(20); // Assuming no orders exist in this range

            // Act
            var filteredOrders = orderService.FilterOrdersOrderTimeRange(startDate, endDate);

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void GetOrdersSortedById_ReturnsSortedOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);

            // Act
            var filteredOrders = orderService.GetOrdersSortedById();

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.AreEqual("TestUser2", filteredOrders[1].User.Name);
        }
        [TestMethod]
        public void GetOrdersSortedById_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var emptyOrderList = new List<Order>();

            // Act
            var filteredOrders = orderService.GetOrdersSortedById();

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void GetOrdersSortedByPickupTime_ReturnsSortedOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);

            // Act
            var filteredOrders = orderService.GetOrdersSortedByPickupTime();

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.AreEqual("TestUser2", filteredOrders[1].User.Name);
        }
        [TestMethod]
        public void GetOrdersSortedByPickupTime_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var emptyOrderList = new List<Order>();

            // Act
            var filteredOrders = orderService.GetOrdersSortedByPickupTime();

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void GetOrdersSortedByOrderTime_ReturnsSortedOrders()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);

            // Act
            var filteredOrders = orderService.GetOrdersSortedByOrderTime();

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(2, filteredOrders.Count());
            Assert.AreEqual("TestUser2", filteredOrders[1].User.Name);
        }
        [TestMethod]
        public void GetOrdersSortedByOrderTime_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var emptyOrderList = new List<Order>();

            // Act
            var filteredOrders = orderService.GetOrdersSortedByOrderTime();

            // Assert
            Assert.IsNotNull(filteredOrders);
            Assert.AreEqual(0, filteredOrders.Count());
        }
        [TestMethod]
        public void SortOrderlinesByPrice_ReturnsSortedOrderlines()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Standard" }, Quantity = 1 }
                }
            };

            // Act
            orderService.SortOrderlinesByPrice(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(2, order.OrderLines.Count);
            Assert.AreEqual("Vegan", order.OrderLines[0].Food.Ingredients);
            Assert.AreEqual("Ham & Cheese", order.OrderLines[1].Food.Ingredients);
        }
        [TestMethod]
        public void SortOrderlinesByPrice_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>()
            };

            // Act
            orderService.SortOrderlinesByPrice(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(0, order.OrderLines.Count);
        }
        [TestMethod]
        public void SortOrderlinesByPriceDescending_ReturnsSortedOrderlines()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Standard" }, Quantity = 1 }
                }
            };

            // Act
            orderService.SortOrderlinesByPriceDescending(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(2, order.OrderLines.Count);
            Assert.AreEqual("Ham & Cheese", order.OrderLines[0].Food.Ingredients);
            Assert.AreEqual("Vegan", order.OrderLines[1].Food.Ingredients);
        }
        [TestMethod]
        public void SortOrderlinesByPriceDescending_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>()
            };

            // Act
            orderService.SortOrderlinesByPriceDescending(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(0, order.OrderLines.Count);
        }
        [TestMethod]
        public void SortOrderlinesByQuantity_ReturnsSortedOrderlines()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Standard" }, Quantity = 1 }
                }
            };

            // Act
            orderService.SortOrderlinesByQuantity(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(2, order.OrderLines.Count);
            Assert.AreEqual("Vegan", order.OrderLines[0].Food.Ingredients);
            Assert.AreEqual("Ham & Cheese", order.OrderLines[1].Food.Ingredients);
        }
        [TestMethod]
        public void SortOrderlinesByQuantity_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>()
            };

            // Act
            orderService.SortOrderlinesByQuantity(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(0, order.OrderLines.Count);
        }
        [TestMethod]
        public void SortOrderlinesByQuantityDescending_ReturnsSortedOrderlines()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50, Category = "Standard" }, Quantity = 2 },
                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75, Category = "Standard" }, Quantity = 1 }
                }
            };

            // Act
            orderService.SortOrderlinesByQuantityDescending(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(2, order.OrderLines.Count);
            Assert.AreEqual("Ham & Cheese", order.OrderLines[0].Food.Ingredients);
            Assert.AreEqual("Vegan", order.OrderLines[1].Food.Ingredients);
        }
        [TestMethod]
        public void SortOrderlinesByQuantityDescending_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>()
            };

            // Act
            orderService.SortOrderlinesByQuantityDescending(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(0, order.OrderLines.Count);
        }
        */
        /*
        [TestMethod]
        public void SortOrderlinesById_ReturnsSortedOrderlines()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
                }
            };

            // Act
            orderService.SortOrderlinesById(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(2, order.OrderLines.Count);
            Assert.AreEqual("Ham & Cheese", order.OrderLines[0].Food.Ingredients);
            Assert.AreEqual("Vegan", order.OrderLines[1].Food.Ingredients);
        }
        [TestMethod]
        public void SortOrderlinesById_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>()
            };

            // Act
            orderService.SortOrderlinesById(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(0, order.OrderLines.Count);
        }
        [TestMethod]
        public void SortOrderlinesByIdDescending_ReturnsSortedOrderlines()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>
                {
                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
                }
            };

            // Act
            orderService.SortOrderlinesByIdDescending(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(2, order.OrderLines.Count);
            Assert.AreEqual("Vegan", order.OrderLines[0].Food.Ingredients);
            Assert.AreEqual("Ham & Cheese", order.OrderLines[1].Food.Ingredients);
        }
        [TestMethod]
        public void SortOrderlinesByIdDescending_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var orderService = new OrderService(_dbService, _httpContextAccessor);
            var order = new Order
            {
                //Id = 1,
                User = new User { Name = "TestUser1" },
                PickupTime = DateTime.Now.AddDays(2),
                OrderLines = new List<OrderLine>()
            };

            // Act
            orderService.SortOrderlinesByIdDescending(order);

            // Assert
            Assert.IsNotNull(order.OrderLines);
            Assert.AreEqual(0, order.OrderLines.Count);
        }
*/
    }
}