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

        public CampaignOfferServiceTests()
        {
            _options = new DbContextOptions<FoodContext>();
            _dbService = new GenericDbService<CampaignOffer>(_options);
        }

        [TestInitialize]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseInMemoryDatabase("TestDb_CampaignOffers")
                .Options;

            using (var context = new FoodContext(_options))
            {
                context.Database.EnsureDeleted(); // ren database for hver testkørsel

                // Tilføjer en række offers til den midlertidige databasen
                context.CampaignOffers.AddRange(
                    new CampaignOffer 
                    { 
                        //Id = 1, 
                        Name = "Offer A", 
                        ImageLink = "img1.jpg", 
                        Price = 10.99,
                        StartTime = DateTime.Now.AddDays(-1),
                        EndTime = DateTime.Now.AddMonths(1)
                    },
                    new CampaignOffer 
                    { 
                        //Id = 2, 
                        Name = "Offer B", 
                        ImageLink = "img2.jpg", 
                        Price = 8.50,
                        StartTime = DateTime.Now.AddDays(1),
                        EndTime = DateTime.Now.AddMonths(1)
                    }
                );
                context.SaveChanges();
            }
            _dbService = new GenericDbService<CampaignOffer>(_options);
        }

        [TestMethod]
        public async Task CreateCampaignOffer_AddsOfferToDatabase()
        {
            // Arrange
            var newOffer = new CampaignOffer {Name = "Offer C", ImageLink = "img3.jpg", Price = 12.50 };

            // Act
            using (var context = new FoodContext(_options))
            {
                var database = new GenericDbService<CampaignOffer>(_options);
                var service = new CampaignOfferService(database);
                await context.Database.EnsureDeletedAsync();
                await service.CreateCampaignOffer(newOffer);
            }
            // Assert
            using (var context = new FoodContext(_options))
            {
                var result = await context.CampaignOffers.FindAsync(newOffer.Id);

                Assert.IsNotNull(result);
                Assert.AreEqual("Offer C", newOffer.Name);
                Assert.AreEqual("img3.jpg", newOffer.ImageLink);
                Assert.AreEqual(12.50, newOffer.Price);
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
        public void ReadCampaignOffer_InvalidId_ThrowsException()
        {
            // Arrange
            var dbService = new GenericDbService<CampaignOffer>(_options);
            var service = new CampaignOfferService(dbService);

            // Act & Assert
            var ex = Assert.ThrowsException<Exception>(() => service.ReadCampaignOffer(999));
            Assert.AreEqual("CampaignOffer with id 999 not found.", ex.Message);
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
            var updatedOffer = new CampaignOffer { Id = 999, Name = "Non-existent Offer", ImageLink = "non_existent.jpg", Price = 0.00 };

            // Act
            using (var context = new FoodContext(_options))
            {
                var database = new GenericDbService<CampaignOffer>(_options);
                var service = new CampaignOfferService(database);
                service.UpdateCampaignOffer(updatedOffer);

            // Assert
                var result = context.CampaignOffers.Find(999); //Try to find the non-existent offer
                Assert.IsNull(result, "Offer should not be updated in the database");
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

            // Act
            var deletedOffer = service.DeleteCampaignOffer(999);

            // Assert
            Assert.IsNull(deletedOffer);
        }
        [TestMethod]
        public async Task SetOfferValidities_UpdatesIsActiveProperty()
        {
            // Arrange
            var offerToValidate = new CampaignOffer
            {
                Name = "Offer A",
                ImageLink = "img1.jpg",
                Price = 10.99,
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddMonths(1)
            };

            bool beforeValidation = offerToValidate.IsActive;

            // Act
            using (var context = new FoodContext(_options))
            {
                var database = new GenericDbService<CampaignOffer>(_options);
                var service = new CampaignOfferService(database);

                // Change the status of the offer to be valid
                offerToValidate.IsActive = true;


                // Check if the status is changed
                Assert.AreNotEqual(beforeValidation, offerToValidate.IsActive);

                // Add the offer to the database
                database.AddObjectAsync(offerToValidate).Wait();

                // Set the validities of the offers
                service.SetOfferValidities();

                // Assert
                var updatedOffer = await database.GetObjectByIdAsync(offerToValidate.Id);
                Assert.AreEqual(beforeValidation, updatedOffer.IsActive);
            }
        }
        [TestMethod]
        public void CheckOfferValidity_ValidOffer_ReturnsTrue()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var offer = new CampaignOffer { StartTime = DateTime.Now.AddDays(-1), EndTime = DateTime.Now.AddDays(1) };

            // Act
            bool isValid = service.CheckOfferValidity(offer);

            // Assert
            Assert.IsTrue(isValid);
        }
        [TestMethod]
        public void CheckOfferValidity_InvalidOffer_ReturnsFalse()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var offer = new CampaignOffer { StartTime = DateTime.Now.AddDays(1), EndTime = DateTime.Now.AddDays(2) };

            // Act
            bool isValid = service.CheckOfferValidity(offer);

            // Assert
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void CheckOfferValidity_OfferNotStarted_ReturnsFalse()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var offer = new CampaignOffer { StartTime = DateTime.Now.AddDays(1), EndTime = DateTime.Now.AddDays(2) };

            // Act
            bool isValid = service.CheckOfferValidity(offer);

            // Assert
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void CheckOfferValidity_OfferExpired_ReturnsFalse()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var offer = new CampaignOffer { StartTime = DateTime.Now.AddDays(-2), EndTime = DateTime.Now.AddDays(-1) };

            // Act
            bool isValid = service.CheckOfferValidity(offer);

            // Assert
            Assert.IsFalse(isValid);
        }
        [TestMethod]
        public void CheckOfferValidity_NullStartTime_ReturnsTrue()
        {
            // Arrange
            var service = new CampaignOfferService(_dbService);
            var offer = new CampaignOffer { StartTime = null, EndTime = DateTime.Now.AddDays(1) };

            // Act
            bool isValid = service.CheckOfferValidity(offer);

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}