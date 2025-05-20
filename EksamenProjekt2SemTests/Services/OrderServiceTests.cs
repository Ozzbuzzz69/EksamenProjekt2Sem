//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using EksamenProjekt2Sem.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EksamenProjekt2Sem.Models;
//using EksamenProjektTest.EFDbContext;
//using Microsoft.EntityFrameworkCore;

//namespace EksamenProjekt2Sem.Services.Tests
//{
//    [TestClass]
//    public class OrderServiceTests
//    {
//        private DbContextOptions<FoodContext> _options;
//        private GenericDbService<Order> _dbService;
//        [TestInitialize]
//        public void Setup()
//        {
//            _options = new DbContextOptionsBuilder<FoodContext>()
//                .UseInMemoryDatabase("TestDb_ReadOrder")
//                .Options;

//            using (var context = new FoodContext(_options))
//            {
//                context.Database.EnsureDeleted(); // ren database for hver testkørsel
//                context.Database.EnsureCreated();

//                // Tilføjer en række Orders til den midlertidige databasen
//                context.Orders.AddRange(
//                    new Order
//                    {
//                        Id = 1,
//                        User = new User { Name = "TestUser1" },
//                        PickupTime = DateTime.Now.AddDays(2),
//                        OrderLines = new List<OrderLine>
//                        {
//                            new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                            new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                        }
//                    },
//                    new Order
//                    {
//                        Id = 2,
//                        User = new User { Name = "TestUser2" },
//                        PickupTime = DateTime.Now.AddDays(3),
//                        OrderLines = new List<OrderLine>
//                        {
//                            new OrderLine { Food = new Sandwich { Ingredients = "Chicken & Bacon", MeatType = "Chicken", Price = 60 }, Quantity = 1 },
//                            new OrderLine { Food = new Sandwich { Ingredients = "Veggie Delight", Price = 70 }, Quantity = 2 }
//                        }
//                    });
//                context.SaveChanges();
//            }
//            _dbService = new GenericDbService<Order>(_options);
//        }


//        [TestMethod]
//        public void CreateOrderTest_AddsOrderToDatabase()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var newOrder = new Order
//            {
//                Id = 3,
//                User = new User { Name = "TestUser3" },
//                PickupTime = DateTime.Now.AddDays(4),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Turkey & Cheese", MeatType = "Turkey", Price = 55 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.CreateOrder(newOrder);

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var orderInDb = context.Orders.FirstOrDefault(o => o.User.Name == "TestUser3");
//                Assert.IsNotNull(orderInDb);
//                Assert.AreEqual("TestUser3", orderInDb.User.Name);
//                Assert.AreEqual(1, orderInDb.OrderLines.Count);
//                Assert.AreEqual("Turkey & Cheese", orderInDb.OrderLines[0].Food.Ingredients);
//            }
//        }
//        [TestMethod]
//        public void ReadOrderTest_ValidId_ReturnsCorrectOrder()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            int orderId = 1;

//            // Act
//            var order = orderService.ReadOrder(orderId);

//            // Assert
//            Assert.IsNotNull(order);
//            Assert.AreEqual(orderId, order.Id);
//            Assert.AreEqual("TestUser1", order.User.Name);
//        }
//        [TestMethod]
//        public void ReadOrderTest_InvalidId_ReturnsNull()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            int invalidOrderId = 999;

//            // Act
//            var order = orderService.ReadOrder(invalidOrderId);

//            // Assert
//            Assert.IsNull(order);
//        }
//        [TestMethod]
//        public void ReadAllOrdersTest_ReturnsAllOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);

//            // Act
//            var orders = orderService.ReadAllOrders();

//            // Assert
//            Assert.IsNotNull(orders);
//            Assert.AreEqual(2, orders.Count());
//        }
//        [TestMethod]
//        public void ReadAllOrdersByUser_ValidUser_ReturnsOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var user = new User { Name = "TestUser3" };
//            var newOrder = new Order
//            {
//                Id = 3,
//                User = user,
//                PickupTime = DateTime.Now.AddDays(4),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Turkey & Cheese", MeatType = "Turkey", Price = 55 }, Quantity = 1 }
//                }
//            };
//            orderService.CreateOrder(newOrder); // Assuming this method is implemented correctly

