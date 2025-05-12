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
        public async Task AddObjectAsync_AddsItemToDatabase()
        {
            // Arrange
            var service = new GenericDbService<Food>();
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
    }
#endregion
}
