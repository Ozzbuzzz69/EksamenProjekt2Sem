using Microsoft.VisualStudio.TestTools.UnitTesting;
using EksamenProjekt2Sem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using EksamenProjekt2Sem.Models;


namespace EksamenProjekt2Sem.Services.Tests
{
#region Test af funktioner

    [TestClass]
    public class GenericDbServiceTests
    {
        public class DummyClass
        {
            public int Id { get; set; }
        }

        [TestMethod]
        public void SortById_SortsCorrectly()
        {
            // Arrange
            var service = new GenericDbService<DummyClass>();
            var list = new List<DummyClass>
        {
            new DummyClass { Id = 5 },
            new DummyClass { Id = 2 },
            new DummyClass { Id = 9 }
        };

            // Act
            var sorted = service.SortById(list);

            // Assert
            Assert.AreEqual(2, sorted[0].Id);
            Assert.AreEqual(5, sorted[1].Id);
            Assert.AreEqual(9, sorted[2].Id);
        }


    }
#endregion

#region Test af database

    [TestClass]
    public class GenericDbServiceIntegrationTests
    {
        private DbContextOptions<FoodContext> _options;

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // Ny hver gang
                .Options;
        }

        [TestMethod]
        public async Task GetObjectsAsync_ReturnsAllItems()
        {
            // Arrange: Tilføj 2 sandwich-objekter til InMemory-databasen
            using (var context = new FoodContext(_options))
            {
                context.Sandwiches.Add(new Sandwich { Id = 1, Ingredients = "Beef", MeatType = "Beef", Price = 24.95, Category = "Standard" });
                context.Sandwiches.Add(new Sandwich { Id = 2, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard" });
                await context.SaveChangesAsync();
            }

            IEnumerable<Sandwich> result;

            // Act: Brug GenericDbService til at hente dem
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>();
                result = await service.GetObjectsAsync();
            }

            // Assert: Kontroller, at der returneres 2 sandwiches
            Assert.IsNotNull(result);
            var list = result.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Any(s => s.Ingredients == "Beef"));
            Assert.IsTrue(list.Any(s => s.Ingredients == "Ham & Cheese"));
        }

        [TestMethod]
        public async Task AddObjectAsync_AddsItemToDatabase()
        {
            // Arrange
            var service = new GenericDbService<Sandwich>();
            var item = new Sandwich { Id = 1, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard"};

            // Act
            using (var context = new FoodContext(_options))
            {
                context.Database.EnsureDeleted();
                await service.AddObjectAsync(item);
            }

            // Assert
            using (var context = new FoodContext(_options))
            {
                var result = await context.Sandwiches.FindAsync(1);
                Assert.IsNotNull(result);
                Assert.AreEqual("Ham & Cheese", result.Ingredients);
            }
        }
        [TestMethod]
        public async Task SaveObjects_SavesAllItemsToDatabase()
        {
            // Arrange
            var sandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard"},
                new Sandwich { Id = 2, Ingredients = "Pork", MeatType = "Pork", Price = 14.95, Category = "Standard"}
            };

            // Act
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>();
                await service.SaveObjects(sandwiches);
            }

            // Assert
            using (var context = new FoodContext(_options))
            {
                var saved = await context.Sandwiches.ToListAsync();

                Assert.AreEqual(2, saved.Count, "Der blev ikke gemt det forventede antal sandwiches.");
                Assert.IsTrue(saved.Any(s => s.Ingredients == "Ham & Cheese"), "Sandwich 'Ham & Cheese' blev ikke fundet.");
                Assert.IsTrue(saved.Any(s => s.Ingredients == "Pork"), "Sandwich 'Pork' blev ikke fundet.");
            }
        }

    }
    #endregion
}