//            // Act
//            var orders = orderService.ReadAllOrdersByUser(user);

//            // Assert
//            Assert.IsNotNull(orders);
//            Assert.AreEqual(1, orders.Count());
//            Assert.AreEqual("TestUser3", orders[0].User.Name);
//        }
//        [TestMethod]
//        public void ReadAllOrdersByUser_InvalidUser_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var user = new User { Name = "NonExistentUser" };

//            // Act
//            var orders = orderService.ReadAllOrdersByUser(user);

//            // Assert
//            Assert.IsNotNull(orders);
//            Assert.AreEqual(0, orders.Count());
//        }
//        [TestMethod]
//        public void UpdateOrderTest_ValidOrder_UpdatesOrderInDatabase()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var orderToUpdate = new Order
//            {
//                Id = 1,
//                User = new User { Name = "UpdatedUser" },
//                PickupTime = DateTime.Now.AddDays(5),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Updated Sandwich", MeatType = "Turkey", Price = 55 }, Quantity = 2 }
//                }
//            };

//            // Act
//            orderService.UpdateOrder(orderToUpdate.Id, orderToUpdate);

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var updatedOrder = context.Orders.FirstOrDefault(o => o.Id == 1);
//                Assert.IsNotNull(updatedOrder);
//                Assert.AreEqual("UpdatedUser", updatedOrder.User.Name);
//                Assert.AreEqual(2, updatedOrder.OrderLines.Count);
//            }
//        }
//        [TestMethod]
//        public void UpdateOrderTest_InvalidOrderId_ThrowsException()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var invalidOrderId = 999;
//            var orderToUpdate = new Order
//            {
//                Id = invalidOrderId,
//                User = new User { Name = "UpdatedUser" },
//                PickupTime = DateTime.Now.AddDays(5),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Updated Sandwich", MeatType = "Turkey", Price = 55 }, Quantity = 2 }
//                }
//            };

//            // Act & Assert
//            Assert.ThrowsException<ArgumentNullException>(() => orderService.UpdateOrder(invalidOrderId, orderToUpdate));
//        }
//        [TestMethod]
//        public void DeleteOrderTest_ValidOrderId_DeletesOrderFromDatabase()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            int orderIdToDelete = 1;

//            // Act
//            orderService.DeleteOrder(orderIdToDelete);

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var deletedOrder = context.Orders.FirstOrDefault(o => o.Id == orderIdToDelete);
//                Assert.IsNull(deletedOrder);
//            }
//        }
//        [TestMethod]
//        public void DeleteOrderTest_InvalidOrderId_ThrowsException()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            int invalidOrderId = 999;

//            // Act & Assert
//            Assert.ThrowsException<ArgumentNullException>(() => orderService.DeleteOrder(invalidOrderId));
//        }
//        [TestMethod]
//        public void CalculateTotalPriceTest_ValidOrder_ReturnsCorrectTotalPrice()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);

//            // Act
//            double totalPrice = orderService.CalculateTotalPrice(1);

//            // Assert
//            Assert.AreEqual(175, totalPrice);
//        }
//        [TestMethod]
//        public void CalculateTotalPriceTest_InvalidId_ThrowsException()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            int invalidOrderId = 999;

//            // Act & Assert
//            Assert.ThrowsException<Exception>(() => orderService.CalculateTotalPrice(invalidOrderId));
//        }
//        [TestMethod]
//        public void CalculateTotalPriceTest_EmptyOrder_ReturnsZero()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var emptyOrder = new Order
//            {
//                Id = 4,
//                User = new User { Name = "EmptyOrderUser" },
//                PickupTime = DateTime.Now.AddDays(4),
//                OrderLines = new List<OrderLine>()
//            };
//            orderService.CreateOrder(emptyOrder); // Assuming this method is implemented correctly

//            // Act
//            double totalPrice = orderService.CalculateTotalPrice(emptyOrder.Id);

//            // Assert
//            Assert.AreEqual(0, totalPrice);
//        }
//        [TestMethod]
//        public void ReadOrderByUserId_ValidUserId_ReturnsOrder()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var userId = 1; // Assuming this user exists in the test data

//            // Act
//            var order = orderService.ReadOrderByUserId(userId);

