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
//    public class SandwichServiceTests
//    {
//        private DbContextOptions<FoodContext> _options;
//        private GenericDbService<Sandwich> _dbService;

//        public SandwichServiceTests()
//        {
//            _options = new DbContextOptions<FoodContext>();
//            _dbService = new GenericDbService<Sandwich>(_options);
//        }

//        [TestInitialize]
//        public void Setup()
//        {
//            _options = new DbContextOptionsBuilder<FoodContext>()
//                .UseInMemoryDatabase("TestDb_ReadSandwich")
//                .Options;

//            using (var context = new FoodContext(_options))
//            {
//                context.Database.EnsureDeleted(); // ren database for hver testkørsel

//                // Tilføjer en række sandwiches til den midlertidige databasen
//                context.Sandwiches.AddRange(
//                    new Sandwich
//                    {
//                        //Id = 1,
//                        Ingredients = "Ham & Cheese",
//                        MeatType = "Ham",
//                        Price = 19.95,
//                        Category = "Standard"
//                    },
//                    new Sandwich
//                    {
//                        //Id = 2,
//                        Ingredients = "Turkey & Salad",
//                        MeatType = "Turkey",
//                        Price = 22.50,
//                        Category = "Premium"
//                    });

//                context.SaveChanges();
//            }
//            _dbService = new GenericDbService<Sandwich>(_options);
//        }

//        [TestMethod]
//        public async Task CreateSandwichTest_AddsSandwichToDatabase()
//        {
//            // Arrange

//            var item = new Sandwich {Ingredients = "Turkey & Cheese", MeatType = "Turkey", Price = 19.95, Category = "Standard" };

//            // Act
//            using (var context = new FoodContext(_options))
//            {
//                var database = new GenericDbService<Sandwich>(_options);
//                var service = new SandwichService(database);
//                await context.Database.EnsureDeletedAsync();
//                await service.CreateSandwich(item);
//            }

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var result = await context.Sandwiches.FindAsync(item.Id);

//                Assert.IsNotNull(result, "Sandwich blev ikke fundet i databasen.");
//                Assert.AreEqual("Turkey & Cheese", result.Ingredients, "Sandwich med forkerte ingredienser fundet");
//                Assert.AreEqual(19.95, result.Price, "Sandwich med forkert pris fundet");
//            }
//        }
//        [TestMethod]
//        public void ReadSandwich_ValidId_ReturnsCorrectSandwich()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // Act
//            var result = service.ReadSandwich(1);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual("Ham & Cheese", result.Ingredients);
//            Assert.AreEqual("Ham", result.MeatType);
//            Assert.AreEqual(19.95, result.Price);
//        }

//        [TestMethod]
//        public void ReadSandwich_InvalidId_ThrowsException()
//        {
//            // Arrange
//            var dbService = new GenericDbService<Sandwich>(_options);
//            var service = new SandwichService(dbService);

//            // Act & Assert
//            var ex = Assert.ThrowsException<Exception>(() => service.ReadSandwich(999));
//            Assert.AreEqual("Sandwich with id 999 not found.", ex.Message);
//        }

//        [TestMethod]
//        public void ReadAllSandwiches_ReturnsAllSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // Act
//            var result = service.ReadAllSandwiches();

//            // Assert
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Ham & Cheese", result[0].Ingredients);
//            Assert.AreEqual("Turkey & Salad", result[1].Ingredients);
//        }

//        [TestMethod]
//        public void UpdateSandwich_ValidId_UpdatesSandwich()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            var sandwichToUpdate = new Sandwich { Id = 1, Ingredients = "Updated Ham & Cheese", MeatType = "Updated Ham", Price = 20.00, Category = "Updated" };

//            // Act
//            service.UpdateSandwich(sandwichToUpdate);

//            // Assert
//            var updatedSandwich = service.ReadSandwich(1);
//            Assert.AreEqual("Updated Ham & Cheese", updatedSandwich.Ingredients);
//            Assert.AreEqual("Updated Ham", updatedSandwich.MeatType);
//            Assert.AreEqual(20.00, updatedSandwich.Price);
//        }

//        [TestMethod]
//        public void UpdateSandwich_UpdatesSandwichInDatabase()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // New sandwich object with updated details
//            var updatedSandwich = new Sandwich
//            {
//                Id = 1,
//                Ingredients = "Turkey & Cheese",
//                MeatType = "Turkey",
//                Price = 21.95,
//                Category = "Premium",
//                InSeason = "false"
//            };

