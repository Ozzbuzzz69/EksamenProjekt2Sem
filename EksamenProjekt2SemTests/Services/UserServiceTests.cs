//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using EksamenProjekt2Sem.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using EksamenProjektTest.EFDbContext;
//using EksamenProjekt2Sem.Models;

//namespace EksamenProjekt2Sem.Services.Tests
//{
//    [TestClass]
//    public class UserServiceTests
//    {
//        private DbContextOptions<FoodContext> _options;
//        private GenericDbService<User> _dbService;
//        [TestInitialize]
//        public void Setup()
//        {
//            _options = new DbContextOptionsBuilder<FoodContext>()
//                .UseInMemoryDatabase("TestDb_ReadUser")
//                .Options;

//            using (var context = new FoodContext(_options))
//            {
//                context.Database.EnsureDeleted(); // ren database for hver testkørsel
//                context.Database.EnsureCreated();

//                // Tilføjer en række Users til den midlertidige databasen
//                context.Users.AddRange(
//                    new User
//                    {
//                        Id = 1,
//                        Name = "TestUser1",
//                        Email = "tester@example.com",
//                        PhoneNumber = "12345678",
//                        Password = "password123"
//                    },
//                    new User
//                    {
//                        Id = 2,
//                        Name = "TestUser2",
//                        Email = "random@example2.com",
//                        PhoneNumber = "87654321",
//                        Password = "password456"
//                    },
//                    new User
//                    { 
//                        Id = 3,
//                        Name = "TestUser3",
//                        Email = "example@tester.com",
//                        PhoneNumber = "11223344",
//                        Password = "password789"
//                    });
//                context.SaveChanges();
//            }
//            _dbService = new GenericDbService<User>(_options);
//        }
//        [TestMethod]
//        public void CreateUserTest_AddsUserToDatabase()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            var newUser = new User
//            {
//                Id = 4,
//                Name = "TestUser4",
//                Email = "something@elpmaxe.moc",
//                PhoneNumber = "12345678",
//                Password = "password123"
//            };

//            // Act
//            service.CreateUser(newUser);

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var userInDb = context.Users.FirstOrDefault(u => u.Id == newUser.Id);
//                Assert.IsNotNull(userInDb);
//                Assert.AreEqual(newUser.Name, userInDb.Name);
//                Assert.AreEqual(newUser.Email, userInDb.Email);
//                Assert.AreEqual(newUser.PhoneNumber, userInDb.PhoneNumber);
//                Assert.AreEqual(newUser.Password, userInDb.Password);
//            }
//        }
//        [TestMethod]
//        public void GetUserByEmailTest_ValidEmail_ReturnsCorrectUser()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            string email = "tester@example.com";

//            // Act
//            var userToFind = service.GetUserByEmail(email);

//            // Assert
//            Assert.IsNotNull(userToFind);
//            Assert.AreEqual(email, userToFind.Email);
//            Assert.AreEqual("TestUser1", userToFind.Name);
//            Assert.AreEqual("12345678", userToFind.PhoneNumber);
//            Assert.AreEqual("password123", userToFind.Password);
//        }
//        [TestMethod]
//        public void GetUserByEmailTest_InvalidEmail_ReturnsNull()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            string email = "nonExistentEmail";

//            // Act
//            var userToFind = service.GetUserByEmail(email);

//            // Assert
//            Assert.IsNull(userToFind);
//        }
//        [TestMethod]
//        public void GetUserByIdTest_ValidId_ReturnsCorrectUser()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            int id = 1;

//            // Act
//            var userToFind = service.GetUserById(id);

//            // Assert
//            Assert.IsNotNull(userToFind);
//            Assert.AreEqual(id, userToFind.Id);
//            Assert.AreEqual("TestUser1", userToFind.Name);
//            Assert.AreEqual("tester@example.com", userToFind.Email);
//            Assert.AreEqual("12345678", userToFind.PhoneNumber);
//            Assert.AreEqual("password123", userToFind.Password);
//        }
//        [TestMethod]
//        public void GetUserByIdTest_InvalidId_ReturnsNull()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            int id = 999;

//            // Act
//            var userToFind = service.GetUserById(id);

//            // Assert
//            Assert.IsNull(userToFind);
//        }
//        [TestMethod]
//        public void ReadAllUsersTest_ReturnsAllUsers()
//        {
//            // Arrange
//            var service = new UserService(_dbService);

//            // Act
//            var users = service.ReadAllUsers();

//            // Assert
//            Assert.AreEqual(3, users.Count);
//            Assert.AreEqual("TestUser1", users[0].Name);
//            Assert.AreEqual("TestUser2", users[1].Name);
//            Assert.AreEqual("TestUser3", users[2].Name);
//        }
//        [TestMethod]
//        public void UpdateUserTest_ValidUser_UpdatesUserInDatabase()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            var newUser = new User
//            {
//                Id = 4,
//                Name = "TestUser4",
//                Email = "something@elpmaxe.moc",
//                PhoneNumber = "12345678",
//                Password = "password123"
//            };

//            // Act
//            service.UpdateUser(1, newUser);

//            // Assert
//            using (var context = new FoodContext(_options))
//            {
//                var userInDb = context.Users.FirstOrDefault(u => u.Id == 1);
//                Assert.IsNotNull(userInDb);
//                Assert.AreEqual(newUser.Name, userInDb.Name);
//                Assert.AreEqual(newUser.Email, userInDb.Email);
//                Assert.AreEqual(newUser.PhoneNumber, userInDb.PhoneNumber);
//                Assert.AreEqual(newUser.Password, userInDb.Password);
//            }
//        }
//        [TestMethod]
//        public void UpdateUserTest_InvalidUser_ThrowsException()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            int invalidId = 999;
//            var newUser = new User
//            {
//                Id = 4,
//                Name = "TestUser4",
//                Email = "something@elpmaxe.moc",
//                PhoneNumber = "12345678",
//                Password = "password123"
//            };

//            // Act & Assert
//            Assert.ThrowsException<Exception>(() => service.UpdateUser(invalidId, newUser));
//        }
//        [TestMethod]
//        public void DeleteUserTest_ValidId_DeletesOrderFromDatabase()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            int userIdToDelete = 1;

//            // Act
//            var deletedUser = service.DeleteUser(userIdToDelete);

//            // Assert
//            Assert.IsNotNull(deletedUser);
//        }
//        [TestMethod]
//        public void DeleteUserTest_InvalidId_ThrowsException()
//        {
//            // Arrange
//            var service = new UserService(_dbService);
//            int invalidId = 999;

//            // Act & Assert
//            Assert.ThrowsException<Exception>(() => service.DeleteUser(invalidId));
//        }
//    }
//}