//            // Assert
//            Assert.IsNotNull(order);
//            Assert.AreEqual("TestUser1", order.User.Name);
//        }
//        [TestMethod]
//        public void ReadOrderByUserId_InvalidUserId_ReturnsNull()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var invalidUserId = 999;

//            // Act
//            var order = orderService.ReadOrderByUserId(invalidUserId);

//            // Assert
//            Assert.IsNull(order);
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTime_ValidDate_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(2);

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTime(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(1, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders.First().User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTime_InvalidDate_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist for this date

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTime(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTimeLower_ValidDate_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(1);

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTimeLower(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTimeLower_InvalidDate_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist after this date

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTimeLower(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTimeUpper_ValidDate_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(3);

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTimeUpper(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTimeUpper_InvalidDate_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(1); // Assuming no orders exist before this date

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTimeUpper(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTimeRange_ValidDates_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime startDate = DateTime.Now.AddDays(1);
//            DateTime endDate = DateTime.Now.AddDays(2);

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTimeRange(startDate, endDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(1, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersPickupTimeRange_InvalidDates_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime startDate = DateTime.Now.AddDays(10);
//            DateTime endDate = DateTime.Now.AddDays(20); // Assuming no orders exist in this range

//            // Act
//            var filteredOrders = orderService.FilterOrdersPickupTimeRange(startDate, endDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTime_ValidDate_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(2);

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTime(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(1, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTime_InvalidDate_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist for this date

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTime(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTimeLower_ValidDate_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(1);

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTimeLower(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTimeLower_InvalidDate_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(10); // Assuming no orders exist after this date

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTimeLower(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTimeUpper_ValidDate_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(3);

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTimeUpper(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTimeUpper_InvalidDate_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime filterDate = DateTime.Now.AddDays(1); // Assuming no orders exist before this date

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTimeUpper(filterDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTimeRange_ValidDates_ReturnsFilteredOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime startDate = DateTime.Now.AddDays(1);
//            DateTime endDate = DateTime.Now.AddDays(2);

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTimeRange(startDate, endDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(1, filteredOrders.Count());
//            Assert.AreEqual("TestUser1", filteredOrders[0].User.Name);
//        }
//        [TestMethod]
//        public void FilterOrdersOrderTimeRange_InvalidDates_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            DateTime startDate = DateTime.Now.AddDays(10);
//            DateTime endDate = DateTime.Now.AddDays(20); // Assuming no orders exist in this range

//            // Act
//            var filteredOrders = orderService.FilterOrdersOrderTimeRange(startDate, endDate);

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void GetOrdersSortedById_ReturnsSortedOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);

//            // Act
//            var filteredOrders = orderService.GetOrdersSortedById();

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser2", filteredOrders[1].User.Name);
//        }
//        [TestMethod]
//        public void GetOrdersSortedById_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var emptyOrderList = new List<Order>();

//            // Act
//            var filteredOrders = orderService.GetOrdersSortedById();

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void GetOrdersSortedByPickupTime_ReturnsSortedOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);

//            // Act
//            var filteredOrders = orderService.GetOrdersSortedByPickupTime();

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser2", filteredOrders[1].User.Name);
//        }
//        [TestMethod]
//        public void GetOrdersSortedByPickupTime_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var emptyOrderList = new List<Order>();

//            // Act
//            var filteredOrders = orderService.GetOrdersSortedByPickupTime();

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void GetOrdersSortedByOrderTime_ReturnsSortedOrders()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);

//            // Act
//            var filteredOrders = orderService.GetOrdersSortedByOrderTime();

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(2, filteredOrders.Count());
//            Assert.AreEqual("TestUser2", filteredOrders[1].User.Name);
//        }
//        [TestMethod]
//        public void GetOrdersSortedByOrderTime_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var emptyOrderList = new List<Order>();

//            // Act
//            var filteredOrders = orderService.GetOrdersSortedByOrderTime();

//            // Assert
//            Assert.IsNotNull(filteredOrders);
//            Assert.AreEqual(0, filteredOrders.Count());
//        }
//        [TestMethod]
//        public void SortOrderlinesByPrice_ReturnsSortedOrderlines() 
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.SortOrderlinesByPrice(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(2, order.OrderLines.Count);
//            Assert.AreEqual("Vegan", order.OrderLines[0].Food.Ingredients);
//            Assert.AreEqual("Ham & Cheese", order.OrderLines[1].Food.Ingredients);
//        }
//        [TestMethod]
//        public void SortOrderlinesByPrice_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>()
//            };

//            // Act
//            orderService.SortOrderlinesByPrice(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(0, order.OrderLines.Count);
//        }
//        [TestMethod]
//        public void SortOrderlinesByPriceDescending_ReturnsSortedOrderlines()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.SortOrderlinesByPriceDescending(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(2, order.OrderLines.Count);
//            Assert.AreEqual("Ham & Cheese", order.OrderLines[0].Food.Ingredients);
//            Assert.AreEqual("Vegan", order.OrderLines[1].Food.Ingredients);
//        }
//        [TestMethod]
//        public void SortOrderlinesByPriceDescending_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>()
//            };

//            // Act
//            orderService.SortOrderlinesByPriceDescending(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(0, order.OrderLines.Count);
//        }
//        [TestMethod]
//        public void SortOrderlinesByQuantity_ReturnsSortedOrderlines()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.SortOrderlinesByQuantity(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(2, order.OrderLines.Count);
//            Assert.AreEqual("Vegan", order.OrderLines[0].Food.Ingredients);
//            Assert.AreEqual("Ham & Cheese", order.OrderLines[1].Food.Ingredients);
//        }
//        [TestMethod]
//        public void SortOrderlinesByQuantity_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>()
//            };

//            // Act
//            orderService.SortOrderlinesByQuantity(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(0, order.OrderLines.Count);
//        }
//        [TestMethod]
//        public void SortOrderlinesByQuantityDescending_ReturnsSortedOrderlines()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                    new OrderLine { Food = new Sandwich { Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.SortOrderlinesByQuantityDescending(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(2, order.OrderLines.Count);
//            Assert.AreEqual("Ham & Cheese", order.OrderLines[0].Food.Ingredients);
//            Assert.AreEqual("Vegan", order.OrderLines[1].Food.Ingredients);
//        }
//        [TestMethod]
//        public void SortOrderlinesByQuantityDescending_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>()
//            };

//            // Act
//            orderService.SortOrderlinesByQuantityDescending(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(0, order.OrderLines.Count);
//        }
//        [TestMethod]
//        public void SortOrderlinesById_ReturnsSortedOrderlines()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Id = 9, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                    new OrderLine { Food = new Sandwich { Id = 12, Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.SortOrderlinesById(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(2, order.OrderLines.Count);
//            Assert.AreEqual("Ham & Cheese", order.OrderLines[0].Food.Ingredients);
//            Assert.AreEqual("Vegan", order.OrderLines[1].Food.Ingredients);
//        }
//        [TestMethod]
//        public void SortOrderlinesById_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>()
//            };

//            // Act
//            orderService.SortOrderlinesById(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(0, order.OrderLines.Count);
//        }
//        [TestMethod]
//        public void SortOrderlinesByIdDescending_ReturnsSortedOrderlines()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>
//                {
//                    new OrderLine { Food = new Sandwich { Id = 9, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 50 }, Quantity = 2 },
//                    new OrderLine { Food = new Sandwich { Id = 12, Ingredients = "Vegan", Price = 75 }, Quantity = 1 }
//                }
//            };

//            // Act
//            orderService.SortOrderlinesByIdDescending(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(2, order.OrderLines.Count);
//            Assert.AreEqual("Vegan", order.OrderLines[0].Food.Ingredients);
//            Assert.AreEqual("Ham & Cheese", order.OrderLines[1].Food.Ingredients);
//        }
//        [TestMethod]
//        public void SortOrderlinesByIdDescending_EmptyList_ReturnsEmptyList()
//        {
//            // Arrange
//            var orderService = new OrderService(_dbService);
//            var order = new Order
//            {
//                Id = 1,
//                User = new User { Name = "TestUser1" },
//                PickupTime = DateTime.Now.AddDays(2),
//                OrderLines = new List<OrderLine>()
//            };

//            // Act
//            orderService.SortOrderlinesByIdDescending(order);

//            // Assert
//            Assert.IsNotNull(order.OrderLines);
//            Assert.AreEqual(0, order.OrderLines.Count);
//        }
//    }
//}