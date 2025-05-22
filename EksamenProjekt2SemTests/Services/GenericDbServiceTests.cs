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
        [TestMethod]
        public void SortByCriteria_SortsSandwichesByPriceAscending()
        {
            // Arrange
            var service = new GenericDbService<Sandwich>();

            var sandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, Ingredients = "A", MeatType = "Ham", Price = 29.95, Category = "Premium" },
                new Sandwich { Id = 2, Ingredients = "B", MeatType = "Turkey", Price = 19.95, Category = "Standard" },
                new Sandwich { Id = 3, Ingredients = "C", MeatType = "Beef", Price = 24.95, Category = "Deluxe" }
            };

            // Act
            var sorted = service.SortByCriteria(sandwiches, "Price");

            // Assert
            Assert.AreEqual(3, sorted.Count);
            Assert.AreEqual(2, sorted[0].Id); // Price 19.95
            Assert.AreEqual(3, sorted[1].Id); // Price 24.95
            Assert.AreEqual(1, sorted[2].Id); // Price 29.95
        }
        [TestMethod]
        public void SortByCriteria_SortsByMeatTypeAlphabetically()
        {
            var service = new GenericDbService<Sandwich>();
            var sandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, MeatType = "Turkey" },
                new Sandwich { Id = 2, MeatType = "Beef" },
                new Sandwich { Id = 3, MeatType = "Ham" }
            };

            var result = service.SortByCriteria(sandwiches, "MeatType");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[0].Id); // Beef
            Assert.AreEqual(3, result[1].Id); // Ham
            Assert.AreEqual(1, result[2].Id); // Turkey
        }
        [TestMethod]
        public void SortByCriteria_InvalidProperty_ReturnsUnsorted()
        {
            var service = new GenericDbService<Sandwich>();
            var sandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, Price = 25 },
                new Sandwich { Id = 2, Price = 20 },
                new Sandwich { Id = 3, Price = 30 }
            };

            var result = service.SortByCriteria(sandwiches, "NotARealProperty");

            // Forvent: Ingen sortering, samme rækkefølge som input
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);
        }
        [TestMethod]
        public void SortByCriteria_IgnoresItemsWithNullProperty()
        {
            var service = new GenericDbService<Sandwich>();
            var sandwiches = new List<Sandwich>
            {
                new Sandwich { Id = 1, Ingredients = "Deluxe" },
                new Sandwich { Id = 2, Ingredients = null },          // skal ignoreres
                new Sandwich { Id = 3, Ingredients = "Standard" }
            };

            var result = service.SortByCriteria(sandwiches, "Ingredients");

            Assert.AreEqual(3, result.Count); // kun 2 med ikke-null Category
            Assert.AreEqual(1, result[0].Id); // Deluxe
            Assert.AreEqual(3, result[1].Id); // Standard
            Assert.AreEqual(2, result[2].Id); // null
        }
    }
    #endregion

    #region Test af database

    [TestClass]
    public class GenericDbServiceIntegrationTests
    {
        private DbContextOptions<FoodContext> _options;

        public GenericDbServiceIntegrationTests() { _options = new DbContextOptions<FoodContext>(); }

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }


        [TestMethod]
        public async Task GetObjectsAsync_ReturnsAllItems()
        {
            // Arrange: Tilføj 2 sandwich-objekter til InMemory-databasen
            using (var context = new FoodContext(_options))
            {
                context.Database.EnsureDeleted(); // ren database for hver testkørsel
                context.Sandwiches.Add(new Sandwich {Ingredients = "Beef", MeatType = "Beef", Price = 24.95, Category = "Standard" });
                context.Sandwiches.Add(new Sandwich {Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard" });
                await context.SaveChangesAsync();
            }

            IEnumerable<Sandwich> result;

            // Act: Brug GenericDbService til at hente dem
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>(_options);
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
            var item = new Sandwich { Id = 1, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard" };

            // Act
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>(_options);
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
                new Sandwich {Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard"},
                new Sandwich {Ingredients = "Pork", MeatType = "Pork", Price = 14.95, Category = "Standard"}
            };

            // Act
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>(_options);
                context.Database.EnsureDeleted();
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
        [TestMethod]
        public async Task DeleteObjectAsync_RemovesItemFromDatabase()
        {
            // Arrange: Tilføj én sandwich til databasen
            var sandwichToDelete = new Sandwich { Id = 1, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard" };

            using (var context = new FoodContext(_options))
            {
                context.Sandwiches.Add(sandwichToDelete);
                await context.SaveChangesAsync();
            }

            // Act: Slet sandwich med DeleteObjectAsync
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>(_options);
                await service.DeleteObjectAsync(sandwichToDelete);
            }

            // Assert: Sørg for at databasen nu er tom
            using (var context = new FoodContext(_options))
            {
                var remaining = await context.Sandwiches.ToListAsync();
                Assert.AreEqual(0, remaining.Count, "Sandwich blev ikke slettet som forventet.");
            }
        }
        [TestMethod]
        public async Task UpdateObjectAsync_UpdatesItemInDatabase()
        {
            // Arrange: Tilføj én sandwich til databasen
            var sandwichToUpdate = new Sandwich { Id = 1, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard" };

            using (var context = new FoodContext(_options))
            {
                context.Sandwiches.Add(sandwichToUpdate);
                await context.SaveChangesAsync();
            }

            // Act: Ændre sandwichens ingredienser
            sandwichToUpdate.Ingredients = "Turkey & Cheese";

            // Act: Update sandwich med UpdateObjectAsync
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>(_options);
                await service.UpdateObjectAsync(sandwichToUpdate);
            }

            // Assert: Sørg for at databasen ikke indeholder den gamle sandwich, men den opdaterede
            using (var context = new FoodContext(_options))
            {
                var saved = await context.Sandwiches.ToListAsync();
                Assert.IsFalse(saved.Any(s => s.Ingredients == "Ham & Cheese"), "Den gamle Sandwich blev ikke opdateret som forventet.");
                Assert.IsTrue(saved.Any(s => s.Ingredients == "Turkey & Cheese"), "Den nye Sandwich fremkom ikke som forventet.");
            }
        }
        [TestMethod]
        public async Task GetObjectByIdAsync_GetsItemByIdInDatabase()
        {
            // Arrange: Tilføj to sandwiches til databasen
            var sandwichToGet = new Sandwich { Id = 1, Ingredients = "Ham & Cheese", MeatType = "Ham", Price = 19.95, Category = "Standard" };
            var sandwichToIgnore = new Sandwich { Id = 2, Ingredients = "Pork", MeatType = "Pork", Price = 14.95, Category = "Standard" };

            using (var context = new FoodContext(_options))
            {
                context.Sandwiches.Add(sandwichToGet);
                await context.SaveChangesAsync();
            }

            // Act: Hent sandwich med GetObjectByIdAsync
            Sandwich result;
            using (var context = new FoodContext(_options))
            {
                var service = new GenericDbService<Sandwich>(_options);
                result = await service.GetObjectByIdAsync(1);
            }

            // Assert: Sørg for at den hentede sandwich er den samme som den der blev ledt efter
            Assert.IsNotNull(result);
            Assert.AreEqual("Ham & Cheese", result.Ingredients, "Den hentede sandwich er ikke den forventede.");
        }


    }
    #endregion
}
