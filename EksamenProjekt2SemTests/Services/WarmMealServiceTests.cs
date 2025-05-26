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
//using System.IO;

//namespace EksamenProjekt2Sem.Services.Tests
//{
//    [TestClass]
//    public class WarmMealServiceTests
//    {
//        private DbContextOptions<FoodContext> _options;
//        private GenericDbService<WarmMeal> _dbService;

//        public WarmMealServiceTests()
//        {
//            _options = new DbContextOptions<FoodContext>();
//            _dbService = new GenericDbService<WarmMeal>(_options);
//        }
//        [TestInitialize]
//        public void Setup()
//        {
//            _options = new DbContextOptionsBuilder<FoodContext>()
//                .UseInMemoryDatabase("TestDb_ReadWarmMeal")
//                .Options;

//            using (var context = new FoodContext(_options))
//            {
//                context.Database.EnsureDeleted(); // ren database for hver testkørsel

//                // Tilføjer en række WarmMeals til den midlertidige databasen
//                context.WarmMeals.AddRange(
//                    new WarmMeal
//                    {
//                        //Id = 1,
//                        Ingredients = "Pasta Bolognase",
//                        MeatType = "Beef",
//                        Price = 19.95,
//                        MinPersonAmount = 1,
//                    },
//                    new WarmMeal
//                    {
//                        //Id = 2,
//                        Ingredients = "Pizza",
//                        MeatType = null,
//                        Price = 60.00,
//                        MinPersonAmount = 2,
//                    });

//                context.SaveChanges();
//            }
//            _dbService = new GenericDbService<WarmMeal>(_options);
//        }

//        [TestMethod]
//        public async Task CreateWarmMealTest_AddsWarmMealToDatabase()
//        {
//            // Arrange

//            var item = new WarmMeal {Ingredients = "Burger", MeatType = "Beef", Price = 40.00, MinPersonAmount = 1, };

//            // Act
//            using (var context = new FoodContext(_options))
//            {
//                var database = new GenericDbService<WarmMeal>(_options);
//                var service = new WarmMealService(database);
//                await context.Database.EnsureDeletedAsync();
//                await service.CreateWarmMeal(item);
//            }

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var result = await context.WarmMeals.FindAsync(item.Id);

//                Assert.IsNotNull(result, "WarmMeal blev ikke fundet i databasen.");
//                Assert.AreEqual("Burger", result.Ingredients, "WarmMeal med forkerte ingredienser fundet");
//                Assert.AreEqual(40.00, result.Price, "WarmMeal med forkert pris fundet");
//            }
//        }
//        [TestMethod]
//        public void ReadWarmMeal_ValidId_ReturnsCorrectWarmMeal()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // Act
//            var result = service.ReadWarmMeal(1);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual("Pasta Bolognase", result.Ingredients);
//            Assert.AreEqual("Beef", result.MeatType);
//            Assert.AreEqual(19.95, result.Price);
//        }
//        [TestMethod]
//        public void ReadWarmMeal_InvalidId_ThrowsException()
//        {
//            // Arrange
//            var dbService = new GenericDbService<WarmMeal>(_options);
//            var service = new WarmMealService(dbService);

//            // Act & Assert
//            var ex = Assert.ThrowsException<Exception>(() => service.ReadWarmMeal(999));
//            Assert.AreEqual("WarmMeal with id 999 not found.", ex.Message);
//        }
//        [TestMethod]
//        public void ReadAllWarmMeals_ReturnsAllWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // Act
//            var result = service.ReadAllWarmMeals();

//            // Assert
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//            Assert.AreEqual("Pizza", result[1].Ingredients);
//        }
//        [TestMethod]
//        public void UpdateWarmMeal_ValidId_UpdatesWarmMeal()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            var WarmMealToUpdate = new WarmMeal { Id = 1, Ingredients = "Updated Pasta Bolognase", MeatType = "Updated Beef", Price = 20.00, MinPersonAmount = 3 };

//            // Act
//            service.UpdateWarmMeal(WarmMealToUpdate);

//            // Assert
//            var updatedWarmMeal = service.ReadWarmMeal(1);
//            Assert.AreEqual("Updated Pasta Bolognase", updatedWarmMeal.Ingredients);
//            Assert.AreEqual("Updated Beef", updatedWarmMeal.MeatType);
//            Assert.AreEqual(20.00, updatedWarmMeal.Price);
//            Assert.AreEqual(3, updatedWarmMeal.MinPersonAmount);
//        }
//        [TestMethod]
//        public void UpdateWarmMeal_InvalidId_DoesNotUpdate()
//        {
//            // Arrange
//            var WarmMealToUpdate = new WarmMeal { Id = 999, Ingredients = "Non-existent Meal", MeatType = "Non-existent Meat", Price = 0.00, MinPersonAmount = 0 };

//            // Act
//            using (var context = new FoodContext(_options))
//            {
//                var database = new GenericDbService<WarmMeal>(_options);
//                var service = new WarmMealService(database);
//                service.UpdateWarmMeal(WarmMealToUpdate);

//            // Assert
//                var result = context.WarmMeals.Find(999); // Try to find the non-existent warm meal
//                Assert.IsNull(result, "WarmMeal should not have been updated in the database.");
//            }
//        }
///*
//        [TestMethod]
//        public void UpdateWarmMeal_UpdatesWarmMealInDatabase()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // New warmmeal object with updated details
//            var updatedWarmMeal = new WarmMeal
//            {
//                Id = 1,
//                Ingredients = "Turkey & Cheese",
//                MeatType = "Turkey",
//                Price = 21.95,
//                MinPersonAmount = 3,
//                InSeason = "false"
//            };

