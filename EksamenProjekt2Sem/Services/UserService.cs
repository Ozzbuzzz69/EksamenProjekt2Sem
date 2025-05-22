using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EksamenProjekt2Sem.Services
{
    public class UserService : GenericDbService<User>
    {
        public List<User> _users;// Overskud fra domain model
        private GenericDbService<User> _dbService; // Overskud fra domain model

       
        public UserService(GenericDbService<User> genericDbService)
        {
            _dbService = genericDbService;
            
            if (_users == null)
            {
                _users = MockUser.GetMockUsers();
            }
            else
                _users = _dbService.GetObjectsAsync().Result.ToList();
        }
        //Getting mock data into the database

        public async Task SeedMockUsersAsync()
        {
            _users = new List<User>();
            var users = MockUser.GetMockUsers();
            await _dbService.SaveObjects(users);
        }

        public async Task CreateUser(User user)
        {
            await _dbService.AddObjectAsync(user);
        }


        
        public User? GetUserByEmail(string email)
        {
            return _users.Find(user => user.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var users = await _dbService.GetObjectsAsync();
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetUserById(int id)
        {
            return _users.Find(user => user.Id == id);
        }

        /// <summary>
        /// Reads all users. 
        /// </summary>
        /// <returns></returns>

        public async Task<List<User>> ReadAllUsersAsync()
        {
            var users = await _dbService.GetObjectsAsync();
            return users.ToList();
        }
        /// <summary>
        /// Updates user with given id. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public void UpdateUser(int id, User user)
        {
            if (user != null)
            { 
                 foreach (User u in _users)
                 {
                    if (u.Id == id)
                    {
                         u.Name = user.Name;
                         u.Email = user.Email;
                         u.PhoneNumber = user.PhoneNumber;
                         u.Password = user.Password;
                    }
                 }
            }
            _dbService.UpdateObjectAsync(user).Wait();
        }

        /// <summary>
        /// Deletes user with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User DeleteUser(int id)
        {
            User userToBeDeleted = null;
            foreach (User u in _users)
            {
                if (u.Id == id)
                {
                    userToBeDeleted = u;
                }
            }
            if (userToBeDeleted != null)
            {
                _users.Remove(userToBeDeleted);
                _dbService.DeleteObjectAsync(userToBeDeleted);
            }
            return userToBeDeleted;
        }



        



        //public Order Order(List<OrderLine> orderlines)
        //{
        //    // Create order from orderlines
        //    return new Order(); // Placeholder return
        //}
        //public User GetUserOrders(User user)
        //{
        //    return (User)(from o in Orders
        //                   where o.User.Id == GetObjectByIdAsync(user.Id).Result.Id
        //                   select o);

        //}


    }
}
