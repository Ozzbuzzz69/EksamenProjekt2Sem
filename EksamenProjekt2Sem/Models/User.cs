using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class User
    {
        
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Password { get; private set; }

        public User()
        { }

        /// <summary>
        /// Constructor for User class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        public User(int id, string name, string email, string phoneNumber, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
        }
        /// <summary>
        /// Function to set otherwise private id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Function to set otherwise private name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Function to set otherwise private email
        /// </summary>
        /// <param name="email"></param>
        public void SetEmail(string email)
        {
            Email = email;
        }
        /// <summary>
        /// Function to set otherwise private phone number
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
        /// <summary>
        /// Function to set otherwise private password
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            Password = password;
        }
        /// <summary>
        /// Function to validate password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>bool</returns>
        public bool ValidatePassword(string password)
        {
            return Password == password;
        }
        /// <summary>
        /// Function to validate email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public bool ValidateEmail(string email)
        {
            return Email == email;
        }

    }
}
