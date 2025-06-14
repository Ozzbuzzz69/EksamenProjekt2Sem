﻿using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EksamenProjekt2Sem.Services
{
    public class UserService : GenericDbService<User>
    {
        public List<User> _users;
        private GenericDbService<User> _dbService;

       
        public UserService(GenericDbService<User> genericDbService)
        {
            _dbService = genericDbService;
            try
            {
                _users = _dbService.GetObjectsAsync().Result.ToList();
                if (_users == null || _users.Count() < 1)
                {
                    SeedMockUsersAsync().Wait();
                    _users = _dbService.GetObjectsAsync().Result.ToList();
                }
            }
            catch (AggregateException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
            }
            if (_users == null)
            {
                _users = new();
            }
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
        public void UpdateUser(User user)
        {
            if (user == null)
                return;

            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
                return;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Password = user.Password;

            _dbService.UpdateObjectAsync(existingUser).Wait(); // Only update if the user exists
        }


        /// <summary>
        /// Deletes user with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User? DeleteUser(int id)
        {
            User? userToBeDeleted = null;
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
                _dbService.DeleteObjectAsync(userToBeDeleted).Wait();
            }
            return userToBeDeleted;
        }
    }
}