//            // Act: Call UpdateWarmMeal to update the warmmeal
//            service.UpdateWarmMeal(updatedWarmMeal);

//            // Assert: Check if the warmmeal is updated in the database
//            using (var context = new FoodContext(_options))
//            {
//                var result = context.WarmMeals.Find(1); // Find the warmmeal by ID
//                Assert.IsNotNull(result); // Ensure the warmmeal exists in the database
//                Assert.AreEqual("Turkey & Cheese", result.Ingredients); // Check if the Ingredients were updated
//                Assert.AreEqual("Turkey", result.MeatType); // Check if the MeatType was updated
//                Assert.AreEqual(21.95, result.Price); // Check if the Price was updated
//                Assert.AreEqual(3, result.MinPersonAmount); // Check if the MinPersonAmount was updated
//                Assert.AreEqual("false", result.InSeason); // Check if the InSeason was updated
//            }
//        }
//*/
//        [TestMethod]
//        public void DeleteWarmMeal_ValidId_DeletesWarmMeal()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // Act
//            var deletedWarmMeal = service.DeleteWarmMeal(1);

//            // Assert
//            Assert.IsNotNull(deletedWarmMeal);
//            Assert.AreEqual("Pasta Bolognase", deletedWarmMeal.Ingredients);
//            Assert.AreEqual(1, _dbService.GetObjectsAsync().Result.Count());
//        }
//        [TestMethod]
//        public void DeleteWarmMeal_InvalidId_ThrowsException()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // Act & Assert
//            var ex = Assert.ThrowsException<Exception>(() => service.DeleteWarmMeal(999));
//            Assert.AreEqual("WarmMeal with id 999 not found.", ex.Message);
//        }
///*
//        [TestMethod]
//        public void FilterWarmMealByMinPersonLower_ValidPrice_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            int MinPerson = 1;

//            // Act
//            var result = service.FilterWarmMealByMinPersonLower(MinPerson);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterWarmMealByMinPersonLower_InvalidMinPerson_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            int MinPerson = 100;

//            // Act
//            var result = service.FilterWarmMealByMinPersonLower(MinPerson);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterWarmMealByMinPersonUpper_ValidMinPerson_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            int MinPerson = 1;

//            // Act
//            var result = service.FilterWarmMealByPriceUpper(MinPerson);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Pizza", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterWarmMealByMinPersonUpper_InvalidMinPerson_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            int MinPerson = 0;

//            // Act
//            var result = service.FilterWarmMealByMinPersonUpper(MinPerson);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterWarmMealByMinPersonRange_ValidRange_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            int lower = 1;
//            int upper = 2;

//            // Act
//            var result = service.FilterWarmMealByMinPersonRange(lower, upper);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//        }
//        [TestMethod]
//        public void FilterWarmMealByMinPersonRange_InvalidRange_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            int lower = 3;
//            int upper = 0;

//            // Act
//            var result = service.FilterWarmMealByMinPersonRange(lower, upper);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//*/
//        [TestMethod]
//        public void SearchWarmMealByMeatType_ValidCategory_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            string meatType = "Beef";

//            // Act
//            var result = service.SearchWarmMealByMeatType(meatType);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count); // Should return 2 because of the null value in the base data
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void SearchWarmMealByMeatType_InvalidCategory_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            string meatType = "NonExistentMeatType";

//            // Act
//            var result = service.SearchWarmMealByMeatType(meatType);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count); // Should return 1 because of the null value in the base data
//        }
///*
//        [TestMethod]
//        public void FilterWarmMealByIngredient_ValidIngredient_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            string ingredient = "Pasta";

//            // Act
//            var result = service.FilterWarmMealByIngredient(ingredient);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterWarmMealByIngredient_InvalidIngredient_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            string ingredient = "NonExistentIngredient";

//            // Act
//            var result = service.FilterWarmMealByIngredient(ingredient);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterWarmMealByPriceLower_ValidPrice_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            double price = 20.00;

//            // Act
//            var result = service.FilterWarmMealByPriceLower(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Pizza", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterWarmMealByPriceLower_InvalidPrice_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            double price = 100.00;

//            // Act
//            var result = service.FilterWarmMealByPriceLower(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterWarmMealByPriceUpper_ValidPrice_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            double price = 20.00;

//            // Act
//            var result = service.FilterWarmMealByPriceUpper(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterWarmMealByPriceUpper_InvalidPrice_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            double price = 10.00;

//            // Act
//            var result = service.FilterWarmMealByPriceUpper(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterWarmMealByPriceRange_ValidRange_ReturnsMatchingWarmMeals()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            double lower = 15.00;
//            double upper = 25.00;

//            // Act
//            var result = service.FilterWarmMealByPriceRange(lower, upper);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterWarmMealByPriceRange_InvalidRange_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);
//            double lower = 30.00;
//            double upper = 40.00;

//            // Act
//            var result = service.FilterWarmMealByPriceRange(lower, upper);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void GetWarmMealsSortedById_ReturnsSortedList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // Act
//            var result = service.GetWarmMealsSortedById();

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Pasta Bolognase", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void GetWarmMealsSortedByPrice_ReturnsSortedList()
//        {
//            // Arrange
//            var service = new WarmMealService(_dbService);

//            // Act
//            var result = service.GetWarmMealsSortedByPrice();

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Pizza", result[1].Ingredients);
//        }
//*/
//    }
//}