using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EksamenProjekt2Sem.Services
{
    public class UserService : GenericDbService<User>
    {
        public List<Order> Orders { get; }
        public List<User> Users { get; }// Overskud fra domain model
        private GenericDbService<User> _dbService; // Overskud fra domain model

       
        public UserService(GenericDbService<User> genericDbService)
        {
            _dbService = genericDbService;

        }
       

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user)
        {
            Users.Add(user);
            _dbService.AddObjectAsync(user);
        }

        /// <summary>
        /// Finds user with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            return Users.Find(user => user.Email == email);
        }

        /// <summary>
        /// Reads all users. 
        /// </summary>
        /// <returns></returns>
        public List<User> ReadAllUsers()
        {
            return Users;
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
                 foreach (User u in Users)
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
            _dbService.SaveObjects(Users);
        }

        /// <summary>
        /// Deletes user with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User DeleteUser(int id)
        {
            User userToBeDeleted = null;
            foreach (User u in Users)
            {
                if (u.Id == id)
                {
                    userToBeDeleted = u;
                }
            }
            if (userToBeDeleted != null)
            {
                Users.Remove(userToBeDeleted);
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
