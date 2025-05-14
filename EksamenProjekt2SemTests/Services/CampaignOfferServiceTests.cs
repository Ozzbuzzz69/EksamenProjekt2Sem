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

namespace EksamenProjekt2Sem.Services.Tests
{
    [TestClass]
    public class CampaignOfferServiceTests
    {
        private DbContextOptions<FoodContext> _options;
        private GenericDbService<CampaignOffer> _dbService;

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseInMemoryDatabase("TestDb_CampaignOffers")
                .Options;

            using (var context = new FoodContext(_options))
            {
                context.Database.EnsureDeleted(); // ren database for hver testkørsel
                context.Database.EnsureCreated();

                // Tilføjer en række offers til den midlertidige databasen
                context.CampaignOffers.AddRange(
                    new CampaignOffer { Id = 1, Name = "Offer A", ImageLink = "img1.jpg", Price = 10.99 },
                    new CampaignOffer { Id = 2, Name = "Offer B", ImageLink = "img2.jpg", Price = 8.50 }
                );
                context.SaveChanges();
            }
            _dbService = new GenericDbService<CampaignOffer>(_options);
        }
        [TestMethod]
        public void CreateCampaignOffer_ShouldAddNewOffer()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var newOffer = new CampaignOffer { Id = 3, Name = "Offer C", ImageLink = "img3.jpg", Price = 12.50 };

            // Act
            service.CreateCampaignOffer(newOffer);

            // Assert
            using (var context = new FoodContext(_options))
            {
                var offers = context.CampaignOffers.ToList();
                Assert.AreEqual(3, offers.Count);
                Assert.AreEqual("Offer C", offers[2].Name);
            }
        }
        [TestMethod]
        public void ReadCampaignOffer_ValidId_ReturnsCorrectOffer()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            int offerId = 1;

            // Act
            var offer = service.ReadCampaignOffer(offerId);

            // Assert
            Assert.IsNotNull(offer);
            Assert.AreEqual(offerId, offer.Id);
            Assert.AreEqual("Offer A", offer.Name);
            Assert.AreEqual("img1.jpg", offer.ImageLink);
            Assert.AreEqual(10.99, offer.Price);
        }
        [TestMethod]
        public void ReadCampaignOffer_InvalidId_ReturnsNull()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            int invalidId = 999;

            // Act
            var offer = service.ReadCampaignOffer(invalidId);

            // Assert
            Assert.IsNull(offer);
        }
        [TestMethod]
        public void ReadAllCampaignOffers_ReturnsAllOffers()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);

            // Act
            var offers = service.ReadAllCampaignOffers();

            // Assert
            Assert.AreEqual(2, offers.Count);
            Assert.AreEqual("Offer A", offers[0].Name);
            Assert.AreEqual("Offer B", offers[1].Name);
        }
        [TestMethod]
        public void UpdateCampaignOffer_ValidId_UpdatesOffer()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var updatedOffer = new CampaignOffer { Id = 1, Name = "Updated Offer A", ImageLink = "updated_img1.jpg", Price = 11.99 };

            // Act
            service.UpdateCampaignOffer(updatedOffer);

            // Assert
            using (var context = new FoodContext(_options))
            {
                var offer = context.CampaignOffers.Find(1);
                Assert.IsNotNull(offer);
                Assert.AreEqual("Updated Offer A", offer.Name);
                Assert.AreEqual("updated_img1.jpg", offer.ImageLink);
                Assert.AreEqual(11.99, offer.Price);
            }
        }
        [TestMethod]
        public void UpdateCampaignOffer_InvalidId_DoesNotUpdate()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var updatedOffer = new CampaignOffer { Id = 999, Name = "Non-existent Offer", ImageLink = "non_existent.jpg", Price = 0.00 };

            // Act
            service.UpdateCampaignOffer(updatedOffer);

            // Assert
            using (var context = new FoodContext(_options))
            {
                var offer = context.CampaignOffers.Find(1);
                Assert.IsNotNull(offer);
                Assert.AreEqual("Offer A", offer.Name); // Should not be updated
            }
        }
        [TestMethod]
        public void DeleteCampaignOffer_ValidId_DeletesOffer()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            int offerId = 1;

            // Act
            var deletedOffer = service.DeleteCampaignOffer(offerId);

            // Assert
            using (var context = new FoodContext(_options))
            {
                var offer = context.CampaignOffers.Find(offerId);
                Assert.IsNull(offer);
                Assert.AreEqual("Offer A", deletedOffer.Name);
            }
        }
        [TestMethod]
        public void DeleteCampaignOffer_InvalidId_ReturnsNull()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            int invalidId = 999;

            // Act
            var deletedOffer = service.DeleteCampaignOffer(invalidId);

            // Assert
            Assert.IsNull(deletedOffer);
        }
    }
}