//            // Act: Call UpdateSandwich to update the sandwich
//            service.UpdateSandwich(updatedSandwich);

//            // Assert: Check if the sandwich is updated in the database
//            using (var context = new FoodContext(_options))
//            {
//                var result = context.Sandwiches.Find(1); // Find the sandwich by ID
//                Assert.IsNotNull(result); // Ensure the sandwich exists in the database
//                Assert.AreEqual("Turkey & Cheese", result.Ingredients); // Check if the Ingredients were updated
//                Assert.AreEqual("Turkey", result.MeatType); // Check if the MeatType was updated
//                Assert.AreEqual(21.95, result.Price); // Check if the Price was updated
//                Assert.AreEqual("Premium", result.Category); // Check if the Category was updated
//                Assert.AreEqual("false", result.InSeason); // Check if the InSeason was updated
//            }
//        }
//        [TestMethod]
//        public void DeleteSandwich_ValidId_DeletesSandwich()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // Act
//            var deletedSandwich = service.DeleteSandwich(1);

//            // Assert
//            Assert.IsNotNull(deletedSandwich);
//            Assert.AreEqual("Ham & Cheese", deletedSandwich.Ingredients);
//            Assert.AreEqual(1, _dbService.GetObjectsAsync().Result.Count());
//        }
//        [TestMethod]
//        public void DeleteSandwich_InvalidId_ReturnsNull()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // Act
//            var deletedSandwich = service.DeleteSandwich(999);

//            // Assert
//            Assert.IsNull(deletedSandwich);
//        }
//        [TestMethod]
//        public void SearchSandwichByCategory_ValidCategory_ReturnsMatchingSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            string category = "Standard";

//            // Act
//            var result = service.SearchSandwichByCategory(category);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Ham & Cheese", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void SearchSandwichByCategory_InvalidCategory_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            string category = "NonExistentCategory";

//            // Act
//            var result = service.SearchSandwichByCategory(category);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void SearchSandwichByMeatType_ValidCategory_ReturnsMatchingSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            string meatType = "Turkey";

//            // Act
//            var result = service.SearchSandwichByMeatType(meatType);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Turkey & Salad", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void SearchSandwichByMeatType_InvalidCategory_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            string meatType = "NonExistentMeatType";

//            // Act
//            var result = service.SearchSandwichByMeatType(meatType);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterSandwichByIngredient_ValidIngredient_ReturnsMatchingSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            string ingredient = "Ham";

//            // Act
//            var result = service.FilterSandwichByIngredient(ingredient);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Ham & Cheese", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterSandwichByIngredient_InvalidIngredient_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            string ingredient = "NonExistentIngredient";

//            // Act
//            var result = service.FilterSandwichByIngredient(ingredient);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterSandwichByPriceLower_ValidPrice_ReturnsMatchingSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            double price = 20.00;

//            // Act
//            var result = service.FilterSandwichByPriceLower(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Turkey & Salad", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterSandwichByPriceLower_InvalidPrice_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            double price = 100.00;

//            // Act
//            var result = service.FilterSandwichByPriceLower(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterSandwichByPriceUpper_ValidPrice_ReturnsMatchingSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            double price = 20.00;

//            // Act
//            var result = service.FilterSandwichByPriceUpper(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Ham & Cheese", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void FilterSandwichByPriceUpper_InvalidPrice_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            double price = 10.00;

//            // Act
//            var result = service.FilterSandwichByPriceUpper(price);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void FilterSandwichByPriceRange_ValidRange_ReturnsMatchingSandwiches()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            double lower = 15.00;
//            double upper = 25.00;

//            // Act
//            var result = service.FilterSandwichByPriceRange(lower, upper);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//        }
//        [TestMethod]
//        public void FilterSandwichByPriceRange_InvalidRange_ReturnsEmptyList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);
//            double lower = 30.00;
//            double upper = 40.00;

//            // Act
//            var result = service.FilterSandwichByPriceRange(lower, upper);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count);
//        }
//        [TestMethod]
//        public void GetSandwichesSortedById_ReturnsSortedList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // Act
//            var result = service.GetSandwichesSortedById();

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Ham & Cheese", result[0].Ingredients);
//        }
//        [TestMethod]
//        public void GetSandwichesSortedByPrice_ReturnsSortedList()
//        {
//            // Arrange
//            var service = new SandwichService(_dbService);

//            // Act
//            var result = service.GetSandwichesSortedByPrice();

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//            Assert.AreEqual("Turkey & Salad", result[1].Ingredients);
//        }



//    }
//}